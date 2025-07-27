<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignupConfirm.aspx.cs" Inherits="Authentication.SignupConfirm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>OTP Verification</title>
    <link href="Styles/main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-container">
            <h3 style="text-align:center; color: #e14658;">OTP Verification</h3>

            <asp:Label ID="lblOtpMessage" runat="server" CssClass="error-message" />

            <p style="text-align:center;">Your OTP is: 
                <strong id="otpDisplay" style="color: #3f3250;"><%= Session["OTP"] %></strong>
            </p>

            <div style="text-align:center;">
                <button type="button" class="submit-button" onclick="copyOTP()">Copy OTP</button>
            </div>

            <p style="text-align:center;">Time remaining: <span id="timer">60</span> seconds</p>

            <asp:TextBox ID="txtEnteredOtp" runat="server" CssClass="inputbox" placeholder="Enter OTP here" />

            <div style="text-align:center;">
                <asp:Button ID="btnVerify" runat="server" Text="Verify OTP" CssClass="submit-button" OnClick="btnVerify_Click" />
            </div>
        </div>
    </form>

    <script>
        var otpText = document.getElementById("otpDisplay")?.textContent || "";
        var timer = 60;
        var interval = setInterval(function () {
            timer--;
            document.getElementById("timer").textContent = timer;
            if (timer <= 0) {
                clearInterval(interval);
                alert("OTP expired. Please generate a new one.");
                window.location.href = "Signup.aspx";
            }
        }, 1000);

        function copyOTP() {
            if (navigator.clipboard) {
                navigator.clipboard.writeText(otpText).then(function () {
                    alert("OTP copied to clipboard!");
                }, function () {
                    alert("Failed to copy OTP.");
                });
            } else {
                // fallback for older browsers
                var tempInput = document.createElement("input");
                tempInput.value = otpText;
                document.body.appendChild(tempInput);
                tempInput.select();
                document.execCommand("copy");
                document.body.removeChild(tempInput);
                alert("OTP copied!");
            }
        }
    </script>
</body>
</html>