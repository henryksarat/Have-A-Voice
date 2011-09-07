<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<SomethingListWithUser<Badge>>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="Social.ViewHelpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Badges
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="banner black full red-top"> 
			<%= NameHelper.FullName(Model.Get().TargetUser) %>'s BADGES
			<div id="header" class="buttons"> 
				<div class="clearfix"></div> 
			</div> 
		</div> 
		
        <ul id="list" class="badge-list clearfix"> 
            <% foreach (Badge myBadge in Model.Get().ListedItems) { %>
			    <li> 
				    <div> 
                        <img src="/Content/images/badges/<%= myBadge.Image %>" />
                        <%= myBadge.Description %>
				    </div> 
			    </li> 
            <% } %>
		</ul> 
	</div>
</asp:Content>

