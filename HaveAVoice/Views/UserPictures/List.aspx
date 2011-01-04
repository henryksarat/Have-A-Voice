<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.UserPicture>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Gallery
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Gallery</h2><br /><br />
    <p>
        <%= Html.Encode(ViewData["Message"]) %><br /><br />
    </p>
    <p>
        <%= Html.Encode(TempData["Message"]) %><br /><br />
    </p>
    <table>
        <tr>
            <td>
                <% using (Html.BeginForm("Add", "UserPictures", FormMethod.Post, new { enctype = "multipart/form-data" })) { %>
			<div class="push_1 grid_6">
				<input type="file" id="ProfilePictureUpload" name="ProfilePictureUpload" size="23"/>
                <%= Html.ValidationMessage("ProfilePictureUpload", "*")%>
			</div>
                <p>
                    <input type="submit" value="Upload" />
               </p>
                <% } %> 
            </td>
        </tr>
        <tr>
        <% foreach (var item in Model) { %>
            <td><%= ImageHelper.Image("../../UserPictures/" + item.ImageName, 200, 200)%></td>
    
        <% } %>
        </tr>
    </table>
</asp:Content>