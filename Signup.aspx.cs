using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
namespace Authentication
{
    public partial class Signup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }


        protected void GenerateOtp_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (txtPassword.Text != txtConfirm.Text)
                {
                    lblMessage.Text = "Passwords do not match.";
                    return;
                }

                string fullName = txtName.Text.Trim();
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text;
                string role = ddlRole.SelectedValue;
                string question = ddlSecurityQuestion.SelectedItem.Text;
                string answer = txtSecurityAnswer.Text.Trim();

                string passwordHash = HashPassword(password);
                string answerHash = HashPassword(answer);

                // Store all in Session
                Session["PendingUser"] = new string[] { fullName, email, passwordHash, role, question, answerHash };

                // Generate OTP
                Random rnd = new Random();
                int otp = rnd.Next(100000, 999999);
                Session["OTP"] = otp;

                // Redirect to OTP verification page
                Response.Redirect("SignupConfirm.aspx");
            }
        }

    
        
    }
}
    
