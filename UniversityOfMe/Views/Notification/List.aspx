<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<NotificationModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= UOMConstants.TITLE %> - Your Notifications
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <div class="eight last"> 
        <div class="create-feature-form">
		    <div class="banner black full"> 
			    Your Notifications
		    </div> 
            <div class="padding-col">
                <% Html.RenderPartial("Message"); %>
                <% Html.RenderPartial("Validation"); %>


                <% Html.RenderPartial("Notifications", Model.Get()); %>
            </div>
        </div>
	</div>
</asp:Content>
