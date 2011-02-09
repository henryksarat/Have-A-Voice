<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.Restriction>" %>
<%@ Import Namespace="HaveAVoice.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit</h2>

    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>
    
    <% using (Html.BeginForm()) {%>
    
        <fieldset>
            <legend>Fields</legend>
            <p>
                <label for="Name">Name:</label>
                <%= Html.TextBox("Name", Model.Name)%>
                <%= Html.ValidationMessage("Name", "*")%>
            </p>
            <p>
                <label for="Description">Description:</label>
                <%= Html.TextBox("Description", Model.Description)%>
                <%= Html.ValidationMessage("Description", "*")%>
            </p>
            
            Issues
            <p>
                <label for="Name">Posts time limit to work under:</label>
                <%= Html.TextBox("IssuePostsTimeLimit", Model.IssuePostsTimeLimit)%>
            </p>
            <p>
                <label for="Name">How long to wait before postings:</label>
                <%= Html.TextBox("IssuePostsWaitTimeBeforePostSeconds", Model.IssuePostsWaitTimeBeforePostSeconds)%>
            </p>
            <p>
                <label for="Name">Number of posts within time limit:</label>
                <%= Html.TextBox("IssuePostsWithinTimeLimit", Model.IssuePostsWithinTimeLimit)%>
            </p>

            Issues Replys

            <p>
                <label for="Name">Posts time limit to work under:</label>
                <%= Html.TextBox("IssueReplyPostsTimeLimit", Model.IssueReplyPostsTimeLimit)%>
            </p>
            <p>
                <label for="Name">How long to wait before postings:</label>
                <%= Html.TextBox("IssueReplyPostsWaitTimeBeforePostSeconds", Model.IssueReplyPostsWaitTimeBeforePostSeconds)%>
            </p>
            <p>
                <label for="Name">Number of posts within time limit:</label>
                <%= Html.TextBox("IssueReplyPostsWithinTimeLimit", Model.IssueReplyPostsWithinTimeLimit)%>
            </p>
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

