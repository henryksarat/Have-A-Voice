<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.UserPicturesModel>" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UserPictures
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Your Pictures</h2>
    
    <% using(Html.BeginForm()) {%>
    <p>
        <strong>Current profile picture:</strong><br />
        <%= Html.Hidden("ProfilePictureURL", Model.ProfilePictureURL) %>
        <%= Html.Hidden("UserId", Model.UserId) %>
        <a href="<%= Model.ProfilePictureURL %>"><img src="<%= Model.ProfilePictureURL %>"  style="border-style: none"  alt="" width="100px" height="100px" /></a>
    <p style="color:Red">
        <%= Html.Encode(ViewData["Message"]) %>
    </p>
    </p>
    <table>
        <tr>
            <th></th>
            <th></th>
            <th>
                Date Uploaded
            </th>
        </tr>

        <% foreach(var item in Model.UserPictures) { %>

               <% if(item.ProfilePicture == true)
                   continue; %>
        
            <tr>
                <td>
                    <%= CheckBoxHelper.StandardCheckbox("SelectedProfilePictureId", item.Id.ToString(), Model.SelectedUserPictures.Contains(item.Id)) %>
                </td>
                <td>
                    <a href="<%= HAVConstants.USER_PICTURE_LOCATION + "/" + item.ImageName %>">
                    <img src="<%= HAVConstants.USER_PICTURE_LOCATION + "/" + item.ImageName %>" style="border-style: none" alt="" width="100px" height="100px" />
                    </a>
                </td>
                <td>
                    <%= Html.Encode(String.Format("{0:g}", item.DateTimeStamp))%>
                </td>
            </tr>
        
        <% } %>
 
    </table>
    <p>
        <button name="button" value="SetProfilePicture">Set to Profile Picture</button>
        <button name="button" value="Delete">Delete</button>
    </p>
    <% } %>
    
</asp:Content>

