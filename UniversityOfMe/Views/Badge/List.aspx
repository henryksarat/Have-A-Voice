<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<Badge>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="Social.ViewHelpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= UOMConstants.TITLE %> = Your Friends
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="banner black full red-top"> 
			BADGES
			<div id="header" class="buttons"> 
				<div class="clearfix"></div> 
			</div> 
		</div> 
		
        <ul id="list" class="friend-list clearfix"> 
            <% foreach (Badge myBadge in Model.Get()) { %>
			    <li> 
				    <div> 
					    <%= myBadge.Name %>
				    </div> 
			    </li> 
            <% } %>
		</ul> 
	</div>
</asp:Content>

