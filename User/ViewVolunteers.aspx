<%@ Page Title="Volunteers" Language="C#" MasterPageFile="~/MasterPage.Master"
    AutoEventWireup="true" CodeBehind="ViewVolunteers.aspx.cs"
    Inherits="Authentication.User.ViewVolunteers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <!-- Page Banner/Header -->
        <div class="banner">
            <h1>Volunteers for Event</h1>
            <p>Below is the list of applicants who have applied to volunteer.</p>
        </div>

        <!-- Event title -->
        <asp:Label ID="lblEventTitle" runat="server" CssClass="highlight"></asp:Label>

        <!-- Outer Repeater: Categories -->
        <asp:Repeater ID="rptCategories" runat="server" OnItemDataBound="rptCategories_ItemDataBound">
            <ItemTemplate>
                <!-- Category heading -->
                <h3 class="sub-heading"><%# Eval("CategoryName") %></h3>

                <!-- Inner Repeater: Volunteers -->
                <asp:Repeater ID="rptVolunteers" runat="server" OnItemCommand="rptVolunteers_ItemCommand">
                    <HeaderTemplate>
                        <table class="volunteer-table">
                            <tr>
                                <th>Name</th>
                                <th>Email</th>
                                <th>Status</th>
                                <th>Actions</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("FullName") %></td>
                            <td><%# Eval("Email") %></td>
                            <td><%# Eval("Status") %></td>
                            <td>
                                <asp:Panel runat="server" Visible='<%# Eval("Status").ToString() == "Pending" %>'>
                                    <asp:Button ID="btnApprove" runat="server" Text="Approve"
                                        CssClass="submit-button"
                                        CommandName="Approve" CommandArgument='<%# Eval("ApplicationID") %>' />
                                    <asp:Button ID="btnReject" runat="server" Text="Reject"
                                        CssClass="submit-button"
                                        CommandName="Reject" CommandArgument='<%# Eval("ApplicationID") %>' />
                                </asp:Panel>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </ItemTemplate>
        </asp:Repeater>

        <!-- Messages -->
        <asp:Label ID="lblNoVolunteers" runat="server"
            Text="No volunteer applications yet."
            CssClass="error-message" Visible="false"></asp:Label>
        <asp:Label ID="lblMessage" runat="server"
            CssClass="success-message" Visible="false"></asp:Label>

        <!-- Back Button -->
        <div class="form-actions">
            <asp:Button ID="btnBackToMyEvents" runat="server"
                Text="Back to My Events"
                CssClass="submit-button"
                PostBackUrl="~/User/MyOrganizedEvents.aspx" />
        </div>
    </div>
</asp:Content>
