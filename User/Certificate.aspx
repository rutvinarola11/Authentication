<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Certificate.aspx.cs" Inherits="Authentication.User.Certificate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div class="container form-container">
        <h2 class="form-title">Certificate of Participation</h2>
        
        <asp:Panel ID="pnlCertificate" runat="server" CssClass="certificate-box" style="background-color: #fff; padding: 20px; text-align: center;">

            <asp:Literal ID="litCertificateContent" runat="server"></asp:Literal>
        </asp:Panel>

        <asp:Button ID="btnDownload" runat="server" Text="Download Certificate as PDF" CssClass="submit-button" OnClick="btnDownload_Click" />
        <asp:Button ID="btnBack" runat="server" Text="Back to My Events" CssClass="submit-button" PostBackUrl="~/User/MyVolunteerEvents.aspx" />
    </div>
</asp:Content>
