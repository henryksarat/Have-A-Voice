<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<PhotoDisplayView>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="Social.Generic.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Gallery
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last">
        <% Html.RenderPartial("Validation"); %>
        <% Html.RenderPartial("Message"); %>
		<div class="banner black full red-top small">
			<span class="album">SAMPLE ALBUM <span class="user">by Anca Foster</span></span>
			<div class="buttons">
                <%= Html.ActionLink("Delete", "Delete", "Photo", new { id = Model.Get().Photo.Id, albumId = Model.Get().Photo.PhotoAlbumId }, new { @class = "remove mr17" })%>
	            <%= Html.ActionLink("Set as Profile Picture", "SetProfilePicture", "Photo", new { id = Model.Get().Photo.Id }, new { @class = "profile mr22" })%>
                <%= Html.ActionLink("Set as Album Cover", "SetAlbumCover", "Photo", new { id = Model.Get().Photo.Id }, new { @class = "create" })%>
			</div>
		</div>
		<div class="image">
            <% if (Model.Get().PreviousPhoto != null) { %>
                <a href="/Photo/Display/<%= Model.Get().PreviousPhoto.Id %>" class="nav-btn flft clearfix">&lt; Previous</a>
            <% } %>
			
            <% if (Model.Get().NextPhoto != null) { %>
                <a href="/Photo/Display/<%= Model.Get().NextPhoto.Id %>" class="nav-btn frgt clearfix">Next &gt;</a>
            <% } %>
            <div class="imagedisplay">
                <img src="<%= PhotoHelper.ConstructUrl(Model.Get().Photo.ImageName) %>" alt="photo" />
            </div>
		</div>
					
		<div class="banner full">
			COMMENTS
		</div>
							
		<div id="review">
			<div class="create">
                <% using (Html.BeginForm("Create", "PhotoComment", new { id = Model.Get().Photo.Id }, FormMethod.Post, null)) { %>
				    <%= Html.TextArea("Comment", string.Empty, 6, 0, new { @class = "full" })%>
                    <%= Html.ValidationMessage("Comment", "*")%>
									
				    <div class="frgt mt13">
					    <input type="submit" class="frgt btn site" name="post" value="Post Comment" />
				    </div>
				    <div class="clearfix"></div>
                <% } %>
			</div>
								
			<div class="clearfix"></div>
							
            <% foreach (PhotoComment myComment in Model.Get().Photo.PhotoComments) { %>	
			    <div class="review">
				    <table border="0" cellpadding="0" cellspacing="0">
					    <tr>
						    <td class="avatar">
							    <img src="<%= PhotoHelper.ProfilePicture(myComment.User) %>" class="profile big mr22" />
						    </td>
						    <td>
							    <div class="red bld"><%= NameHelper.FullName(myComment.User) %>
								    <div class="frgt">
									    <span class="gray small nrm"><%= LocalDateHelper.ToLocalTime(myComment.DateTimeStamp)%></span>
								    </div>
							    </div>
							    <%= myComment.Comment %>
						    </td>
					    </tr>
				    </table>
		
				    <div class="clearfix"></div>
			    </div>
            <% } %>
		</div>
					
	</div>
</asp:Content>