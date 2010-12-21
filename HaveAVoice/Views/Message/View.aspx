<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.ViewMessageModel>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ViewMessage
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>ViewMessage</h2>
    
    <% using (Html.BeginForm("CreateReply", "Message")) { %>
        <%= Html.Hidden("Id", Model.Message.Id) %>
            <p>
                 <%= Html.Encode(ViewData["Message"]) %>
            </p>
            <% if (Model != null) {%>
            <tr>
                <td>
                    <%= MessageHelper.MessageItem(Model.Message.FromUser.Username, Model.Message.Subject, Model.Message.Body, Model.Message.DateTimeStamp)%>
                </td>
            </tr>    
            <tr>
                <td>
                    <table border="0" cellspacing="0" cellpadding="0" width="100%">
                	    <tr>
                		    <td style="width:30px"></td>
                		    <td>
                		         <% foreach (var reply in Model.Message.Replys) { %>
                                    <p>
                                        <%= MessageHelper.MessageItem(reply.User.Username, "", reply.Body, reply.DateTimeStamp)%>
                                    </p>
                                <%}%>
                    		
                		    </td>
                	    </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    
                    <%= Html.ValidationSummary("Reply was unsuccessful. Please correct the errors and try again.") %>
                    <%= Html.TextArea("Reply")%>
                    <%= Html.ValidationMessage("Reply", "*")%>
                </td>
            </tr>
            <tr>
                <td>
                    <p>
                        <input type="submit" value="Submit" />
                    </p>
                </td>
            </tr>
        <% } %>
    <% } %>
</asp:Content>
