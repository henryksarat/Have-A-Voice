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
		<div class="create-feature-form create"> 
			<div class="banner black full small"> 
				<span class="edit"><%=Model.Get().Name %></span> 
				<div class="buttons"> 
                    <%= Html.ActionLink("Remove Album", "Delete", "PhotoAlbum", new { id = Model.Get().Id }, new { @class = "remove" })%>
				</div> 
			</div> 

            <% Html.RenderPartial("Message"); %>
            <% Html.RenderPartial("Validation"); %>

            <div class="padding-col">
                <% using (Html.BeginForm("Edit", "PhotoAlbum", FormMethod.Post)) { %>
                    <%= Html.Hidden("AlbumId", Model.Get().Id) %>

                    <div class="field-holder">
			            <label for="name">Title</label> 
                        <%= Html.TextBox("Name", Model.Get().Name)%>
                        <%= Html.ValidationMessage("Name", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
			            <label for="Description">Description</label> 
                        <%= Html.TextArea("Description", Model.Get().Description, new { @class = "textarea" })%>
                        <%= Html.ValidationMessage("Description", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
                        <div class="right">
				            <input type="submit" name="submit" class="btn site button-padding" value="Submit" />  
                        </div>
			        </div> 
                <% } %>
            </div>
		</div> 
	</div> 
</asp:Content>