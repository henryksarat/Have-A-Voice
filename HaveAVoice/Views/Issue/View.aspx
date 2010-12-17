<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.IssueModel>" %>
<%@Import Namespace="HaveAVoice.Models.View" %>
<%@Import Namespace="HaveAVoice.Helpers.UI" %>
<%@Import Namespace="HaveAVoice.Helpers" %>
<%@Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@Import Namespace="HaveAVoice.Helpers.Enums" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	View
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>View</h2>

    <% using (Html.BeginForm())
       { %>
       <% UserInformationModel myUserInformationModel = HAVUserInformationFactory.GetUserInformation(); %>
       <% HaveAVoice.Models.User myUser = myUserInformationModel.Details; %>
        
        <%= Html.ValidationSummary("Your reply wasn't posted. Please correct the errors and try again.") %>
        
        <fieldset>
            <legend>Fields</legend>
            <p>
                <%= Html.Encode(ViewData["Message"]) %>
            </p>
            <p>
                <%= Html.Encode(TempData["Message"]) %>
            </p>
            <p>
                <%= Html.Encode(Model.Issue.Title) %>
            </p>
            <p>
                <%= Html.Encode(Model.Issue.Description) %>
            </p>
            <p>
                <% if (Model.Issue.User.Id == myUser.Id || HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Edit_Any_Issue)) { %>
                    <%= Html.ActionLink("Edit", "Edit", new { id = Model.Issue.Id })%>
                <% } %>
                <% if (Model.Issue.User.Id == myUser.Id || HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Delete_Any_Issue)) { %>
                    <%= Html.ActionLink("Delete", "Delete", new { id = Model.Issue.Id })%>
                <% } %>
            </p>
            <br />
            <p>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td><strong>User Replys</strong></td>
                        <td><strong>Official Replys</strong></td>
                    </tr>
                    <tr>
                        <td>
                            <% foreach (IssueReplyModel reply in Model.UserReplys) { %>
                                <p>
                                    <%= IssueHelper.UserIssueReply(reply) %>
                                </p>
                                <% if (!reply.HasDisposition) { %>
                                        <%= Html.ActionLink("Like", "Disposition", "IssueReply", new { id = reply.Id, issueId = Model.Issue.Id, disposition = (int)Disposition.LIKE }, null)%>
                                        <%= Html.ActionLink("Dislike", "Disposition", "IssueReply", new { id = reply.Id, issueId = Model.Issue.Id, disposition = (int)Disposition.DISLIKE }, null)%>
                                <% } %>
                                <p>
                                    <% if (reply.User.Id == myUser.Id || HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Edit_Any_Issue_Reply)) { %>
                                        <%= Html.ActionLink("Edit", "Edit", "IssueReply", new { id = reply.Id }, null)%>
                                    <% } %>
                                   <% if (reply.User.Id == myUser.Id || HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Delete_Any_Issue_Reply)) { %>
                                        <%= Html.ActionLink("Delete", "Delete", "IssueReply", new { id = reply.Id, issueId = Model.Issue.Id }, null)%>
                                    <% } %>
                                </p>
                            <%}%>
                        </td>
                        <td>
                            <% foreach (IssueReplyModel reply in Model.OfficialReplys) { %>
                                <p>
                                    <%= IssueHelper.OfficialIssueReply(reply) %>
                                </p>
                                <p>
                                    <% if (reply.User.Id == myUser.Id || HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Edit_Any_Issue_Reply)) { %>
                                        <%= Html.ActionLink("Edit", "Edit", "IssueReply", new { id = reply.Id }, null)%>
                                    <% } %>
                                   <% if (reply.User.Id == myUser.Id || HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Delete_Any_Issue_Reply)) { %>
                                        <%= Html.ActionLink("Delete", "Delete", "IssueReply", new { id = reply.Id, issueId = Model.Issue.Id }, null)%>
                                    <% } %>
                                </p>
                            <%}%>
                        </td>
                    </tr>
                </table>
            </p>
            <p></p>
            <p></p>
            <p>
                <label for="Name"><strong>Reply</strong></label>
                <%= Html.TextArea("Reply",
                    HAVUserInformationFactory.IsLoggedIn() ? Model.Comment : "You must be logged in to reply", 5, 30,
                    HAVUserInformationFactory.IsLoggedIn() ? null : new { @readonly = "readonly" })%>
                <%= Html.ValidationMessage("Reply", "*")%>
                <% if (!HAVPermissionHelper.AllowedToPerformAction(myUserInformationModel, HAVPermission.Official_Account)) { %>   
                    <%= Html.CheckBox("Anonymous", Model.Anonymous) %> Post reply as Anonymous
                <% } %>
                <table>
                    <tr>
                        <td><label for="Like">Like</label> <%= Html.RadioButton("Disposition", Disposition.LIKE, Model.Disposition == Disposition.LIKE ? true : false)%></td>
                        <td><label for="Dislike">Dislike</label> <%= Html.RadioButton("Disposition", Disposition.DISLIKE, Model.Disposition == Disposition.DISLIKE ? true : false)%></td>
                        <td><%= Html.ValidationMessage("Disposition", "*")%></td>
                    </tr>
                </table>
            </p>
            
            <p>
                <input type="submit" value="Submit" />
            </p>

            <%= ComplaintHelper.IssueLink(Model.Issue.Id) %>
        </fieldset>
    <% } %>   

</asp:Content>

