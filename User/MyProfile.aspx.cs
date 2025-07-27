using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
namespace Authentication.User
{
    public partial class MyProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            if (Session["UserID"] == null || Session["Role"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadProfile();
                ShowPanelByRole(Session["Role"].ToString());
            }
        }

        private void ShowPanelByRole(string role)
        {
            pnlOrganizer.Visible = role == "Organizer";
            pnlVolunteer.Visible = role == "Volunteer";
            pnlParticipant.Visible = role == "Participant";
        }

        private void LoadProfile()
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            string role = Session["Role"].ToString();
            string connStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                // Common: Load from Users table
                SqlCommand userCmd = new SqlCommand("SELECT FullName, Email FROM Users WHERE UserID = @UserID", con);
                userCmd.Parameters.AddWithValue("@UserID", userId);
                SqlDataReader userReader = userCmd.ExecuteReader();

                if (userReader.Read())
                {
                    txtFullName.Text = userReader["FullName"].ToString();
                    txtEmail.Text = userReader["Email"].ToString();
                }
                userReader.Close();

                // Role-specific
                if (role == "Organizer")
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Organizers WHERE OrganizerID = @UserID", con);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtOrgName.Text = reader["OrgName"]?.ToString();
                        ddlOrgType.SelectedValue = reader["OrgType"]?.ToString();
                        txtDesignation.Text = reader["Designation"]?.ToString();
                        txtOrgWebsite.Text = reader["OrgWebsite"]?.ToString();
                        txtOrgLinkedIn.Text = reader["LinkedIn"]?.ToString();
                        txtOrgSocial.Text = reader["Facebook"]?.ToString();
                        txtOfficeAddress.Text = reader["OfficeAddress"]?.ToString();
                        txtCity.Text = reader["City"]?.ToString();
                        txtState.Text = reader["State"]?.ToString();
                        txtCountry.Text = reader["Country"]?.ToString();
                        txtPinCode.Text = reader["PinCode"]?.ToString();
                        txtOrgPhone.Text = reader["OrgPhone"]?.ToString();
                        // TODO: Load selected OrgEvents if needed
                    }
                    reader.Close();
                }
                else if (role == "Volunteer")
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM VolunteerProfiles WHERE VolunteerID = @UserID", con);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtMobile.Text = reader["Mobile"]?.ToString();
                        txtDob.Text = Convert.ToDateTime(reader["DOB"]).ToString("yyyy-MM-dd");
                        ddlGender.SelectedValue = reader["Gender"]?.ToString();
                        txtSkills.Text = reader["Skills"]?.ToString();
                        txtLanguages.Text = reader["Languages"]?.ToString();
                        txtQualification.Text = reader["Qualification"]?.ToString();
                        txtCurrentAddress.Text = reader["CurrentAddress"]?.ToString();
                        txtCity.Text = reader["City"]?.ToString();
                        txtState.Text = reader["State"]?.ToString();
                        txtCountry.Text = reader["Country"]?.ToString();
                        txtPinCode.Text = reader["PinCode"]?.ToString();
                        ddlMode.SelectedValue = reader["PreferredMode"]?.ToString();
                    }
                    reader.Close();
                }
                else if (role == "Participant")
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Participants WHERE ParticipantID = @UserID", con);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtMobile.Text = reader["Mobile"]?.ToString();
                        txtDob.Text = Convert.ToDateTime(reader["DOB"]).ToString("yyyy-MM-dd");
                        ddlGender.SelectedValue = reader["Gender"]?.ToString();
                        txtLanguages.Text = reader["Languages"]?.ToString();
                        txtQualification.Text = reader["Qualification"]?.ToString();
                        txtCurrentAddress.Text = reader["CurrentAddress"]?.ToString();
                        txtCity.Text = reader["City"]?.ToString();
                        txtState.Text = reader["State"]?.ToString();
                        txtCountry.Text = reader["Country"]?.ToString();
                        txtPinCode.Text = reader["PinCode"]?.ToString();
                        ddlMode.SelectedValue = reader["PreferredMode"]?.ToString();
                        txtInstitute.Text = reader["InstituteName"]?.ToString();
                        txtCourse.Text = reader["CourseDept"]?.ToString();
                        txtYear.Text = reader["YearOfStudy"]?.ToString();
                    }
                    reader.Close();
                }
            }
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            string role = Session["Role"].ToString();
            string connStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                // Update FullName in Users
                SqlCommand updateUser = new SqlCommand("UPDATE Users SET FullName = @FullName WHERE UserID = @UserID", con);
                updateUser.Parameters.AddWithValue("@UserID", userId);
                updateUser.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                updateUser.ExecuteNonQuery();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                if (role == "Organizer")
                {
                    cmd.CommandText = @"UPDATE Organizers SET OrgName=@OrgName, OrgType=@OrgType, Designation=@Designation,
                        OfficeAddress=@OfficeAddress, City=@City, State=@State, Country=@Country, PinCode=@PinCode,
                        OrgPhone=@OrgPhone, OrgEvents=@OrgEvents, OrgWebsite=@OrgWebsite, LinkedIn=@LinkedIn,
                        Facebook=@Facebook WHERE OrganizerID=@UserID";

                    cmd.Parameters.AddWithValue("@OrgName", txtOrgName.Text.Trim());
                    cmd.Parameters.AddWithValue("@OrgType", ddlOrgType.SelectedValue);
                    cmd.Parameters.AddWithValue("@Designation", txtDesignation.Text.Trim());
                    cmd.Parameters.AddWithValue("@OfficeAddress", txtOfficeAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                    cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                    cmd.Parameters.AddWithValue("@Country", txtCountry.Text.Trim());
                    cmd.Parameters.AddWithValue("@PinCode", txtPinCode.Text.Trim());
                    cmd.Parameters.AddWithValue("@OrgPhone", txtOrgPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@OrgEvents", string.Join(",", cblOrgEvents.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Text)));
                    cmd.Parameters.AddWithValue("@OrgWebsite", txtOrgWebsite.Text.Trim());
                    cmd.Parameters.AddWithValue("@LinkedIn", txtOrgLinkedIn.Text.Trim());
                    cmd.Parameters.AddWithValue("@Facebook", txtOrgSocial.Text.Trim());
                }
                else if (role == "Volunteer")
                {
                    cmd.CommandText = @"UPDATE VolunteerProfiles SET Mobile=@Mobile, DOB=@DOB, Gender=@Gender, Skills=@Skills,
                        Languages=@Languages, Qualification=@Qualification, CurrentAddress=@CurrentAddress, City=@City,
                        State=@State, Country=@Country, PinCode=@PinCode, PreferredMode=@PreferredMode
                        WHERE VolunteerID=@UserID";

                    cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                    cmd.Parameters.AddWithValue("@DOB", txtDob.Text.Trim());
                    cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
                    cmd.Parameters.AddWithValue("@Skills", txtSkills.Text.Trim());
                    cmd.Parameters.AddWithValue("@Languages", txtLanguages.Text.Trim());
                    cmd.Parameters.AddWithValue("@Qualification", txtQualification.Text.Trim());
                    cmd.Parameters.AddWithValue("@CurrentAddress", txtCurrentAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                    cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                    cmd.Parameters.AddWithValue("@Country", txtCountry.Text.Trim());
                    cmd.Parameters.AddWithValue("@PinCode", txtPinCode.Text.Trim());
                    cmd.Parameters.AddWithValue("@PreferredMode", ddlMode.SelectedValue);
                }
                else if (role == "Participant")
                {
                    cmd.CommandText = @"UPDATE Participants SET Mobile=@Mobile, DOB=@DOB, Gender=@Gender, Languages=@Languages,
                        Qualification=@Qualification, CurrentAddress=@CurrentAddress, City=@City, State=@State, Country=@Country,
                        PinCode=@PinCode, PreferredMode=@PreferredMode, InstituteName=@InstituteName,
                        CourseDept=@CourseDept, YearOfStudy=@YearOfStudy WHERE ParticipantID=@UserID";

                    cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                    cmd.Parameters.AddWithValue("@DOB", txtDob.Text.Trim());
                    cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
                    cmd.Parameters.AddWithValue("@Languages", txtLanguages.Text.Trim());
                    cmd.Parameters.AddWithValue("@Qualification", txtQualification.Text.Trim());
                    cmd.Parameters.AddWithValue("@CurrentAddress", txtCurrentAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
                    cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                    cmd.Parameters.AddWithValue("@Country", txtCountry.Text.Trim());
                    cmd.Parameters.AddWithValue("@PinCode", txtPinCode.Text.Trim());
                    cmd.Parameters.AddWithValue("@PreferredMode", ddlMode.SelectedValue);
                    cmd.Parameters.AddWithValue("@InstituteName", txtInstitute.Text.Trim());
                    cmd.Parameters.AddWithValue("@CourseDept", txtCourse.Text.Trim());
                    cmd.Parameters.AddWithValue("@YearOfStudy", txtYear.Text.Trim());
                }

                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.ExecuteNonQuery();
            }

            lblMessage.Text = "Profile updated successfully!";
            lblMessage.Visible = true;
        }
    }
}