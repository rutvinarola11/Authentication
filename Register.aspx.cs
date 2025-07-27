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
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;



        }


        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
        protected void RegisterUser(object sender, EventArgs e)
        {
            {
                // Collect user input
                string fullName = txtName.Text.Trim();
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text;
                string passwordHash = HashPassword(password);

                // Get role from dropdown
                string role = ddlRole.SelectedValue;
                if (string.IsNullOrEmpty(role))
                {
                    // Optional: display error message or alert
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please select a role.');", true);
                    return;
                }

                // Get security question and answer
                string securityQuestion = ddlSecurityQuestion.SelectedItem.Text;
                string securityAnswer = txtSecurityAnswer.Text.Trim();
                string hashedSecurityAnswer = HashPassword(securityAnswer);

                // Connection string from Web.config
                string connectionString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = "INSERT INTO Users (FullName, Email, PasswordHash, Role, SecurityQuestion, SecurityAnswer) " +
                                   "VALUES (@FullName, @Email, @PasswordHash, @Role, @SecurityQuestion, @SecurityAnswer)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                        cmd.Parameters.AddWithValue("@Role", role);
                        cmd.Parameters.AddWithValue("@SecurityQuestion", securityQuestion);
                        cmd.Parameters.AddWithValue("@SecurityAnswer", hashedSecurityAnswer);

                        cmd.ExecuteNonQuery();
                    }
                }

                // Redirect after successful registration
                Response.Redirect("Login.aspx");
            }
        }
    }
}