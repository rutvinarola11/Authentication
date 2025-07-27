<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Authentication.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 style="color: #e14658; text-align: center; font-size: 30px; margin-bottom: 20px;">Login to EventSphere</h2>

    <div class="form-container">
        <table style="width: 100%; max-width: 500px;">
            <tr>
                <td>Email:</td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="inputbox" />
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                        ErrorMessage="Email is required" ForeColor="Red" Display="Dynamic" />
                </td>
            </tr>
            <tr>
                <td>Password:</td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="inputbox" />
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                        ErrorMessage="Password is required" ForeColor="Red" Display="Dynamic" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center; padding-top: 10px;">
                    <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="submit-button" OnClick="btnLogin_Click" />
                </td>
            </tr>
            <!-- Forgot Password Button -->
            <tr>
                <td colspan="2" style="text-align: center; padding-top: 10px;">
                    <asp:Button ID="btnForgotPassword" runat="server" Text="Forgot Password?" CssClass="submit-button" OnClick="btnForgotPassword_Click" CausesValidation="false"/>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </td>
            </tr>
        </table>
    </div>

 
</asp:Content>
