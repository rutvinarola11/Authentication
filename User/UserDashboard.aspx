<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master"
    AutoEventWireup="true"
    CodeBehind="UserDashboard.aspx.cs"
    Inherits="Authentication.User.UserDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  <!-- Dashboard Banner -->
<div class="banner">
    <h1>Welcome to EventSphere!</h1>
    <p>Manage your events and volunteer work efficiently</p>
</div>

<!-- Quick Stats -->
<div class="grid grid-3">
    <!-- Organizer -->
    <div class="card" runat="server" id="phEventsCreated" visible="false">
        <h2><asp:Literal ID="litEventsCreated" runat="server" /></h2>
        <p>Events Created</p>
    </div>

    <div class="card" runat="server" id="phParticipantsRegistered" visible="false">
        <h2><asp:Literal ID="litParticipantsRegistered" runat="server" /></h2>
        <p>Participants Registered</p>
    </div>

    <!-- Volunteer -->
    <div class="card" runat="server" id="phEventsVolunteered" visible="false">
        <h2><asp:Literal ID="litEventsVolunteered" runat="server" /></h2>
        <p>Events Volunteered</p>
    </div>

    <div class="card" runat="server" id="phDutiesAssigned" visible="false">
        <h2><asp:Literal ID="litDutiesAssigned" runat="server" /></h2>
        <p>Duties Assigned</p>
    </div>

    <!-- Participant -->
    <div class="card" runat="server" id="phParticipationCount" visible="false">
        <h2><asp:Literal ID="litParticipationCount" runat="server" /></h2>
        <p>Events Participated</p>
    </div>

    <!-- Certificates: visible only to Participant & Volunteer -->
    <div class="card" runat="server" id="phCertificatesEarned" visible="false">
        <h2><asp:Literal ID="litCertificatesEarned" runat="server" /></h2>
        <p>Certificates Earned</p>
    </div>
</div>

  <!-- Quick Actions -->
<h2 style="margin-top: 40px;">Quick Actions</h2>
<div class="grid grid-2">

    <!-- Browse Events (common) -->
    <div class="card" runat="server" id="phBrowseEvents">
        <h3>Browse Events</h3>
        <p>Discover upcoming events and register</p>
        <a href="BrowseEvents.aspx" class="submit-button">Explore</a>
    </div>

    <!-- Create New Event (Organizer only) -->
    <div class="card" runat="server" id="phCreateEvent" visible="false">
        <h3>Create New Event</h3>
        <p>Organize your own event and manage volunteer activity</p>
        <a href="CreateEvent.aspx" class="submit-button">Create</a>
    </div>

    <!-- My Volunteer Events (Participant/Volunteer) -->
    <div class="card" runat="server" id="phMyVolunteerEvents" visible="false">
        <h3>My Volunteer Events</h3>
        <p>Track your volunteering and certificates</p>
        <a href="MyVolunteerEvents.aspx" class="submit-button">View</a>
    </div>
    <!-- Volunteer Dashboard Approval Modal-->
     <div class="modal fade" id="approvedModal" tabindex="-1" aria-labelledby="approvedModalLabel" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
          <div class="modal-header bg-success text-white">
            <h5 class="modal-title" id="approvedModalLabel">🎉 Application Approved</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body">
            <p>Your volunteer application has been <strong>approved</strong>! 🎉</p>
            <p>You can now download your ID pass.</p>
          </div>
          <div class="modal-footer">
            <asp:Button ID="btnDownloadID" runat="server" Text="Download ID Pass"
                CssClass="btn btn-primary" OnClick="btnDownloadID_Click" />
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
          </div>
        </div>
      </div>
    </div>

    <!-- My Organized Events (Organizer only) -->
    <div class="card" runat="server" id="phMyOrganizedEvents" visible="false">
        <h3>My Organized Events</h3>
        <p>View and manage events you've created</p>
        <a href="MyOrganizedEvents.aspx" class="submit-button">Manage</a>
    </div>

    <!-- My Profile (common) -->
    <div class="card">
        <h3>My Profile</h3>
        <p>View or edit your personal details</p>
        <a href="MyProfile.aspx" class="submit-button">Profile</a>
    </div>

    <!-- NEW: My Participation History (Participant only) -->
    <div class="card" runat="server" id="phParticipationHistory" visible="false">
        <h3>My Participation</h3>
        <p>View past event participations</p>
        <a href="MyParticipation.aspx" class="submit-button">History</a>
    </div>

    <!-- NEW: Feedback (Participant only) -->
    <div class="card" runat="server" id="phFeedback" visible="false">
        <h3>Feedback</h3>
        <p>Share feedback for events you attended</p>
        <a href="Feedback.aspx" class="submit-button">Give Feedback</a>
    </div>

    <!-- NEW: View Assigned Duties (Volunteer only) -->
    <div class="card" runat="server" id="phAssignedDuties" visible="false">
        <h3>Assigned Duties</h3>
        <p>Check your assigned volunteer tasks</p>
        <a href="AssignedDuties.aspx" class="submit-button">View</a>
    </div>

    <!-- NEW: Mark Attendance (Volunteer only) -->
    <div class="card" runat="server" id="phMarkAttendance" visible="false">
        <h3>Mark Attendance</h3>
        <p>Confirm your event participation</p>
        <a href="MarkAttendance.aspx" class="submit-button">Submit</a>
    </div>

    <!-- NEW: Volunteer Status (Volunteer only) -->
    <div class="card" runat="server" id="phVolunteerStatus" visible="false">
        <h3>Volunteer Status</h3>
        <p>Track approval and certificates</p>
        <a href="VolunteerStatus.aspx" class="submit-button">Check</a>
    </div>

    <!-- NEW: View Volunteer Applications (Organizer only) -->
    <div class="card" runat="server" id="phVolunteerApplications" visible="false">
        <h3>Volunteer Applications</h3>
        <p>Approve or reject volunteer requests</p>
        <a href="ViewVolunteers.aspx" class="submit-button">Review</a>
    </div>

    <!-- NEW: Manage Event Attendance (Organizer only) -->
    <div class="card" runat="server" id="phManageAttendance" visible="false">
        <h3>Attendance Manager</h3>
        <p>Verify attendance for certificates</p>
        <a href="ManageAttendance.aspx" class="submit-button">Manage</a>
    </div>

    <!-- NEW: View Feedback (Organizer only) -->
    <div class="card" runat="server" id="phViewFeedback" visible="false">
        <h3>View Feedback</h3>
        <p>See participant feedback for events</p>
        <a href="ViewFeedback.aspx" class="submit-button">View</a>
    </div>
</div>


    <!-- Upcoming Events Section -->
    <div>
        <h2 style="margin-top: 40px;">Your Upcoming Events</h2>
        <div class="grid grid-2">
            <asp:Literal ID="litUpcomingEvents" runat="server" />
        </div>
    </div>
      
      <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

<asp:Literal ID="litModalScript" runat="server"></asp:Literal>
</asp:Content>
