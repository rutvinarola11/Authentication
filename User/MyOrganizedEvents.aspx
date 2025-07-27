<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="MyOrganizedEvents.aspx.cs" Inherits="Authentication.User.MyOrganizedEvent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container">
        <div class="banner">
            <h1>My Organized Events</h1>
            <p>Here are the events you've created.</p>
        </div>

        <asp:Repeater ID="rptOrganizedEvents" runat="server" OnItemCommand="rptOrganizedEvents_ItemCommand">
            <HeaderTemplate>
                <div class="grid grid-2" style="margin-top: 30px;">
            </HeaderTemplate>

            <ItemTemplate>
                <div class="card">
                    <h3><%# Eval("Title") %></h3>
                    <p><strong>Date:</strong> <%# Eval("Date", "{0:dd MMM yyyy}") %></p>
                    <p><strong>Location:</strong> <%# Eval("Location") %></p>
                    <p><strong>Description:</strong> <%# Eval("Description") %></p>
                    <p><strong>Created On:</strong> <%# Eval("CreatedAt", "{0:dd MMM yyyy}") %></p>

                    <a href='ViewVolunteers.aspx?eventid=<%# Eval("EventID") %>' class="submit-button">View Volunteers</a>
                    
                    <!-- Add Category Button with CommandArgument -->
                    <asp:LinkButton ID="lnkAddCategory" runat="server"
    CommandName="AddCategory"
    CommandArgument='<%# Eval("EventID") %>'
    CssClass="btn btn-warning"
    Text="Add Volunteer Categories">
</asp:LinkButton>
                </div>
            </ItemTemplate>

            <FooterTemplate>
                </div>
            </FooterTemplate>
        </asp:Repeater>

        <asp:Label ID="lblNoEvents" runat="server" Text="You haven’t organized any events yet." Visible="false" CssClass="error-message" />
        <asp:Button ID="btnBackToDashboard" runat="server" Text="Back to Dashboard" CssClass="submit-button" PostBackUrl="~/User/UserDashboard.aspx" />
    </div>

    <!-- ✅ MODAL FOR ADDING CATEGORY -->
    <div class="modal fade" id="addCategoryModal" tabindex="-1" role="dialog" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered" role="document">
        <div class="modal-content bg-dark text-light">
          <div class="modal-header">
            <h5 class="modal-title">Add Volunteer Category</h5>
            <button type="button" class="close text-light" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">
            <asp:HiddenField ID="hfEventID" runat="server" />
            <div class="form-group">
              <label>Category Name</label>
              <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
              <label>Required Volunteers</label>
              <asp:TextBox ID="txtRequiredCount" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
            </div>
          </div>
          <div class="modal-footer">
            <asp:Button ID="btnSaveCategory" runat="server" CssClass="btn btn-success" Text="Save Category" OnClick="btnSaveCategory_Click" />
            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
          </div>
        </div>
      </div>
    </div>

</asp:Content>
