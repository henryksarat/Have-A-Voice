<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInModel<InboxMessage>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Inbox</h2>

    <table border="0" cellspacing="0" cellpadding="0" width="100%">
    
    <% using (Html.BeginForm())
       { %>
       
            <p>
               <%= Html.Encode(ViewData["Message"]) %>
            </p>
            <p>
               <%= Html.Encode(TempData["Message"]) %>
            </p>
            <% if (Model != null) { %>
                <% foreach (var item in Model.Models) { %>
                
                    <tr align="center">
                        <td>
                            <%= MessageHelper.MessageList(item.FromUserId, item.FromUsername, item.MessageId, item.Subject, item.LastReply, item.DateTimeStamp, item.Viewed) %>
                        </td>
                    </tr> 
                <% } %>
                    <tr>
                        <td>
                            <input type="submit" value="Delete" />
                        </td>
                    </tr> 
                    <tr>
                        <td>
                            <%= Html.ActionLink("Edit UserAccount", "Edit", "User")%> |
                        </td>
                    </tr>   
            
            <% } %>
        <% } %>

        </table>
</asp:Content>

