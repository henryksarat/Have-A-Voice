<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.ComplaintModel>" %>
<%@Import Namespace="HaveAVoice.Helpers" %>
<%@Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Report
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Report a problem</h2>

    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
    
    <%= Html.Encode(ViewData["Message"]) %>
    
    <% using (Html.BeginForm())
       { %>
            You are about to report a problem with <strong> 
            <%= Html.Encode(Model.TowardUser.Username) %></strong>
            against a
            <strong> <%= Html.Encode(Model.ComplaintType) %></strong>
            they posted. <br /><br />
            Summary of what they posted:<br />
            <%= Html.Encode(Model.SourceDescription) %> <br /><br />
            <%= Html.TextArea("Complaint",
                HAVUserInformationFactory.IsLoggedIn() ? string.Empty : "You must be logged in to report a problem.", 20, 50,
                HAVUserInformationFactory.IsLoggedIn() ? null : new { @readonly = "readonly" })%>
            <%= Html.ValidationMessage("Complaint", "*")%>
       <p>
            <input type="submit" value="Send Report" />
       </p>
       <% } %>
</asp:Content>
