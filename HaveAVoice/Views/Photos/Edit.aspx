<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.PhotosModel>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	User Pictures
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% /* just used as example code... not used on Site anymore!! */ %>
	<% /* Html.RenderPartial("UserPanel", Model.NavigationModel); */ %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>
    
    <div class="col-21">
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab b-wht">
    		<span class="fnt-16 tint-6 bold">IMAGES</span>
    	</div>
    	<div class="clear">&nbsp;</div>
    	<div class="b-wht">
    		<div class="clear">&nbsp;</div>
    		<% using(Html.BeginForm("Photos", "Edit", FormMethod.Post, new { @class = "create" })) { %>
    			<div class="p-a10">
	    			<div class="col-4 c-gray fnt-14 bold">
	    				Current Profile Picture:
	    			</div>
	    			<div class="col-3">
				        <%= Html.Hidden("ProfilePictureURL", Model.ProfilePictureURL) %>
				        <%= Html.Hidden("UserId", Model.UserId) %>
						<img src="<%= Model.ProfilePictureURL %>" alt="" class="profile" />
	    			</div>
	    			<div class="clear">&nbsp;</div>
	    			
	    			<%= Html.Encode(ViewData["Message"]) %>
    			</div>
    			<div class="clear">&nbsp;</div>
    			
    			<% int cnt = 1; %>
		        <% foreach(var item in Model.Photos) { %>
				<% if(item.ProfilePicture == true) { continue; } %>
					<div class="col-7">
		    			<div class="col-1">
		    				<div class="p-a10 center">
			    				<%= CheckBoxHelper.StandardCheckbox("SelectedProfilePictureId", item.Id.ToString(), Model.SelectedPhotos.Contains(item.Id)) %>
		    				</div>
		    			</div>
		    			<div class="col-3 center">
		    				<img src="/Photos/<%= item.ImageName %>" class="profile" />
		    			</div>
		    			<div class="col-3">
							<div class="p-a5">
								<div class="date-tile">
									<span><%= item.DateTimeStamp.ToString("MMM").ToUpper() %></span> <%= item.DateTimeStamp.ToString("dd") %>
								</div>
							</div>
		    			</div>
	    			</div>
	    			<% if(cnt % 3 == 0) { %>
	    				<div class="clear">&nbsp;</div>
	    				<div class="spacer-10">&nbsp;</div>
	    			<% } %>
	    			<% cnt++; %>
    			<% } %>
	    		<div class="clear">&nbsp;</div>
	    		<div class="spacer-30">&nbsp;</div>
	    		<div class="center">
	    			<input type="submit" value="Set to Profile Picture" class="create" />
					<input type="button" value="Delete" />
	    		</div>
	    		<div class="spacer-30">&nbsp;</div>
	    		<div class="clear">&nbsp;</div>
    		<% } %>
    	</div>
	</div>
</asp:Content>

