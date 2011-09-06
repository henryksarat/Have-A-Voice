<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<PhotoAlbum>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Photo Albums
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
        <div class="create-feature-form"> 
		    <div class="banner black full red-top small"> 
			    <span class="mine"><%= NameHelper.FullName(Model.User) %> - ALBUMS</span> 
			    <div class="buttons"> 
				    <a href="/PhotoAlbum/Create" class="create">Create Album</a> 
			    </div> 
		    </div> 

            <% Html.RenderPartial("Message"); %>
            <% Html.RenderPartial("Validation"); %>

            <div class="padding-col">
                <% foreach (PhotoAlbum myAlbum in Model.Get()) { %>
		            <div class="album"> 
			            <a href="<%= URLHelper.PhotoAlbumDetailsUrl(myAlbum) %>"> 
				            <img src="<%= PhotoHelper.PhotoAlbumCover(myAlbum) %>" alt="photo" /> 
				            <br /> 
				            <%= myAlbum.Name%>
			            </a> 
		            </div> 
                <% } %>
            </div>
        </div>
	</div> 	
</asp:Content>