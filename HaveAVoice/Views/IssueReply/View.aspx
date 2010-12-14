<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.IssueReplyDetailsModel>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	View a Reply
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% using (Html.BeginForm())
       { %>
        
        <%= Html.ValidationSummary("Your comment wasn't posted. Please correct the errors and try again.") %>
            <p>
                <%= Html.Encode(ViewData["Message"]) %>
            </p>
            <p>
                <%= Html.Encode(TempData["Message"]) %>
            </p>
            <p>
                <%= IssueHelper.IssueReply(Model) %>
            </p>
            <p></p>
            <br />
            <p>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                    <td>Comments</td>
                    </tr>
                    <tr>
                        <td>
                            <% foreach (IssueReplyComment comment in Model.Comments) { %>
                                <p>
                                    <%= IssueHelper.Comment(comment) %>
                                </p>
                                <p>
                                    <% if (comment.User.Id == HAVUserInformationFactory.GetUserInformation().Details.Id || HAVPermissionHelper.AllowedToPerformAction(HAVUserInformationFactory.GetUserInformation(), HAVPermission.Edit_Any_Issue_Reply_Comment)) { %>
                                        <%= Html.ActionLink("Edit", "Edit", "IssueReplyComment", new { id = comment.Id }, null)%>
                                    <% } %>
                                    <% if (comment.User.Id == HAVUserInformationFactory.GetUserInformation().Details.Id || HAVPermissionHelper.AllowedToPerformAction(HAVUserInformationFactory.GetUserInformation(), HAVPermission.Delete_Any_Issue_Reply_Comment)) { %>
                                        <%= Html.ActionLink("Delete", "Delete", "IssueReplyComment", new { issueReplyId = Model.IssueReply.Id, issueReplyCommentId = comment.Id }, null)%>
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
                <label for="Name"><strong>Post Comment</strong></label>
            </p>
            <p>
                <%= Html.TextArea("Comment",
                    HAVUserInformationFactory.IsLoggedIn() ? Model.Comment : "You must be logged in to reply", 5, 30,
                    HAVUserInformationFactory.IsLoggedIn() ? null : new { @readonly = "readonly" })%>
                    
                <%= Html.ValidationMessage("Comment", "*")%>
            </p>
            
            <p>
                <input type="submit" value="Submit" />
            </p>
    <% } %>   
    
</asp:Content>
