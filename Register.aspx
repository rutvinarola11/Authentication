<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Authentication.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <h2 style="color: #e14658; text-align: center; font-size: 30px; margin-bottom: 20px;">Register Here</h2>

    <div class="form-container">
        <table class="register-table">
            <tr>
                <td>Full Name:</td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" CssClass="inputbox" />
                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                        ErrorMessage="Name is required" ForeColor="Red" Display="Dynamic" CssClass="error-message" />
                </td>
            </tr>

            <tr>
                <td>Email:</td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="inputbox" />
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                        ErrorMessage="Email is required" ForeColor="Red" Display="Dynamic" CssClass="error-message" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                        ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                        ErrorMessage="Invalid email format" ForeColor="Red" Display="Dynamic" CssClass="error-message" />
                </td>
            </tr>

            <tr>
                <td>Password:</td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="inputbox" />
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                        ErrorMessage="Password is required" ForeColor="Red" Display="Dynamic" CssClass="error-message" />
                    <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtPassword"
                        ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"
                        ErrorMessage="Minimum 8 characters & a number" ForeColor="Red" Display="Dynamic" CssClass="error-message" />
                </td>
            </tr>

            <tr>
                <td>Confirm Password:</td>
                <td>
                    <asp:TextBox ID="txtConfirm" runat="server" TextMode="Password" CssClass="inputbox" />
                    <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirm"
                        ControlToCompare="txtPassword" ErrorMessage="Passwords do not match"
                        ForeColor="Red" Display="Dynamic" CssClass="error-message" />
                </td>
            </tr>

            <tr>
                <td>User Role:</td>
                <td>
                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="inputbox">
                       <asp:ListItem Text="--Select Role--" Value="" />
                     
                        <asp:ListItem Text="Participant" Value="Participant" />
                          <asp:ListItem Text="Organizer" Value="Organizer" />
                             <asp:ListItem Text="Volunteer" Value="Volunteer" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvRole" runat="server" ControlToValidate="ddlRole"
                        InitialValue="" ErrorMessage="Please select a role" ForeColor="Red" CssClass="error-message" Display="Dynamic" />
                </td>
            </tr>

            <tr>
                <td>Security Question:</td>
                <td>
                    <asp:DropDownList ID="ddlSecurityQuestion" runat="server" CssClass="inputbox">
                        <asp:ListItem Text="--Select Question--" Value="" />
                        <asp:ListItem Text="What is your favorite color?" Value="What is your favorite color?" />
                        <asp:ListItem Text="What is your pet’s name?" Value="What is your pet’s name?" />
                        <asp:ListItem Text="What is your mother’s name?" Value="What is your mother’s name?" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvQuestion" runat="server" ControlToValidate="ddlSecurityQuestion"
                        InitialValue="" ErrorMessage="Select a question" ForeColor="Red" CssClass="error-message" Display="Dynamic" />
                </td>

            </tr>

            <tr>
                <td>Security Answer:</td>
                <td>
                    <asp:TextBox ID="txtSecurityAnswer" runat="server" CssClass="inputbox" />
                    <asp:RequiredFieldValidator ID="rfvAnswer" runat="server" ControlToValidate="txtSecurityAnswer"
                        ErrorMessage="Answer is required" ForeColor="Red" Display="Dynamic" CssClass="error-message" />
                </td>
            </tr>

            <tr>
                <td colspan="2" style="text-align: center; padding-top: 15px;">
                    <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="submit-button" OnClick="RegisterUser" />
                </td>
            </tr>
        </table>
    </div>

   
</asp:Content>
