<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="RegisterForEvent.aspx.cs" Inherits="Authentication.User.RegisterForEvent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="form-container">
        <h2>Event Registration</h2>
        
        <asp:Label ID="lblMessage" runat="server" CssClass="error-message" EnableViewState="false" />
        
        <asp:Panel ID="pnlEventDetails" runat="server" Visible="false">
            <table>
                <tr>
                    <td><strong>Event Title:</strong></td>
                    <td><asp:Label ID="lblTitle" runat="server" /></td>
                </tr>
                <tr>
                    <td><strong>Date:</strong></td>
                    <td><asp:Label ID="lblDate" runat="server" /></td>
                </tr>
                <tr>
                    <td><strong>Location:</strong></td>
                    <td><asp:Label ID="lblLocation" runat="server" /></td>
                </tr>
            </table>

            <asp:Button ID="btnConfirm" runat="server" Text="Confirm Registration" CssClass="submit-button" OnClick="btnConfirm_Click" />
        </asp:Panel>
         <asp:Button ID="btnBackToDashboard3" runat="server" Text="Back to Dashboard" CssClass="submit-button" PostBackUrl="~/User/UserDashboard.aspx" />

    </div>
</asp:Content>
