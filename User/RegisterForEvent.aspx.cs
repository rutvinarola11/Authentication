using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace Authentication.User
{
    public partial class RegisterForEvent : System.Web.UI.Page
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

                int eventId;
                if (!int.TryParse(Request.QueryString["eventid"], out eventId))
                {
                    lblMessage.Text = "Invalid event selected.";
                    return;
                }

                LoadEventDetails();
            }
        }

        private void LoadEventDetails()
        {
            string connStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            string query = "SELECT Title, Date, Location FROM Events WHERE EventID = @EventID";

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@EventID", Request.QueryString["eventid"]);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblTitle.Text = reader["Title"].ToString();
                    lblDate.Text = Convert.ToDateTime(reader["Date"]).ToString("dd MMM yyyy");
                    lblLocation.Text = reader["Location"].ToString();
                    pnlEventDetails.Visible = true;
                }
                else
                {
                    lblMessage.Text = "Event not found.";
                }

                reader.Close();
            }
        }
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            int eventId = Convert.ToInt32(Request.QueryString["eventid"]);
            string connStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                // Check if already registered in Volunteers table
                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Volunteers WHERE UserID = @UserID AND EventID = @EventID", con);
                checkCmd.Parameters.AddWithValue("@UserID", userId);
                checkCmd.Parameters.AddWithValue("@EventID", eventId);

                int count = (int)checkCmd.ExecuteScalar();
                if (count > 0)
                {
                    lblMessage.Text = "You have already registered for this event.";
                    return;
                }

                // Insert into Volunteers table
                SqlCommand insertCmd = new SqlCommand("INSERT INTO Volunteers (UserID, EventID) VALUES (@UserID, @EventID)", con);
                insertCmd.Parameters.AddWithValue("@UserID", userId);
                insertCmd.Parameters.AddWithValue("@EventID", eventId);

                insertCmd.ExecuteNonQuery();

                lblMessage.CssClass = "success-message";
                lblMessage.Text = "Registration successful! You are now a volunteer for this event.";
                btnConfirm.Visible = false;
            }
        }
    }
}