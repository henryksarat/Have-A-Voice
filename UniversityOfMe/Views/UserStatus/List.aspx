<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<UserStatus>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	User Status's From Within Your University
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <div class="eight last"> 
        <div class="create-feature-form">
		    <div class="banner black full"> 
			    University Wide Statuses
		    </div> 
            <div class="padding-col">
                <% Html.RenderPartial("Message"); %>
                <% Html.RenderPartial("Validation"); %>


                <ul> 
                    <% foreach (UserStatus myUserStatus in Model.Get()) { %>
                            <div>
                    	        <div class="center" style="display: inline-block"> 
                                    <a href="<%= URLHelper.ProfileUrl(myUserStatus.User) %>">
                                        <img src="<%= PhotoHelper.ProfilePicture(myUserStatus.User) %>" class="profile lrg" />
                                    </a>
				                </div> 
                                <div style="display: inline-block; vertical-align: middle; height: 80px">
                                    <div style="vertical-align: middle">
                                        <a class="itemlinked" href="<%= URLHelper.ProfileUrl(myUserStatus.User) %>"><%= NameHelper.FullName(myUserStatus.User) %></a>
                                        <span class="black">is <%= myUserStatus.Status %> at <%= LocalDateHelper.ToLocalTime(myUserStatus.DateTimeStamp) %> </span>
                                    </div>
                                </div>
                            </div>
                    <% } %>
                </ul> 
            </div>
        </div>
	</div>
</asp:Content>
