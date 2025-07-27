<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="MyVolunteerEvents.aspx.cs" Inherits="Authentication.User.MyVolunteerEvents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div class="container">
        <div class="banner">
            <h1>My Registered Events</h1>
            <p>Here are the events you’ve signed up to volunteer for.</p>
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="success-message" Visible="false" />

        <asp:Repeater ID="rptMyEvents" runat="server" OnItemCommand="rptMyEvents_ItemCommand">
            <HeaderTemplate>
                <div class="grid grid-2" style="margin-top: 30px;">
            </HeaderTemplate>

            <ItemTemplate>
    <div class="card">
        <h3><%# Eval("Title") %></h3>
        <p><strong>Date:</strong> <%# Eval("Date", "{0:dd MMM yyyy}") %></p>
        <p><strong>Location:</strong> <%# Eval("Location") %></p>
        <p><strong>Registered On:</strong> <%# Eval("RegistrationDate", "{0:dd MMM yyyy}") %></p>

        <!-- ✅ Download ID button only if status = Approved -->
        <asp:Button runat="server"
            Text="Download ID Pass"
            CommandName="DownloadID"
            CommandArgument='<%# Eval("ApplicationID") %>'
            CssClass="submit-button"
            Visible='<%# Eval("Status").ToString() == "Approved" %>' />

        <!-- ✅ Certificate button (your existing one) -->
        <asp:Button runat="server" 
            Text="Download Certificate" 
            CommandName="DownloadCertificate" 
            CommandArgument='<%# Eval("EventID") %>' 
            CssClass="submit-button" 
            Visible='<%# Convert.ToDateTime(Eval("Date")) <= DateTime.Today %>' />

        <!-- ✅ Cancel Registration button (your existing one) -->
        <asp:Button runat="server" 
            Text="Cancel Registration"
            CommandName="CancelRegistration"
            CommandArgument='<%# Eval("EventID") %>' 
            CssClass="submit-button" />
    </div>
</ItemTemplate>

            <FooterTemplate>
                </div>
            </FooterTemplate>
        </asp:Repeater>

        <asp:Label ID="lblNoEvents" runat="server" Text="You haven’t registered for any events yet." Visible="false" CssClass="error-message" />
        <asp:Button ID="btnBackToDashboard1" runat="server" Text="Back to Dashboard" CssClass="submit-button" PostBackUrl="~/User/UserDashboard.aspx" />
   </div>

</asp:Content>
