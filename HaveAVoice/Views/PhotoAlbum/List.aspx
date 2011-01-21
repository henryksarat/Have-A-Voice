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
			$("ul li").mouseenter(function() {
				$("a.edit", $(this)).show();
				$("a.delete", $(this)).show();
			}).mouseleave(function() {
				$("a.edit", $(this)).hide();
				$("a.delete", $(this)).hide();
			});
			
			$("a[rel=new-album]").click(function() {
				$(".popup").show();
				return false;
			});
		});
	</script>

	<% Html.RenderPartial("UserPanel", Model.NavigationModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>

	<div class="col-21">
	    <% UserInformationModel myUserInformationModel = HAVUserInformationFactory.GetUserInformation(); %>
	    <% bool myIsUser = myUserInformationModel.Details.Id == Model.SourceUserIdOfContent; %>
	
		<ul class="photo-album">
		    <% foreach (var item in Model.Models) { %>
		    	<li>
		    		<!-- Image needs to be wrapped in anchor tag Html.ActionLink will not work.  An alternative is needed. -->
		    		<img src="/Content/images/album.png" alt="<%= item.Name %>" /><br />
			        <%= Html.ActionLink(item.Name, "Details", "PhotoAlbum", new { id = item.Id }, new { @class = "album"}) %>
			        <% if (myIsUser) { %>
			            <%= Html.ActionLink("Edit", "Edit", "PhotoAlbum", new { id = item.Id }, new { @class = "edit" }) %>
			            <%= Html.ActionLink("Delete", "Delete", "PhotoAlbum", new { id = item.Id }, new { @class = "delete" }) %>
			        <% } %>
		        </li>
		    <% } %>
		    <li>
		    	<!-- jQuery is a simple show.  Will be expanded / styled for popup -->
		    	<a href="#" rel="new-album" class="create">Create Album</a>
		    </li>
	    </ul>
		<div class="clear">&bnsp;</div>
	
	    <% if (myIsUser) { %>
	    	<div class="popup">
		        <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.")%>
		
		        <% using (Html.BeginForm("Create", "PhotoAlbum")) {%>
		            <%= Html.Encode(ViewData["Message"])%><br />
		            <%= Html.Encode(TempData["Message"])%><br />
		            <p>
		                <label for="Name">Name:</label>
		                <%= Html.TextBox("Name")%>
		                <%= Html.ValidationMessage("Name", "*")%>
		            </p>
		            <p>
		                <label for="Description">Description:</label>
		                <%= Html.TextArea("Description")%>
		                <%= Html.ValidationMessage("Description", "*")%>
		            </p>
		            <p>
		                <input type="submit" value="Post" />
		            </p>
		        <% } %>
	        </div>
	    <% } %>
    	<div class="clear">&nbsp;</div>
    </div>
</asp:Content>