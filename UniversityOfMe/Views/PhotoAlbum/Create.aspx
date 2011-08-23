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
				<span class="album">CREATE ALBUM</span> 
			</div> 

            <% Html.RenderPartial("Message"); %>
            <% Html.RenderPartial("Validation"); %>

            <div class="padding-col">
                <% using (Html.BeginForm("Create", "PhotoAlbum", FormMethod.Post)) {%>
                    <div class="field-holder">
			            <label for="name">Title</label> 
                        <%= Html.TextBox("Name", string.Empty) %>
                        <%= Html.ValidationMessage("Name", "*", new { @class = "req" })%>
                    </div>
                    <div class="field-holder">
			            <label for="description">Description</label> 
			            <%= Html.TextArea("Description", new { @class = "textarea" })%>
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