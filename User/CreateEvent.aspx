<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="CreateEvent.aspx.cs" Inherits="Authentication.User.CreateEvent" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h2 style="color: #e14658; text-align: center; font-size: 30px; margin-bottom: 20px;">Create New Event</h2>

<div class="form-container">
    <asp:ValidationSummary ID="vsSummary" runat="server" CssClass="error-message" HeaderText="Please fix the following:" />
    <table class="register-table">
        <tr>
            <td>Title:</td>
            <td>
                <asp:TextBox ID="txtTitle" runat="server" CssClass="inputbox" />
                <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle"
                    ErrorMessage="Title is required." CssClass="error-message" Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td>Description:</td>
            <td>
                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="4" CssClass="inputbox" />
                <asp:RequiredFieldValidator ID="rfvDesc" runat="server" ControlToValidate="txtDescription"
                    ErrorMessage="Description is required." CssClass="error-message" Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td>Date:</td>
            <td>
                <asp:TextBox ID="txtDate" runat="server" TextMode="Date" CssClass="inputbox" />
                <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDate"
                    ErrorMessage="Date is required." CssClass="error-message" Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td>Location:</td>
            <td>
                <asp:TextBox ID="txtLocation" runat="server" CssClass="inputbox" />
                <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="txtLocation"
                    ErrorMessage="Location is required." CssClass="error-message" Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td>Category:</td>
            <td>
                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="inputbox" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" />
                <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="ddlCategory"
                    InitialValue="" ErrorMessage="Please select a category." CssClass="error-message" Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td>Skills Required:</td>
            <td>
                <asp:TextBox ID="txtSkills" runat="server" CssClass="inputbox" />
            </td>
        </tr>
        <tr>
            <td>Max Volunteers:</td>
            <td>
                <asp:TextBox ID="txtMaxVolunteers" runat="server" CssClass="inputbox" />
                <asp:RequiredFieldValidator ID="rfvMaxVol" runat="server" ControlToValidate="txtMaxVolunteers"
                    ErrorMessage="Required" CssClass="error-message" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revMaxVol" runat="server" ControlToValidate="txtMaxVolunteers"
                    ValidationExpression="^\d+$" ErrorMessage="Enter valid number" CssClass="error-message" Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td>Max Participants:</td>
            <td>
                <asp:TextBox ID="txtMaxParticipants" runat="server" CssClass="inputbox" />
                <asp:RequiredFieldValidator ID="rfvMaxPart" runat="server" ControlToValidate="txtMaxParticipants"
                    ErrorMessage="Required" CssClass="error-message" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revMaxPart" runat="server" ControlToValidate="txtMaxParticipants"
                    ValidationExpression="^\d+$" ErrorMessage="Enter valid number" CssClass="error-message" Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td>Allow Volunteers:</td>
            <td><asp:CheckBox ID="chkAllowVolunteers" runat="server" /></td>
        </tr>
        <tr>
            <td>Allow Participants:</td>
            <td><asp:CheckBox ID="chkAllowParticipants" runat="server" Checked="true" /></td>
        </tr>
        <tr>
            <td>Start Time:</td>
            <td><asp:TextBox ID="txtStartTime" runat="server" CssClass="inputbox" TextMode="Time" /></td>
        </tr>
        <tr>
            <td>End Time:</td>
            <td><asp:TextBox ID="txtEndTime" runat="server" CssClass="inputbox" TextMode="Time" /></td>
        </tr>
        <tr>
            <td>Registration Deadline:</td>
            <td>
                <asp:TextBox ID="txtDeadline" runat="server" CssClass="inputbox" TextMode="Date" />
                <asp:RequiredFieldValidator ID="rfvDeadline" runat="server" ControlToValidate="txtDeadline"
                    ErrorMessage="Deadline is required." CssClass="error-message" Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td>Registration Fee (₹):</td>
            <td>
                <asp:TextBox ID="txtFee" runat="server" CssClass="inputbox" Text="0.00" />
                <asp:RequiredFieldValidator ID="rfvFee" runat="server" ControlToValidate="txtFee"
                    ErrorMessage="Fee is required." CssClass="error-message" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revFee" runat="server" ControlToValidate="txtFee"
                    ValidationExpression="^\d+(\.\d{1,2})?$"
                    ErrorMessage="Enter a valid amount (e.g., 0.00)" CssClass="error-message" Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td>Event Mode:</td>
            <td>
                <asp:DropDownList ID="ddlMode" runat="server" CssClass="inputbox">
                    <asp:ListItem Text="--Select--" Value="" />
                    <asp:ListItem Text="Online" Value="Online" />
                    <asp:ListItem Text="Offline" Value="Offline" />
                    <asp:ListItem Text="Hybrid" Value="Hybrid" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvMode" runat="server" ControlToValidate="ddlMode"
                    InitialValue="" ErrorMessage="Select event mode." CssClass="error-message" Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td>Event Status:</td>
            <td>
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="inputbox">
                    <asp:ListItem Text="--Select--" Value="" />
                    <asp:ListItem Text="Upcoming" Value="Upcoming" />
                    <asp:ListItem Text="Ongoing" Value="Ongoing" />
                    <asp:ListItem Text="Completed" Value="Completed" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlStatus"
                    InitialValue="" ErrorMessage="Select event status." CssClass="error-message" Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td>Contact Email:</td>
            <td>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="inputbox" TextMode="Email" />
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="Email required." CssClass="error-message" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                    ValidationExpression="^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$"
                    ErrorMessage="Invalid email." CssClass="error-message" Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td>Contact Phone:</td>
            <td>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="inputbox" />
                <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone"
                    ErrorMessage="Phone required." CssClass="error-message" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtPhone"
                    ValidationExpression="^\d{10}$" ErrorMessage="Enter 10-digit number." CssClass="error-message" Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td>Event Banner:</td>
            <td><asp:FileUpload ID="fuBanner" runat="server" CssClass="inputbox" /></td>
        </tr>
        <tr>
            <td>Accept Terms:</td>
            <td><asp:CheckBox ID="chkTerms" runat="server" Text=" I agree to the terms and conditions." /></td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:Button ID="btnCreateEvent" runat="server" Text="Create Event" CssClass="submit-button" OnClick="btnCreateEvent_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:Label ID="lblStatus" runat="server" CssClass="error-message" Visible="false" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:Button ID="btnBack" runat="server" Text="Back to Dashboard"  CausesValidation="False" PostBackUrl="~/User/UserDashboard.aspx" CssClass="submit-button" />
            </td>
        </tr>
    </table>
</div>
</asp:Content>
