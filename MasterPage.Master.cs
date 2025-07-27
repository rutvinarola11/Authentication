using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Authentication
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserID"] != null)
                {
                    phBeforeLogin.Visible = false;
                    phAfterLogin.Visible = true;
                    explorelink.Visible = true;

                    // Role-based navbar logic
                    string role = Session["Role"]?.ToString();

                    // Add role-specific navbar logic if needed
                    // Example: if Organizer, maybe show Create Event etc.
                }
                else
                {
                    phBeforeLogin.Visible = true;
                    phAfterLogin.Visible = false;
                    explorelink.Visible = false;
                }
            }
        }
    }
}