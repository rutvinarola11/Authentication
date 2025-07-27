<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="MyProfile.aspx.cs" Inherits="Authentication.User.MyProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h2 style="color: #e14658; text-align: center; font-size: 30px; margin-bottom: 20px;">My Profile</h2>

<div class="form-container">
    <asp:Label ID="lblMessage" runat="server" CssClass="success-message" Visible="false" />

    <!-- Role-based Panel: Shared Fields -->
    <asp:Panel ID="pnlCommon" runat="server">
        <div class="form-group">
            <label>Full Name:</label>
            <asp:TextBox ID="txtFullName" runat="server" CssClass="inputbox" />
            <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName" ErrorMessage="Full Name is required" CssClass="error-message" Display="Dynamic" />
        </div>

        <div class="form-group">
            <label>Email:</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="inputbox" ReadOnly="true" />
        </div>

        <div class="form-group">
            <label>User Role:</label>
            <asp:DropDownList ID="ddlRole" runat="server" CssClass="inputbox" Enabled="false" />
        </div>
    </asp:Panel>

    <!-- Organizer Section -->
    <asp:Panel ID="pnlOrganizer" runat="server" Visible="false">
        <div class="form-group">
            <label>Phone:</label>
            <asp:TextBox ID="txtOrgPhone" runat="server" CssClass="inputbox" />
        </div>

        <div class="form-group">
            <label>Organization Name:</label>
            <asp:TextBox ID="txtOrgName" runat="server" CssClass="inputbox" />
        </div>

        <div class="form-group">
            <label>Organization Type:</label>
            <asp:DropDownList ID="ddlOrgType" runat="server" CssClass="inputbox">
                <asp:ListItem Text="--Select--" Value="" />
                <asp:ListItem Text="NGO" />
                <asp:ListItem Text="Educational" />
                <asp:ListItem Text="Corporate" />
                <asp:ListItem Text="Cultural" />
                <asp:ListItem Text="Private" />
                <asp:ListItem Text="Other" />
            </asp:DropDownList>
        </div>

        <div class="form-group">
            <label>Designation:</label>
            <asp:TextBox ID="txtDesignation" runat="server" CssClass="inputbox" />
        </div>

        <div class="form-group">
            <label>Office Address:</label>
            <asp:TextBox ID="txtOfficeAddress" runat="server" CssClass="inputbox" TextMode="MultiLine" Rows="2" />
        </div>

        <div class="form-group">
            <label>City / State / Country / PIN:</label>
            <asp:TextBox ID="txtCity" runat="server" CssClass="inputbox" Placeholder="City" />
            <asp:TextBox ID="txtState" runat="server" CssClass="inputbox" Placeholder="State" />
            <asp:TextBox ID="txtCountry" runat="server" CssClass="inputbox" Placeholder="Country" />
            <asp:TextBox ID="txtPinCode" runat="server" CssClass="inputbox" Placeholder="PIN / ZIP" />
        </div>

        <div class="form-group">
            <label>Types of Events Organized:</label>
            <asp:CheckBoxList ID="cblOrgEvents" runat="server" CssClass="checkbox-list">
                <asp:ListItem Text="Conference" />
                <asp:ListItem Text="Seminar" />
                <asp:ListItem Text="Workshop" />
                <asp:ListItem Text="Cultural Event" />
                <asp:ListItem Text="Hackathon" />
                <asp:ListItem Text="Exhibition" />
                <asp:ListItem Text="Sports" />
            </asp:CheckBoxList>
        </div>

        <div class="form-group">
            <label>Upload Organization Logo:</label>
            <asp:FileUpload ID="fuLogo" runat="server" CssClass="inputbox" />
        </div>

        <div class="form-group">
            <label>Upload ID Proof:</label>
            <asp:FileUpload ID="fuOrgID" runat="server" CssClass="inputbox" />
        </div>

        <div class="form-group">
            <label>Website:</label>
            <asp:TextBox ID="txtOrgWebsite" runat="server" CssClass="inputbox" Placeholder="Website URL" />
        </div>

        <div class="form-group">
            <label>LinkedIn:</label>
            <asp:TextBox ID="txtOrgLinkedIn" runat="server" CssClass="inputbox" Placeholder="LinkedIn URL" />
        </div>

        <div class="form-group">
            <label>Facebook / Instagram:</label>
            <asp:TextBox ID="txtOrgSocial" runat="server" CssClass="inputbox" Placeholder="Social Media URL" />
        </div>
    </asp:Panel>

    <!-- Volunteer Section -->
    <asp:Panel ID="pnlVolunteer" runat="server" Visible="false">
        <div class="form-group">
            <label>Mobile Number:</label>
            <asp:TextBox ID="txtMobile" runat="server" CssClass="inputbox" />
        </div>

        <div class="form-group">
            <label>Date of Birth:</label>
            <asp:TextBox ID="txtDob" runat="server" CssClass="inputbox" TextMode="Date" />
        </div>

        <div class="form-group">
            <label>Gender:</label>
            <asp:DropDownList ID="ddlGender" runat="server" CssClass="inputbox">
                <asp:ListItem Text="--Select--" Value="" />
                <asp:ListItem Text="Male" />
                <asp:ListItem Text="Female" />
                <asp:ListItem Text="Other" />
            </asp:DropDownList>
        </div>

        <div class="form-group">
            <label>Volunteer Skills:</label>
            <asp:TextBox ID="txtSkills" runat="server" CssClass="inputbox" />
        </div>

        <div class="form-group">
            <label>Languages Known:</label>
            <asp:TextBox ID="txtLanguages" runat="server" CssClass="inputbox" />
        </div>

        <div class="form-group">
            <label>Highest Qualification:</label>
            <asp:TextBox ID="txtQualification" runat="server" CssClass="inputbox" />
        </div>

        <div class="form-group">
            <label>ID Proof Upload:</label>
            <asp:FileUpload ID="fuVolunteerID" runat="server" CssClass="inputbox" />
        </div>

        <div class="form-group">
            <label>Current Address:</label>
            <asp:TextBox ID="txtCurrentAddress" runat="server" CssClass="inputbox" TextMode="MultiLine" Rows="2" />
        </div>

        <div class="form-group">
            <label>City / State / Country / PIN:</label>
            <asp:TextBox ID="TextBox1" runat="server" CssClass="inputbox" Placeholder="City" />
            <asp:TextBox ID="TextBox2" runat="server" CssClass="inputbox" Placeholder="State" />
            <asp:TextBox ID="TextBox3" runat="server" CssClass="inputbox" Placeholder="Country" />
            <asp:TextBox ID="TextBox4" runat="server" CssClass="inputbox" Placeholder="PIN / ZIP" />
        </div>

        <div class="form-group">
            <label>Preferred Type of Events:</label>
            <asp:CheckBoxList ID="cblVolunteerEvents" runat="server" CssClass="checkbox-list">
                <asp:ListItem Text="Workshop" />
                <asp:ListItem Text="Seminar" />
                <asp:ListItem Text="Cultural Event" />
                <asp:ListItem Text="Hackathon" />
                <asp:ListItem Text="Exhibition" />
                <asp:ListItem Text="Sports" />
            </asp:CheckBoxList>
        </div>

        <div class="form-group">
            <label>Preferred Mode:</label>
            <asp:DropDownList ID="ddlMode" runat="server" CssClass="inputbox">
                <asp:ListItem Text="Online" />
                <asp:ListItem Text="Offline" />
                <asp:ListItem Text="Hybrid" />
            </asp:DropDownList>
        </div>
    </asp:Panel>

    <!-- Participant Section -->
    <asp:Panel ID="pnlParticipant" runat="server" Visible="false">
        <div class="form-group">
            <label>Institute / Company Name:</label>
            <asp:TextBox ID="txtInstitute" runat="server" CssClass="inputbox" />
        </div>

        <div class="form-group">
            <label>Course / Department:</label>
            <asp:TextBox ID="txtCourse" runat="server" CssClass="inputbox" />
        </div>

        <div class="form-group">
            <label>Year of Study / Experience:</label>
            <asp:TextBox ID="txtYear" runat="server" CssClass="inputbox" />
        </div>
    </asp:Panel>

    <div style="text-align:center; margin-top: 20px;">
        <asp:Button ID="btnUpdate" runat="server" Text="Update Profile" CssClass="submit-button" OnClick="btnUpdate_Click" />
        <asp:Button ID="btnBack" runat="server" Text="Back to Dashboard" CssClass="submit-button" PostBackUrl="~/User/UserDashboard.aspx" />
    </div>
</div>

</asp:Content>






