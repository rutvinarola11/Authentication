using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
namespace Authentication
{
    public partial class SignupConfirm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["PendingUser"] == null || Session["OTP"] == null)
                {
                    // Optionally show a message here via QueryString before redirect
                    Response.Redirect("Signup.aspx?error=sessionexpired");
                }
            }
        
    }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            string enteredOtp = txtEnteredOtp.Text.Trim();
            string actualOtp = Session["OTP"]?.ToString();

            if (string.IsNullOrEmpty(actualOtp) || enteredOtp != actualOtp)
            {
                lblOtpMessage.Text = "Invalid OTP. Please try again.";
                return;
            }

            string[] data = (string[])Session["PendingUser"];
            string fullName = data[0];
            string email = data[1];
            string passwordHash = data[2];
            string role = data[3];
            string question = data[4];
            string answerHash = data[5];

            string cs = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Users (FullName, Email, PasswordHash, Role, SecurityQuestion, SecurityAnswer) " +
                    "VALUES (@FullName, @Email, @PasswordHash, @Role, @SecurityQuestion, @SecurityAnswer)", con);

                cmd.Parameters.AddWithValue("@FullName", fullName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                cmd.Parameters.AddWithValue("@Role", role);
                cmd.Parameters.AddWithValue("@SecurityQuestion", question);
                cmd.Parameters.AddWithValue("@SecurityAnswer", answerHash);
                cmd.ExecuteNonQuery();
            }

            Session.Remove("PendingUser");
            Session.Remove("OTP");

            Response.Redirect("Login.aspx?signup=success");
        }
    }
}