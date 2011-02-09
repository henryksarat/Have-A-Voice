<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInListModel<PhotoAlbum>>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Albums
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript" language="javascript">
		$(function() {
			var popupStatus = 0;
			
			function loadPopup() {
				if (popupStatus == 0) {
					$(".background-popup").css({ "opacity": "0.0" });
					$(".background-popup").fadeIn("slow");
					$(".popup").fadeIn("slow");
					popupStatus = 1;
				}
			}
			function disablePopup() {
				if (popupStatus == 1) {
					$(".background-popup").fadeOut("slow");
					$(".popup").fadeOut("slow");
					popupStatus = 0;
				}
			}
			function centerPopup() {
				var winWidth = document.documentElement.clientWidth;
				var winHeight = document.documentElement.clientHeight;
				var popWidth = $(".popup").width();
				var popHeight = $(".popup").height();
				$(".popup").css({ "position": "absolute", "top": winHeight / 2 - popHeight / 2, "left": winWidth / 2 - popWidth / 2});
				//$(".background-popup").css({ "height": winHeight });
			}
			
			$("ul li").mouseenter(function() {
				$("a.edit", $(this)).show();
				$("a.delete", $(this)).show();
			}).mouseleave(function() {
				$("a.edit", $(this)).hide();
				$("a.delete", $(this)).hide();
			});
			
			$("a[rel=new-album]").click(function() {
				centerPopup();
				loadPopup();
				return false;
			});
			$(".background-popup").click(function() {
				disablePopup();
			});
			$("#Cancel").click(function() {
				$("#Name").val("");
				$("#Description").val("");
				disablePopup();
			});
		});
	</script>

	<% Html.RenderPartial("UserPanel", Model.NavigationModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation", Model.NavigationModel); %>
        <div class="clear">&nbsp;</div>
    </div>

	<div class="col-21">
	    <% UserInformationModel myUserInformationModel = HAVUserInformationFactory.GetUserInformation(); %>
	    <% bool myIsUser = myUserInformationModel.Details.Id == Model.SourceUserIdOfContent; %>
	
        <% Html.RenderPartial("Message"); %>

		<ul class="photo-album">
		    <% foreach (var item in Model.Models) { %>
		    	<li>
		    		<a href="/PhotoAlbum/Details/<%= item.Id %>" class="album">
			    		<img src="<%= PhotoHelper.RetrievePhotoAlbumCoverUrl(item) %>" alt="<%= item.Name %>" /><br />
			    		<%= item.Name %>
			    	</a>
			        <% if (myIsUser) { %>
			            <%= Html.ActionLink("Edit", "Edit", "PhotoAlbum", new { id = item.Id }, new { @class = "edit" }) %>
			            <%= Html.ActionLink("Delete", "Delete", "PhotoAlbum", new { id = item.Id }, new { @class = "delete" }) %>
			        <% } %>
		        </li>
		    <% } %>
            <% if (myIsUser) { %>
		    <li>
		    	<a href="#" rel="new-album" class="create">Create Album</a>
		    </li>
            <% } %>
	    </ul>
		<div class="clear">&bnsp;</div>
    </div>
    <% if (myIsUser) { %>
    	<div class="background-popup"></div>
    	<div class="popup">
	        <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
	
	        <% using (Html.BeginForm("Create", "PhotoAlbum", FormMethod.Post, new { @class = "create" })) { %>
				<div class="col-3 right m-rgt">
					<label for="Name">Name:</label>
					<div class="clear">&nbsp;</div>
				</div>
				<div class="col-6">
					<%= Html.TextBox("Name") %>
					<span class="req">
						<%= Html.ValidationMessage("Name", "*") %>
					</span>
					<div class="clear">&nbsp;</div>
				</div>

				<div class="clear">&nbsp;</div>
				<div class="spacer-10">&nbsp;</div>
				<div class="clear">&nbsp;</div>

				<div class="col-3 right m-rgt">
					<label for="Description">Description:</label>
					<div class="clear">&nbsp;</div>
				</div>
				<div class="col-6">
					<%= Html.TextArea("Description", null, new { cols = "31", rows = "4", resize = "none"}) %>
					<span class="req">
						<%= Html.ValidationMessage("Description", "*") %>
					</span>
					<div class="clear">&nbsp;</div>
				</div>

				<div class="clear">&nbsp;</div>
				<div class="spacer-10">&nbsp;</div>
				<div class="clear">&nbsp;</div>

				<div class="right">
					<input type="submit" value="Create" class="create" />
					<input type="button" value="Cancel" class="btn" id="Cancel" />
				</div>
	        <% } %>
        </div>
    <% } %>
</asp:Content>