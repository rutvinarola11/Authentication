using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace Authentication.User
{
    public partial class CreateEvent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            if (!IsPostBack)
            {
                if (Session["UserID"] == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                else
                {
                    LoadCategories(); // Load dropdown on first load
                }
            }
        }
        private void LoadCategories()
        {
            string connStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT CategoryID, CategoryName FROM EventCategories", conn);
                conn.Open();
                ddlCategory.DataSource = cmd.ExecuteReader();
                ddlCategory.DataTextField = "CategoryName";
                ddlCategory.DataValueField = "CategoryID";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("--Select Category--", ""));
            }
        }

        protected void btnCreateEvent_Click(object sender, EventArgs e)
        {
            if (!chkTerms.Checked)
            {
                lblStatus.CssClass = "error-message";
                lblStatus.Text = "You must accept the terms and conditions.";
                lblStatus.Visible = true;
                return;
            }

            string title = txtTitle.Text.Trim();
            string description = txtDescription.Text.Trim();
            string location = txtLocation.Text.Trim();
            string skillsRequired = txtSkills.Text.Trim();
            string categoryId = ddlCategory.SelectedValue;
            string mode = ddlMode.SelectedValue;
            string status = ddlStatus.SelectedValue;
            string contactEmail = txtEmail.Text.Trim();
            string contactPhone = txtPhone.Text.Trim();

            bool allowVolunteers = chkAllowVolunteers.Checked;
            bool allowParticipants = chkAllowParticipants.Checked;
            bool termsAccepted = chkTerms.Checked;

            int.TryParse(txtMaxVolunteers.Text.Trim(), out int maxVolunteers);
            int.TryParse(txtMaxParticipants.Text.Trim(), out int maxParticipants);
            decimal.TryParse(txtFee.Text.Trim(), out decimal registrationFee);

            DateTime eventDate;
            if (!DateTime.TryParse(txtDate.Text.Trim(), out eventDate))
            {
                lblStatus.Text = "Invalid event date.";
                lblStatus.Visible = true;
                return;
            }

            TimeSpan startTime;
            TimeSpan endTime;
            DateTime deadline;

            TimeSpan.TryParse(txtStartTime.Text.Trim(), out startTime);
            TimeSpan.TryParse(txtEndTime.Text.Trim(), out endTime);
            DateTime.TryParse(txtDeadline.Text.Trim(), out deadline);

            string bannerPath = "";
            if (fuBanner.HasFile)
            {
                string folderPath = Server.MapPath("~/Assets/EventBanners/");
                if (!System.IO.Directory.Exists(folderPath))
                    System.IO.Directory.CreateDirectory(folderPath);

                string fileName = Guid.NewGuid().ToString() + "_" + System.IO.Path.GetFileName(fuBanner.FileName);
                string savePath = System.IO.Path.Combine(folderPath, fileName);
                fuBanner.SaveAs(savePath);
                bannerPath = "~/Assets/EventBanners/" + fileName;
            }

            int organizerId = Convert.ToInt32(Session["UserID"]);
            string connStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
INSERT INTO Events 
(Title, Description, Location, Date, StartTime, EndTime, OrganizerID, CreatedAt, CategoryID, SkillsRequired, MaxVolunteers, MaxParticipants,
 IsVolunteerOpen, IsParticipantOpen, EventBanner, RegistrationFee, EventMode, RegistrationDeadline, Status, ContactEmail, ContactPhone, TermsAccepted)
VALUES 
(@Title, @Description, @Location, @Date, @StartTime, @EndTime, @OrganizerID, GETDATE(), @CategoryID, @SkillsRequired, @MaxVolunteers, @MaxParticipants,
 @IsVolunteerOpen, @IsParticipantOpen, @EventBanner, @RegistrationFee, @EventMode, @RegistrationDeadline, @Status, @ContactEmail, @ContactPhone, @TermsAccepted)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Location", location);
                cmd.Parameters.AddWithValue("@Date", eventDate);
                cmd.Parameters.AddWithValue("@StartTime", startTime);
                cmd.Parameters.AddWithValue("@EndTime", endTime);
                cmd.Parameters.AddWithValue("@OrganizerID", organizerId);
                cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                cmd.Parameters.AddWithValue("@SkillsRequired", skillsRequired);
                cmd.Parameters.AddWithValue("@MaxVolunteers", maxVolunteers);
                cmd.Parameters.AddWithValue("@MaxParticipants", maxParticipants);
                cmd.Parameters.AddWithValue("@IsVolunteerOpen", allowVolunteers);
                cmd.Parameters.AddWithValue("@IsParticipantOpen", allowParticipants);
                cmd.Parameters.AddWithValue("@EventBanner", bannerPath);
                cmd.Parameters.AddWithValue("@RegistrationFee", registrationFee);
                cmd.Parameters.AddWithValue("@EventMode", mode);
                cmd.Parameters.AddWithValue("@RegistrationDeadline", deadline);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@ContactEmail", contactEmail);
                cmd.Parameters.AddWithValue("@ContactPhone", contactPhone);
                cmd.Parameters.AddWithValue("@TermsAccepted", termsAccepted);

                try
                {
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    lblStatus.CssClass = "success-message";
                    lblStatus.Text = result > 0 ? "Event created successfully!" : "Failed to create event.";
                    lblStatus.Visible = true;
                    ClearForm();
                }
                catch (Exception ex)
                {
                    lblStatus.CssClass = "error-message";
                    lblStatus.Text = "Error: " + ex.Message;
                    lblStatus.Visible = true;
                }
            }
        }
        private void ClearForm()
        {
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtLocation.Text = "";
            txtDate.Text = "";
            txtSkills.Text = "";
            txtMaxVolunteers.Text = "";
            txtMaxParticipants.Text = "";
            txtStartTime.Text = "";
            txtEndTime.Text = "";
            txtDeadline.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtFee.Text = "0.00";
            ddlCategory.SelectedIndex = 0;
            ddlMode.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            chkAllowVolunteers.Checked = false;
            chkAllowParticipants.Checked = true;
            chkTerms.Checked = false;
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedText = ddlCategory.SelectedItem.Text.ToLower();

            // If it's "NGO" or contains "ngo" in name
            if (selectedText.Contains("ngo"))
            {
                txtFee.Text = "0.00";
                txtFee.Enabled = false;
            }
            else
            {
                txtFee.Enabled = true;
            }
        }
    }
    }
