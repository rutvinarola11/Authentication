<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="Authentication.ForgotPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 style="color: #e14658; text-align: center; font-size: 30px; margin-bottom: 20px;">Forgot Password</h2>

    <div class="form-container">
        <table style="width: 100%; max-width: 500px;">
            <tr>
                <td>Email:</td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="inputbox" />
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required" ForeColor="Red" Display="Dynamic" />
                </td>
            </tr>
            <tr>
                <td>Security Question:</td>
                <td>
                    <asp:DropDownList ID="ddlSecurityQuestion" runat="server" CssClass="inputbox" AppendDataBoundItems="true">
                        <asp:ListItem Text="--Select Question--" Value="" />
                        <asp:ListItem Text="What is your favorite color?" Value="What is your favorite color?" />
                        <asp:ListItem Text="What is your pet’s name?" Value="What is your pet’s name?" />
                        <asp:ListItem Text="What is your mother’s name?" Value="What is your mother’s name?" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvSecurityQuestion" runat="server" ControlToValidate="ddlSecurityQuestion" InitialValue="" ErrorMessage="* Required" ForeColor="Red" Display="Dynamic" />
                </td>
            </tr>
            <tr>
                <td>Answer:</td>
                <td>
                    <asp:TextBox ID="txtAnswer" runat="server" CssClass="inputbox" />
                    <asp:RequiredFieldValidator ID="rfvAnswer" runat="server" ControlToValidate="txtAnswer" ErrorMessage="* Required" ForeColor="Red" Display="Dynamic" />
                </td>
            </tr>
            <tr>
                <td>New Password:</td>
                <td>
                    <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="inputbox" />
                    <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ControlToValidate="txtNewPassword" ErrorMessage="* Required" ForeColor="Red" Display="Dynamic" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center; padding-top: 10px;">
                    <asp:Button ID="btnResetPassword" runat="server" Text="Reset Password" CssClass="submit-button" OnClick="btnResetPassword_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" />
                </td>
            </tr>
        </table>
    </div>

 
</asp:Content>
