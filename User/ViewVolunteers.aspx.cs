using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Authentication.User
{
	public partial class ViewVolunteers : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (Request.QueryString["eventid"] == null)
				{
					lblMessage.Text = "Invalid event!";
					lblMessage.CssClass = "text-danger";
					lblMessage.Visible = true;
					return;
				}

				int eventId;
				if (!int.TryParse(Request.QueryString["eventid"], out eventId))
				{
					lblMessage.Text = "Invalid event!";
					lblMessage.CssClass = "text-danger";
					lblMessage.Visible = true;
					return;
				}

				LoadVolunteersGrouped(eventId);
			}
		}

		/// <summary>
		/// Handles Approve / Reject commands from each volunteer row
		/// </summary>
		protected void rptVolunteers_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			try
			{
				if (e.CommandName == "Approve" || e.CommandName == "Reject")
				{
					int applicationId = Convert.ToInt32(e.CommandArgument);
					string newStatus = (e.CommandName == "Approve") ? "Approved" : "Rejected";
					UpdateVolunteerStatus(applicationId, newStatus);
				}
			}
			catch (Exception ex)
			{
				lblMessage.Text = "❌ Error processing command: " + ex.Message;
				lblMessage.CssClass = "text-danger";
				lblMessage.Visible = true;
			}
		}

		/// <summary>
		/// Updates the volunteer application status and, if approved, inserts a duty.
		/// </summary>
		private void UpdateVolunteerStatus(int applicationId, string newStatus)
		{
			try
			{
				string connStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
				using (SqlConnection conn = new SqlConnection(connStr))
				{
					conn.Open();

					// ✅ 1. Update status in VolunteerApplications
					string updateSql = "UPDATE VolunteerApplications SET Status=@Status";
					if (newStatus == "Approved")
					{
						updateSql += ", IsNotificationShown = 0";
					}
					updateSql += " WHERE ApplicationID=@ID";

					using (SqlCommand cmd = new SqlCommand(updateSql, conn))
					{
						cmd.Parameters.AddWithValue("@Status", newStatus);
						cmd.Parameters.AddWithValue("@ID", applicationId);
						cmd.ExecuteNonQuery();
					}

					// ✅ 2. If approved, insert into VolunteerDuties
					if (newStatus == "Approved")
					{
						string detailQuery = @"
                            SELECT va.EventID, va.UserID AS VolunteerID, vc.CategoryName
                            FROM VolunteerApplications va
                            JOIN VolunteerCategories vc ON va.CategoryID = vc.CategoryID
                            WHERE va.ApplicationID=@AppID";

						int eventId = 0;
						int volunteerId = 0;
						string roleTitle = "";

						using (SqlCommand detailCmd = new SqlCommand(detailQuery, conn))
						{
							detailCmd.Parameters.AddWithValue("@AppID", applicationId);
							using (SqlDataReader dr = detailCmd.ExecuteReader())
							{
								if (dr.Read())
								{
									eventId = Convert.ToInt32(dr["EventID"]);
									volunteerId = Convert.ToInt32(dr["VolunteerID"]);
									roleTitle = dr["CategoryName"].ToString();
								}
								else
								{
									lblMessage.Text = "❌ Could not fetch details for this application.";
									lblMessage.CssClass = "text-danger";
									lblMessage.Visible = true;
									return;
								}
							}
						}

						if (eventId > 0 && volunteerId > 0)
						{
							string insertDuty = @"
                                INSERT INTO VolunteerDuties(EventID, VolunteerID, RoleTitle, Description, IsCompleted, IsApprovedByOrganizer)
                                VALUES(@EventID, @VolunteerID, @RoleTitle, @Description, 0, 1)";

							using (SqlCommand insCmd = new SqlCommand(insertDuty, conn))
							{
								insCmd.Parameters.AddWithValue("@EventID", eventId);
								insCmd.Parameters.AddWithValue("@VolunteerID", volunteerId);
								insCmd.Parameters.AddWithValue("@RoleTitle", roleTitle);
								insCmd.Parameters.AddWithValue("@Description", "Assigned by organizer");
								insCmd.ExecuteNonQuery();
							}
						}
					}

					// ✅ Show success
					lblMessage.Text = $"✅ Volunteer status updated to {newStatus}.";
					lblMessage.CssClass = "text-success";
					lblMessage.Visible = true;

					// Reload the volunteers list
					int eid = Convert.ToInt32(Request.QueryString["eventid"]);
					LoadVolunteersGrouped(eid);
				}
			}
			catch (Exception ex)
			{
				lblMessage.Text = "❌ Error updating status: " + ex.Message;
				lblMessage.CssClass = "text-danger";
				lblMessage.Visible = true;
			}
		}

		/// <summary>
		/// Load volunteers for the event, grouped by categories.
		/// </summary>
		private void LoadVolunteersGrouped(int eventId)
		{
			try
			{
				string connStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
				using (SqlConnection conn = new SqlConnection(connStr))
				{
					conn.Open();

					// Event title
					string eventName = "";
					using (SqlCommand evtCmd = new SqlCommand(
						"SELECT Title FROM Events WHERE EventID=@EID", conn))
					{
						evtCmd.Parameters.AddWithValue("@EID", eventId);
						object result = evtCmd.ExecuteScalar();
						if (result != null) eventName = result.ToString();
					}

					// All volunteers for this event
					string volQuery = @"
                        SELECT va.ApplicationID, va.CategoryID, va.Status, u.FullName, u.Email
                        FROM VolunteerApplications va
                        JOIN Users u ON va.UserID = u.UserID
                        WHERE va.EventID=@EID";
					SqlDataAdapter volAdapter = new SqlDataAdapter(volQuery, conn);
					volAdapter.SelectCommand.Parameters.AddWithValue("@EID", eventId);

					DataTable dtVolunteers = new DataTable();
					volAdapter.Fill(dtVolunteers);

					if (dtVolunteers.Rows.Count == 0)
					{
						lblMessage.Text = "⚠️ No volunteers have applied for this event yet.";
						lblMessage.CssClass = "text-warning";
						lblMessage.Visible = true;
						rptCategories.DataSource = null;
						rptCategories.DataBind();
						lblEventTitle.Text = $"Volunteers for: {eventName}";
						return;
					}

					// Store for filtering
					ViewState["AllVolunteers"] = dtVolunteers;

					// Distinct categories
					var distinctCategoryIds = dtVolunteers.AsEnumerable()
						.Select(r => r.Field<int>("CategoryID"))
						.Distinct()
						.ToList();

					DataTable dtCategories = new DataTable();
					if (distinctCategoryIds.Count > 0)
					{
						string idList = string.Join(",", distinctCategoryIds);
						string catQuery = $"SELECT CategoryID, CategoryName FROM VolunteerCategories WHERE EventID=@EID AND CategoryID IN ({idList})";
						using (SqlCommand catCmd = new SqlCommand(catQuery, conn))
						{
							catCmd.Parameters.AddWithValue("@EID", eventId);
							SqlDataAdapter catAdapter = new SqlDataAdapter(catCmd);
							catAdapter.Fill(dtCategories);
						}
					}

					rptCategories.DataSource = dtCategories;
					rptCategories.DataBind();
					lblEventTitle.Text = $"Volunteers for: {eventName}";
				}
			}
			catch (Exception ex)
			{
				lblMessage.Text = "❌ Error loading volunteers: " + ex.Message;
				lblMessage.CssClass = "text-danger";
				lblMessage.Visible = true;
			}
		}

		/// <summary>
		/// Bind volunteers per category
		/// </summary>
		protected void rptCategories_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				var catRow = (DataRowView)e.Item.DataItem;
				int categoryId = Convert.ToInt32(catRow["CategoryID"]);

				DataTable allVolunteers = ViewState["AllVolunteers"] as DataTable;
				if (allVolunteers == null) return;

				// Filter volunteers for this category
				DataRow[] rows = allVolunteers.Select("CategoryID=" + categoryId);
				DataTable dtFiltered = allVolunteers.Clone();
				foreach (DataRow r in rows) dtFiltered.ImportRow(r);

				Repeater rptVolunteers = (Repeater)e.Item.FindControl("rptVolunteers");
				rptVolunteers.DataSource = dtFiltered;
				rptVolunteers.DataBind();
			}
		}
	}
}
