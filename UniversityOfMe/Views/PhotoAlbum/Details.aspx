﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<PhotoAlbum>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	University Of Me - Album -  <%= Model.Get().Name %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#submitPhoto").hide();
            $("#stars-wrapper1").stars();
            $('#addphoto').click(function () {
                $("#newphotos").show();
            });
            $("#newphotos").hide();
        });
    </script>

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="banner black full red-top small"> 
			<span class="album"><%= Model.Get().Name %> <span class="user">by <%= NameHelper.FullName(Model.Get().User) %></span></span> 
			<% if (Model.User.Id == Model.Get().CreatedByUserId) { %>
                <div class="buttons"> 
                    <%= Html.ActionLink("Edit", "Edit", "PhotoAlbum", new { id = Model.Get().Id }, new { @class = "edit mr22" })%>                
                    <%= Html.ActionLink("Remove Album", "Delete", "PhotoAlbum", new { id = Model.Get().Id }, new { @class = "remove mr26" })%>
                    <a id="addphoto" class="add" href="#">Add New Photos</a>				
			    </div> 
            <% } %>
		</div> 
        <div id="newphotos">
		    <div class="center mb25"> 
                <% using (Html.BeginForm("Create", "Photo", FormMethod.Post, new { enctype = "multipart/form-data" })) { %>
                    <%= Html.Hidden("AlbumId", Model.Get().Id) %>
		            <input type="file" name="imagefile" id="iamgefile" class="w392 mr44" /> 
		            <input type="submit" name="upload" class="btn site" value="Upload" /> 
                <% } %>
		    </div> 
        </div>


        <% foreach (Photo myPhoto in Model.Get().Photos.Where(p => !p.ProfilePicture)) { %>
		    <div class="photo"> 
			    <a href="<%= URLHelper.PhotoDisplayUrl(myPhoto) %>"><img src="<%= PhotoHelper.ConstructUrl(myPhoto.ImageName) %>" alt="photo" /></a> 
		    </div> 
        <% } %>
	</div> 
</asp:Content>