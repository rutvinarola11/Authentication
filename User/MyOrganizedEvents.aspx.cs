using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Authentication.User
{
	public partial class MyOrganizedEvent : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (Session["UserID"] == null)
				{
					Response.Redirect("~/Login.aspx");
					return;
				}

				LoadOrganizedEvents();
			}
		}

		private void LoadOrganizedEvents()
		{
			string connStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
			int userId = Convert.ToInt32(Session["UserID"]);

			string query = @"
                SELECT EventID, Title, Date, Location, Description, CreatedAt
                FROM Events
                WHERE OrganizerID = @OrganizerID
                ORDER BY Date DESC";

			using (SqlConnection con = new SqlConnection(connStr))
			using (SqlCommand cmd = new SqlCommand(query, con))
			{
				cmd.Parameters.AddWithValue("@OrganizerID", userId);
				con.Open();

				SqlDataAdapter da = new SqlDataAdapter(cmd);
				DataTable dt = new DataTable();
				da.Fill(dt);

				if (dt.Rows.Count > 0)
				{
					rptOrganizedEvents.DataSource = dt;
					rptOrganizedEvents.DataBind();
					lblNoEvents.Visible = false;
				}
				else
				{
					lblNoEvents.Visible = true;
				}
			}
		}

		// 📌 Called when user clicks "Add Volunteer Categories" in the Repeater
		protected void rptOrganizedEvents_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "AddCategory")
			{
				// get the EventID from CommandArgument
				string eventId = e.CommandArgument.ToString();
				hfEventID.Value = eventId;

				// show the modal
				ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModal", "$('#addCategoryModal').modal('show');", true);
			}
		}

		// 📌 Called when user clicks Save in the modal
		protected void btnSaveCategory_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtCategoryName.Text) || string.IsNullOrWhiteSpace(txtRequiredCount.Text))
			{
				ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModalAgain", "$('#addCategoryModal').modal('show'); alert('Please enter all details.');", true);
				return;
			}

			string eventID = hfEventID.Value;
			string categoryName = txtCategoryName.Text.Trim();
			int requiredCount;

			if (!int.TryParse(txtRequiredCount.Text.Trim(), out requiredCount))
			{
				ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModalAgain", "$('#addCategoryModal').modal('show'); alert('Enter a valid number for Required Volunteers.');", true);
				return;
			}

			string connStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
			using (SqlConnection con = new SqlConnection(connStr))
			{
				string query = @"INSERT INTO VolunteerCategories(EventID, CategoryName, RequiredVolunteers)
                                 VALUES(@EventID, @CategoryName, @RequiredVolunteers)";
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Parameters.AddWithValue("@EventID", eventID);
					cmd.Parameters.AddWithValue("@CategoryName", categoryName);
					cmd.Parameters.AddWithValue("@RequiredVolunteers", requiredCount);

					con.Open();
					cmd.ExecuteNonQuery();
					con.Close();
				}
			}

			// clear inputs
			txtCategoryName.Text = "";
			txtRequiredCount.Text = "";

			// hide modal and notify
			ScriptManager.RegisterStartupScript(this, this.GetType(), "HideModal", "$('#addCategoryModal').modal('hide'); alert('Category added successfully!');", true);
		}
	}
}
