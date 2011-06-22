<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<PhotoAlbum>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Gallery
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="create"> 
			<div class="banner black full red-top small"> 
				<span class="album">CREATE ALBUM</span> 
			</div> 
            <% using (Html.BeginForm("Create", "PhotoAlbum", FormMethod.Post)) {%>
			    <label for="name">Title:</label> 
                <%= Html.TextBox("Name", string.Empty, new { @class = "half" }) %>
                <%= Html.ValidationMessage("Name", "*")%>
			    <label for="desc" class="mt25">Description:</label> 
			    <%= Html.TextArea("Description", string.Empty, 6, 0, new { @class = "full" })%>
                <%= Html.ValidationMessage("Description", "*")%>
			    <div class="right"> 
				    <input type="submit" name="submit" class="btn site mr14" value="Submit" /> 
				    <input type="button" name="cancel" class="btn site" value="Cancel" /> 
			    </div> 
            <% } %>
		</div> 
	</div> 
</asp:Content>