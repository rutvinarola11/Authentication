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
    public partial class MyVolunteerEvents : System.Web.UI.Page
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

                // ✅ Check and fetch FullName if missing
                if (Session["FullName"] == null)
                {
                    string connStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(connStr))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("SELECT FullName FROM Users WHERE UserID = @UserID", con);
                        cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(Session["UserID"]));
                        object fullName = cmd.ExecuteScalar();
                        if (fullName != null)
                        {
                            Session["FullName"] = fullName.ToString();
                        }
                    }
                }

                LoadMyEvents(); // ✅ Continue to load events
            }
        }
        private void LoadMyEvents()
        {
            string connStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            int userId = Convert.ToInt32(Session["UserID"]);

            string query = @"
                SELECT e.EventID, e.Title, e.Date, e.Location, v.JoinedAt AS RegistrationDate
                FROM Volunteers v
                JOIN Events e ON v.EventID = e.EventID
                WHERE v.UserID = @UserID
                ORDER BY e.Date DESC";

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    rptMyEvents.DataSource = dt;
                    rptMyEvents.DataBind();
                    lblNoEvents.Visible = false;
                }
                else
                {
                    lblNoEvents.Visible = true;
                }
            }
        }
        protected void rptMyEvents_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            int eventId = Convert.ToInt32(e.CommandArgument);
            string connStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            if (e.CommandName == "DownloadCertificate")
            {
                Response.Redirect($"~/User/Certificate.aspx?eventid={eventId}");
            }
			else if (e.CommandName == "DownloadID")
			{
				int applicationId = Convert.ToInt32(e.CommandArgument);
				Response.Redirect("~/User/DownloadID.aspx?appid=" + applicationId);
			}
			else if (e.CommandName == "CancelRegistration")
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    string deleteQuery = "DELETE FROM Volunteers WHERE UserID = @UserID AND EventID = @EventID";
                    SqlCommand cmd = new SqlCommand(deleteQuery, con);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@EventID", eventId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                lblMessage.Text = "Your registration has been cancelled.";
                lblMessage.CssClass = "success-message";
                lblMessage.Visible = true;

                LoadMyEvents(); // Refresh the list
            }
        }
    }
}