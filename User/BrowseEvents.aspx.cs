using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
namespace Authentication.User
{
    public partial class BrowseEvents : System.Web.UI.Page
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

                ddlStatus.SelectedValue = "All";
                PopulateCategoryDropdown();
                LoadEvents();
            }
        }

        private void PopulateCategoryDropdown()
        {
            string connStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT CategoryID, CategoryName FROM EventCategories", con);
                con.Open();
                ddlCategory.DataSource = cmd.ExecuteReader();
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryID";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("All", "All"));
            }
        }

        protected void Filter_Changed(object sender, EventArgs e)
        {
            LoadEvents();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlCategory.SelectedValue = "All";
            ddlStatus.SelectedValue = "All";
            txtLocation.Text = "";
            LoadEvents();
        }

        private void LoadEvents()
        {
            string connStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            int userId = Convert.ToInt32(Session["UserID"]);
            string role = Session["Role"].ToString();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT E.EventID, E.Title, E.Description, E.Date, E.Location FROM Events E WHERE 1=1 ");

                if (role == "Organizer")
                    queryBuilder.Append("AND E.OrganizerID = @UserID ");
                else if (role == "Volunteer")
                    queryBuilder.Append("AND E.IsVolunteerOpen = 1 ");
                else if (role == "Participant")
                    queryBuilder.Append("AND E.IsParticipantOpen = 1 ");

                if (ddlCategory.SelectedValue != "All")
                    queryBuilder.Append("AND E.CategoryID = @CategoryID ");

                if (!string.IsNullOrWhiteSpace(txtLocation.Text))
                    queryBuilder.Append("AND E.Location LIKE @Location ");

                if (ddlStatus.SelectedValue != "All")
                    queryBuilder.Append("AND E.Status = @Status ");

                queryBuilder.Append("ORDER BY E.Date ASC");

                using (SqlCommand cmd = new SqlCommand(queryBuilder.ToString(), con))
                {
                    if (role == "Organizer")
                        cmd.Parameters.AddWithValue("@UserID", userId);

                    if (ddlCategory.SelectedValue != "All")
                        cmd.Parameters.AddWithValue("@CategoryID", ddlCategory.SelectedValue);

                    if (!string.IsNullOrWhiteSpace(txtLocation.Text))
                        cmd.Parameters.AddWithValue("@Location", "%" + txtLocation.Text.Trim() + "%");

                    if (ddlStatus.SelectedValue != "All")
                        cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    rptEvents.DataSource = dt;
                    rptEvents.DataBind();

                    lblNoEvents.Visible = (dt.Rows.Count == 0);
                }
            }
        }


        protected void rptEvents_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int eventId = Convert.ToInt32(e.CommandArgument);
            string role = Session["Role"].ToString();

            if (e.CommandName == "Register")
            {
                if (role == "Participant")
                    Response.Redirect($"RegisterForEvent.aspx?eventid={eventId}");
                else if (role == "Volunteer")
                    Response.Redirect($"ApplyVolunteer.aspx?eventid={eventId}");
            }
            else if (e.CommandName == "ViewDetails")
            {
                Response.Redirect($"~/User/EventDetails.aspx?eventid={eventId}");
            }
        
    }

        public string GetButtonText(object eventId)
        {
            string role = Session["Role"].ToString();
            return role == "Volunteer" ? "Apply to Volunteer" : "Register";
        }
    }
}