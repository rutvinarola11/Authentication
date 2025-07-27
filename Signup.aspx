<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="Authentication.Signup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<h2>Sign Up</h2>

<div class="form-container">
    <asp:Label ID="lblMessage" runat="server" CssClass="error-message" />

    <table class="register-table">
        <tr>
            <td>Full Name:</td>
            <td>
                <asp:TextBox ID="txtName" runat="server" CssClass="inputbox" />
                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                    ErrorMessage="Name is required" CssClass="error-message" Display="Dynamic" />
            </td>
        </tr>

        <tr>
            <td>Email:</td>
            <td>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="inputbox" />
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="Email is required" CssClass="error-message" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                    ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                    ErrorMessage="Invalid email format" CssClass="error-message" Display="Dynamic" />
            </td>
        </tr>

        <tr>
            <td>Password:</td>
            <td>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="inputbox" />
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                    ErrorMessage="Password is required" CssClass="error-message" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtPassword"
                    ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"
                    ErrorMessage="Min 8 chars with number" CssClass="error-message" Display="Dynamic" />
            </td>
        </tr>

        <tr>
            <td>Confirm Password:</td>
            <td>
                <asp:TextBox ID="txtConfirm" runat="server" TextMode="Password" CssClass="inputbox" />
                <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirm"
                    ControlToCompare="txtPassword" ErrorMessage="Passwords do not match"
                    CssClass="error-message" Display="Dynamic" />
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
                    InitialValue="" ErrorMessage="Please select a role" CssClass="error-message" Display="Dynamic" />
            </td>
        </tr>

        <tr>
            <td>Security Question:</td>
            <td>
                <asp:DropDownList ID="ddlSecurityQuestion" runat="server" CssClass="inputbox">
                    <asp:ListItem Text="--Select Question--" Value="" />
                    <asp:ListItem Text="Your favorite color?" Value="color" />
                    <asp:ListItem Text="Your pet’s name?" Value="pet" />
                    <asp:ListItem Text="Your mother’s name?" Value="mother" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvQuestion" runat="server" ControlToValidate="ddlSecurityQuestion"
                    InitialValue="" ErrorMessage="Select a question" CssClass="error-message" Display="Dynamic" />
            </td>
        </tr>

        <tr>
            <td>Security Answer:</td>
            <td>
                <asp:TextBox ID="txtSecurityAnswer" runat="server" CssClass="inputbox" />
                <asp:RequiredFieldValidator ID="rfvAnswer" runat="server" ControlToValidate="txtSecurityAnswer"
                    ErrorMessage="Answer is required" CssClass="error-message" Display="Dynamic" />
            </td>
        </tr>

        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:Button ID="btnGenerateOtp" runat="server" Text="Sign Up" CssClass="submit-button" OnClick="GenerateOtp_Click" />
            </td>
        </tr>
    </table>

    <!-- Hidden OTP fields -->
    <asp:HiddenField ID="hdnOtp" runat="server" />
</div>

<!-- OTP Modal -->
<div id="otpModal" class="form-container" style="display:none; background-color: #fff; color: #3f3250;">
    <h3 style="text-align:center; color: #e14658;">OTP Verification</h3>
    <p style="text-align:center;">Your OTP is: <strong id="generatedOtp"></strong></p>
    <div style="text-align:center;">
        <button type="button" class="submit-button" onclick="copyOTP()">Copy OTP</button>
    </div>
    <p style="text-align:center;">Time remaining: <span id="timer">60</span> seconds</p>
    <input type="text" id="otpInputBox" class="inputbox" placeholder="Enter OTP here" />
    <div style="text-align:center; margin-top: 10px;">
        <button class="submit-button" onclick="verifyOtp()">Verify OTP</button>
    </div>
</div>

<script>
    var otp = '';
    var timerInterval;

    function showOtpPopup(receivedOtp) {
        otp = receivedOtp;
        document.getElementById("generatedOtp").textContent = otp;
        document.getElementById("otpInputBox").value = "";

        let modal = document.getElementById("otpModal");
        modal.style.display = "block";

        var timer = 60;
        document.getElementById("timer").textContent = timer;
        clearInterval(timerInterval);

        timerInterval = setInterval(() => {
            timer--;
            document.getElementById("timer").textContent = timer;
            if (timer === 0) {
                clearInterval(timerInterval);
                alert("OTP expired. Please try again.");
                window.location.href = "Signup.aspx";
            }
        }, 1000);
    }

    function copyOTP() {
        navigator.clipboard.writeText(otp);
        document.getElementById("otpInputBox").value = otp;
    }

    function verifyOtp() {
        var entered = document.getElementById("otpInputBox").value;
        if (entered === otp) {
            clearInterval(timerInterval);
            window.location.href = "SignupConfirm.aspx";
        } else {
            alert("Invalid OTP. Please try again.");
        }
    }
</script>




</asp:Content>
