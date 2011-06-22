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
				<span class="edit"><%=Model.Get().Name %></span> 
				<div class="buttons"> 
                    <%= Html.ActionLink("Remove Album", "Delete", "PhotoAlbum", new { id = Model.Get().Id }, new { @class = "remove" })%>
				</div> 
			</div> 
            <% using (Html.BeginForm("Edit", "PhotoAlbum", FormMethod.Post)) { %>
                <%= Html.Hidden("AlbumId", Model.Get().Id) %>
			    <label for="name">Title:</label> 
                <%: Html.TextBox("Name", Model.Get().Name, new { @class = "half" })%>
                <%: Html.ValidationMessage("Name", "*")%>
			    <label for="Description" class="mt25">Description:</label> 
                <%= Html.TextArea("Description", Model.Get().Description, 6, 0, new { @class = "full" })%>
                <%: Html.ValidationMessage("Description", "*")%>
			    <div class="right"> 
				    <input type="submit" name="submit" class="btn site mr14" value="Submit" /> 
				    <input type="button" name="cancel" class="btn site" value="Cancel" /> 
			    </div> 
            <% } %>
		</div> 
	</div> 
</asp:Content>