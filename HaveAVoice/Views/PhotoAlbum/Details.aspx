<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInWrapperModel<PhotoAlbum>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Gallery
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.RenderPartial("UserPanel", Model.NavigationModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>
    
    <div class="col-21">
       <% Html.RenderPartial("Message"); %>
        
        <h1 class="fnt-24 tint-6"><%= Model.Model.Name %></h1>
		<div class="fnt-12 m-btm10">
			<%= Model.Model.Description %>
        </div>

        <% UserInformationModel myUserInformationModel = HAVUserInformationFactory.GetUserInformation(); %>
        <% if (myUserInformationModel.Details.Id == Model.Model.CreatedByUserId) { %>
		    <div class="filter">
			    <% using (Html.BeginForm("Create", "Photos", FormMethod.Post, new { enctype = "multipart/form-data", @class = "create btint-6" })) { %>
                    <%= Html.Hidden("AlbumId", Model.Model.Id) %>
				        <div class="col-4 push-6 center">
					        <input type="file" id="ImageUpload" name="ImageUpload" size="23" />
	                        <%= Html.ValidationMessage("ProfilePictureUpload", "*")%>
				        </div>
                
				    <div class="col-4 push-8 center">
					    <input type="submit" value="Upload" class="create" />
				    </div>
				    <div class="clear">&nbsp;</div>
			    <% } %>
			    <div class="clear">&nbsp;</div>
		    </div>
		    <div class="spacer-10">&nbsp;</div>
        <% }  %>
		<% int cnt = 0; %>
		<% string klass = "gallery"; %>
        <% foreach (var item in Model.Model.Photos) { %>
        	<% if (cnt % 2 == 0) {
        		klass = "gallery";
			} else {
				klass = "gallery alt";
        	} %>
			<div class="col-4 center <%= klass %>">
				<div class="p-a5">
					<div class="image">
						<a href="/Photos/Display/<%= item.Id %>" target="_blank">
							<img src="<%= PhotoHelper.ConstructUrl(item.ImageName) %>" alt="<%= item.ImageName %>" />
						</a>
					</div>
					<div class="p-a5">
						<a href="<%= PhotoHelper.ConstructUrl(item.ImageName) %>" target="_blank"><%= item.ImageName %></a>
					</div>
				</div>
			</div>
			<% cnt++; %>
        <% } %>
        <div class="clear">&nbsp;</div>
	</div>
</asp:Content>