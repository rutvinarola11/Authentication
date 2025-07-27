using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace Authentication.User
{
    public partial class Certificate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null || Request.QueryString["eventid"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCertificatePreview();
            }
        }

        private void LoadCertificatePreview()
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            string fullName = Session["FullName"].ToString();
            int eventId = Convert.ToInt32(Request.QueryString["eventid"]);
            string eventTitle = "";
            DateTime eventDate;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString))
            {
                string query = "SELECT Title, Date FROM Events WHERE EventID = @EventID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@EventID", eventId);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    eventTitle = reader["Title"].ToString();
                    eventDate = Convert.ToDateTime(reader["Date"]);
                }
                else
                {
                    litCertificateContent.Text = "<p class='error-message'>Invalid event selected.</p>";
                    btnDownload.Visible = false;
                    return;
                }
                reader.Close();
            }

            litCertificateContent.Text = $@"
    <div class='certificate-preview'>
        <p>This certifies that <strong>{fullName}</strong><br/>
        has successfully participated in the event<br/>
        <strong>{eventTitle}</strong><br/>
        held on <strong>{eventDate:dd MMMM yyyy}</strong>.</p>
        <br/><br/><em>Issued by EventSphere</em>
    </div>";
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null || Session["FullName"] == null || Request.QueryString["eventid"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            int userId = Convert.ToInt32(Session["UserID"]);
            string fullName = Session["FullName"].ToString();
            int eventId = Convert.ToInt32(Request.QueryString["eventid"]);
            string eventTitle = "";
            DateTime eventDate;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["constr"].ConnectionString))
            {
                con.Open();

                // Check if certificate already exists
                string checkQuery = "SELECT COUNT(*) FROM Certificates WHERE UserID = @UserID AND EventID = @EventID";
                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@UserID", userId);
                checkCmd.Parameters.AddWithValue("@EventID", eventId);

                int existing = (int)checkCmd.ExecuteScalar();
                if (existing > 0)
                {
                    // Already downloaded — show a message and stop further execution
                    litCertificateContent.Text += "<p class='error-message'>You have already downloaded this certificate.</p>";
                    return;
                }

                // Get event info
                string query = "SELECT Title, Date FROM Events WHERE EventID = @EventID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@EventID", eventId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    eventTitle = reader["Title"].ToString();
                    eventDate = Convert.ToDateTime(reader["Date"]);
                }
                else
                {
                    return;
                }
                reader.Close();

                // Insert into Certificates table and update Volunteers
                string insertQuery = @"
            INSERT INTO Certificates (UserID, EventID) VALUES (@UserID, @EventID);
            UPDATE Volunteers SET IsCertificateIssued = 1 WHERE UserID = @UserID AND EventID = @EventID;
        ";
                SqlCommand insertCmd = new SqlCommand(insertQuery, con);
                insertCmd.Parameters.AddWithValue("@UserID", userId);
                insertCmd.Parameters.AddWithValue("@EventID", eventId);
                insertCmd.ExecuteNonQuery();

                // Generate PDF
                using (MemoryStream ms = new MemoryStream())
                {
                    Document doc = new Document(PageSize.A4, 60, 60, 80, 60);
                    PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                    writer.CloseStream = false;
                    doc.Open();

                    Font titleFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 28, BaseColor.BLACK);
                    Font subTitleFont = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 18, BaseColor.DARK_GRAY);
                    Font bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 14, BaseColor.BLACK);
                    Font nameFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.BLUE);

                    PdfContentByte cb = writer.DirectContent;
                    cb.Rectangle(40f, 40f, doc.PageSize.Width - 80f, doc.PageSize.Height - 80f);
                    cb.Stroke();

                    Paragraph title = new Paragraph("Certificate of Participation", titleFont);
                    title.Alignment = Element.ALIGN_CENTER;
                    title.SpacingAfter = 30f;
                    doc.Add(title);

                    Paragraph content = new Paragraph();
                    content.Alignment = Element.ALIGN_CENTER;
                    content.SpacingAfter = 20f;
                    content.Add(new Phrase("This is proudly awarded to\n", bodyFont));
                    content.Add(new Phrase($"{fullName}\n", nameFont));
                    content.Add(new Phrase("for their active participation in the event\n", bodyFont));
                    content.Add(new Phrase($"{eventTitle}\n", subTitleFont));
                    content.Add(new Phrase($"held on {eventDate:dd MMMM yyyy}.\n", bodyFont));
                    doc.Add(content);

                    Paragraph footer = new Paragraph("\n\nIssued by EventSphere", bodyFont);
                    footer.Alignment = Element.ALIGN_CENTER;
                    doc.Add(footer);

                    Paragraph sign = new Paragraph("\n\n________________________\nEvent Organizer", bodyFont);
                    sign.Alignment = Element.ALIGN_CENTER;
                    doc.Add(sign);

                    doc.Close();

                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", $"attachment;filename=Certificate_{eventTitle}.pdf");
                    Response.BinaryWrite(ms.ToArray());
                    Response.End();
                }
            }
        }
    }
}
