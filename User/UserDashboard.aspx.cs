using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;

namespace Authentication.User
{
	public partial class UserDashboard : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (Session["UserID"] == null || Session["Role"] == null)
				{
					Response.Redirect("~/Login.aspx");
					return;
				}

				SetCardVisibility();
				LoadDashboardStats();
				LoadUpcomingEvents();

				int userId = Convert.ToInt32(Session["UserID"]);
				CheckApprovedApplication(userId);
			}
		}

		// ✅ Set visibility of cards based on role
		private void SetCardVisibility()
		{
			string role = Session["Role"].ToString();

			// Organizer cards
			phEventsCreated.Visible = (role == "Organizer");
			phParticipantsRegistered.Visible = (role == "Organizer");
			phCreateEvent.Visible = (role == "Organizer");
			phMyOrganizedEvents.Visible = (role == "Organizer");
			phVolunteerApplications.Visible = (role == "Organizer");
			phManageAttendance.Visible = (role == "Organizer");
			phViewFeedback.Visible = (role == "Organizer");

			// Volunteer cards
			phEventsVolunteered.Visible = (role == "Volunteer");
			phDutiesAssigned.Visible = (role == "Volunteer");
			phAssignedDuties.Visible = (role == "Volunteer");
			phMarkAttendance.Visible = (role == "Volunteer");
			phVolunteerStatus.Visible = (role == "Volunteer");

			// Participant cards
			phParticipationCount.Visible = (role == "Participant");
			phParticipationHistory.Visible = (role == "Participant");
			phFeedback.Visible = (role == "Participant");

			// Shared cards
			phMyVolunteerEvents.Visible = (role == "Participant" || role == "Volunteer");
			phBrowseEvents.Visible = true;

			// Certificates shown only to Volunteer and Participant
			phCertificatesEarned.Visible = (role == "Participant" || role == "Volunteer");
		}

		// ✅ Load statistics based on role
		private void LoadDashboardStats()
		{
			int userId = Convert.ToInt32(Session["UserID"]);
			string role = Session["Role"].ToString();
			string connStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();

				if (role == "Participant" || role == "Volunteer")
				{
					SqlCommand cmdCertificates = new SqlCommand(
						"SELECT COUNT(*) FROM Certificates WHERE UserID = @UserID", conn);
					cmdCertificates.Parameters.AddWithValue("@UserID", userId);
					litCertificatesEarned.Text = cmdCertificates.ExecuteScalar().ToString();
				}

				if (role == "Organizer")
				{
					SqlCommand cmdCreated = new SqlCommand(
						"SELECT COUNT(*) FROM Events WHERE OrganizerID = @UserID", conn);
					cmdCreated.Parameters.AddWithValue("@UserID", userId);
					litEventsCreated.Text = cmdCreated.ExecuteScalar().ToString();

					SqlCommand cmdParticipants = new SqlCommand(@"
                        SELECT COUNT(DISTINCT VA.UserID)
                        FROM VolunteerApplications VA
                        INNER JOIN Events E ON VA.EventID = E.EventID
                        INNER JOIN Users U ON VA.UserID = U.UserID
                        WHERE E.OrganizerID = @UserID AND U.Role = 'Participant'", conn);
					cmdParticipants.Parameters.AddWithValue("@UserID", userId);
					litParticipantsRegistered.Text = cmdParticipants.ExecuteScalar().ToString();
				}

				if (role == "Volunteer")
				{
					SqlCommand cmdDuties = new SqlCommand(
						"SELECT COUNT(*) FROM VolunteerDuties WHERE VolunteerID = @UserID", conn);
					cmdDuties.Parameters.AddWithValue("@UserID", userId);
					litDutiesAssigned.Text = cmdDuties.ExecuteScalar().ToString();

					SqlCommand cmdEventsVolunteered = new SqlCommand(@"
                        SELECT COUNT(DISTINCT EventID)
                        FROM VolunteerApplications
                        WHERE UserID = @UserID AND Status = 'Approved'", conn);
					cmdEventsVolunteered.Parameters.AddWithValue("@UserID", userId);
					litEventsVolunteered.Text = cmdEventsVolunteered.ExecuteScalar().ToString();
				}

				if (role == "Participant")
				{
					SqlCommand cmdParticipation = new SqlCommand(@"
                        SELECT COUNT(DISTINCT EventID)
                        FROM ParticipantRegistrations
                        WHERE UserID = @UserID", conn);
					cmdParticipation.Parameters.AddWithValue("@UserID", userId);
					litParticipationCount.Text = cmdParticipation.ExecuteScalar().ToString();
				}
			}
		}

		// ✅ Show modal if an approved application exists (only once)
		private void CheckApprovedApplication(int userId)
		{
			try
			{
				string connStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
				using (SqlConnection conn = new SqlConnection(connStr))
				{
					conn.Open();
					string query = @"
                        SELECT TOP 1 ApplicationID
                        FROM VolunteerApplications
                        WHERE UserID=@UID AND Status='Approved' AND IsNotificationShown=0";

					SqlCommand cmd = new SqlCommand(query, conn);
					cmd.Parameters.AddWithValue("@UID", userId);

					object result = cmd.ExecuteScalar();
					if (result != null)
					{
						int applicationId = Convert.ToInt32(result);

						// Store in ViewState for download
						ViewState["ApprovedApplicationID"] = applicationId;

						// Mark as shown
						SqlCommand updateCmd = new SqlCommand(
							"UPDATE VolunteerApplications SET IsNotificationShown=1 WHERE ApplicationID=@AID", conn);
						updateCmd.Parameters.AddWithValue("@AID", applicationId);
						updateCmd.ExecuteNonQuery();

						// Trigger Bootstrap modal
						ScriptManager.RegisterStartupScript(
							this, this.GetType(),
							"ShowApprovedModal",
							"var myModal = new bootstrap.Modal(document.getElementById('approvedModal')); myModal.show();",
							true);
					}
				}
			}
			catch
			{
				// optional: log error
			}
		}

		// ✅ On click of "Download ID Pass" in modal
		protected void btnDownloadID_Click(object sender, EventArgs e)
		{
			if (ViewState["ApprovedApplicationID"] != null)
			{
				int appId = Convert.ToInt32(ViewState["ApprovedApplicationID"]);
				Response.Redirect("~/User/DownloadID.aspx?appid=" + appId);
			}
		}

		// ✅ Load upcoming events
		private void LoadUpcomingEvents()
		{
			int userId = Convert.ToInt32(Session["UserID"]);
			string connStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

			using (SqlConnection conn = new SqlConnection(connStr))
			{
				conn.Open();
				string query = @"
                    SELECT DISTINCT E.EventID, E.Title, E.Date, E.Location
                    FROM Events E
                    LEFT JOIN VolunteerApplications VA ON E.EventID = VA.EventID AND VA.Status = 'Approved'
                    WHERE (VA.UserID = @UserID OR E.OrganizerID = @UserID)
                      AND E.Date >= GETDATE()
                    ORDER BY E.Date ASC";

				SqlCommand cmd = new SqlCommand(query, conn);
				cmd.Parameters.AddWithValue("@UserID", userId);

				SqlDataReader reader = cmd.ExecuteReader();
				StringBuilder html = new StringBuilder();

				while (reader.Read())
				{
					string title = reader["Title"].ToString();
					string location = reader["Location"].ToString();
					string eventId = reader["EventID"].ToString();
					DateTime date = Convert.ToDateTime(reader["Date"]);

					html.Append("<div class='card'>");
					html.Append($"<h3>{title}</h3>");
					html.Append($"<p><strong>Date:</strong> {date:dd MMM yyyy}</p>");
					html.Append($"<p><strong>Location:</strong> {location}</p>");
					html.Append($"<a href='EventDetails.aspx?eventid={eventId}' class='submit-button' style='margin-top:10px;'>Details</a>");
					html.Append("</div>");
				}

				litUpcomingEvents.Text = html.ToString();
			}
		}
	}
}
