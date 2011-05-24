<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<PhotoAlbum>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Gallery
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

	<div class="eight last"> 
		<div class="create"> 
			<div class="banner black full red-top small"> 
				<span class="album">CREATE ALBUM</span> 
			</div> 
            <% using (Html.BeginForm("Create", "PhotoAlbum", FormMethod.Post)) {%>
			    <label for="title">Title:</label> 
			    <input type="text" class="half" name="title" id="title" /> 
                <%= Html.ValidationMessage("Title", "*")%>
			    <label for="desc" class="mt25">Description:</label> 
			    <textarea name="desc" id="desc" class="full" rows="6"></textarea> 
                <%= Html.ValidationMessage("Description", "*")%>
			    <div class="right"> 
				    <input type="submit" name="submit" class="btn teal mr14" value="Submit" /> 
				    <input type="button" name="cancel" class="btn teal" value="Cancel" /> 
			    </div> 
            <% } %>
		</div> 
	</div> 
</asp:Content>