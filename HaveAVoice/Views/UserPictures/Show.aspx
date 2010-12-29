<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.UserPicture>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Gallery
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Gallery</h2><br /><br />

    <%= Html.Encode(ViewData["Message"]) %><br /><br />
    
    <table>
        <tr>
        <% foreach (var item in Model) { %>
            <td><%= ImageHelper.Image("../../UserPictures/" + item.ImageName, 200, 200)%></td>
    
        <% } %>
        </tr>
    </table>
</asp:Content>