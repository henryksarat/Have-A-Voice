<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.RestrictionModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create</h2>

    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm("Create", "Restriction")) {%>

        <%= Html.Encode(ViewData["Message"]) %>
        
        <fieldset>
            <legend>Fields</legend>
            <p>
                <label for="Name">Name:</label>
                <%= Html.TextBox("Name", Model.Restriction.Name)%>
                <%= Html.ValidationMessage("Name", "*")%>
            </p>
            <p>
                <label for="Description">Description:</label>
                <%= Html.TextBox("Description", Model.Restriction.Description)%>
                <%= Html.ValidationMessage("Description", "*")%>
            </p>
            <p>
                <label for="Name">Issue Posts Within Time Limit:</label>
                <%= Html.TextBox("IssuePostsWithinTimeLimit", Model.Restriction.IssuePostsWithinTimeLimit)%>
                <%= Html.ValidationMessage("IssuePostsWithinTimeLimit", "*")%>
            </p>
            <p>
                <label for="Name">Issue Posts Time Limit:</label>
                <%= Html.TextBox("IssuePostsTimeLimit", Model.Restriction.IssuePostsTimeLimit)%>
                <%= Html.ValidationMessage("IssuePostsTimeLimit", "*")%>
            </p>
            <p>
                <label for="Name">Issue Posts Wait Time Before Post Seconds:</label>
                <%= Html.TextBox("IssuePostsWaitTimeBeforePostSeconds", Model.Restriction.IssuePostsWaitTimeBeforePostSeconds)%>
                <%= Html.ValidationMessage("IssuePostsWaitTimeBeforePostSeconds", "*")%>
            </p>
            <p>
                <label for="Name">Issue Reply Posts Within Time Limit:</label>
                <%= Html.TextBox("IssueReplyPostsWithinTimeLimit", Model.Restriction.IssueReplyPostsWithinTimeLimit)%>
                <%= Html.ValidationMessage("IssueReplyPostsWithinTimeLimit", "*")%>
            </p>
            <p>
                <label for="Name">Issue Reply Posts Time Limit:</label>
                <%= Html.TextBox("IssueReplyPostsTimeLimit", Model.Restriction.IssueReplyPostsTimeLimit)%>
                <%= Html.ValidationMessage("IssueReplyPostsTimeLimit", "*")%>
            </p>
            <p>
                <label for="Name">Issue Reply Posts Wait Time Before Post Seconds:</label>
                <%= Html.TextBox("IssueReplyPostsWaitTimeBeforePostSeconds", Model.Restriction.IssueReplyPostsWaitTimeBeforePostSeconds)%>
                <%= Html.ValidationMessage("IssueReplyPostsWaitTimeBeforePostSeconds", "*")%>
            </p>
            <p>
                <label for="Name">Issue Reply Comment Posts Within Time Limit:</label>
                <%= Html.TextBox("IssueReplyCommentPostsWithinTimeLimit", Model.Restriction.IssueReplyCommentPostsWithinTimeLimit)%>
                <%= Html.ValidationMessage("IssueReplyCommentPostsWithinTimeLimit", "*")%>
            </p>
            <p>
                <label for="Name">Issue Reply Comment Posts Time Limit:</label>
                <%= Html.TextBox("IssueReplyCommentPostsTimeLimit", Model.Restriction.IssueReplyCommentPostsTimeLimit)%>
                <%= Html.ValidationMessage("IssueReplyCommentPostsTimeLimit", "*")%>
            </p>
            <p>
                <label for="Name">Issue Reply Comment Posts Wait Time Before Post Seconds:</label>
                <%= Html.TextBox("IssueReplyCommentPostsWaitTimeBeforePostSeconds", Model.Restriction.IssueReplyCommentPostsWaitTimeBeforePostSeconds)%>
                <%= Html.ValidationMessage("IssueReplyCommentPostsWaitTimeBeforePostSeconds", "*")%>
            </p>
            <p>
                <label for="Name">Merge Issue Request Within Time Limit:</label>
                <%= Html.TextBox("MergeIssueRequestWithinTimeLimit", Model.Restriction.MergeIssueRequestWithinTimeLimit)%>
                <%= Html.ValidationMessage("MergeIssueRequestWithinTimeLimit", "*")%>
            </p>
            <p>
                <label for="Name">Merge Issue Request Time Limit:</label>
                <%= Html.TextBox("MergeIssueRequestTimeLimit", Model.Restriction.MergeIssueRequestTimeLimit)%>
                <%= Html.ValidationMessage("MergeIssueRequestTimeLimit", "*")%>
            </p>
            <p>
                <label for="Name">Merge Issue Request Wait Time Before Post Seconds:</label>
                <%= Html.TextBox("MergeIssueRequestWaitTimeBeforePostSeconds", Model.Restriction.MergeIssueRequestWaitTimeBeforePostSeconds)%>
                <%= Html.ValidationMessage("MergeIssueRequestWaitTimeBeforePostSeconds", "*")%>
            </p>
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>

    <% } %>

</asp:Content>
