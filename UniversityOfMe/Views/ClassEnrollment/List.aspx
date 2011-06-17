<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<ClassEnrollment>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="Social.ViewHelpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Inbox
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="banner black full red-top"> 
			<%= Model.Get().First<ClassEnrollment>().Class.ClassCode %> Members
		</div> 
		
        <ul id="list" class="friend-list clearfix"> 
            <% foreach (ClassEnrollment myClassEnrollment in Model.Get()) { %>
			    <li> 
                    <div> 
					    <a href="<%= URLHelper.ProfileUrl(myClassEnrollment.User) %>"><img src="<%= PhotoHelper.ProfilePicture(myClassEnrollment.User) %>" class="profile lrg flft mr9" /></a>
					    <span class="name"><%= NameHelper.FullName(myClassEnrollment.User)%></span>
					    <%= UniversityHelper.GetMainUniversity(myClassEnrollment.User).UniversityName %><br /> 
					    <span class="red">&nbsp;</span> 
					    <a href="/Message/Create/<%= myClassEnrollment.UserId %>" class="frgt mail">Mail</a> 
                    </div>
			    </li> 
            <% } %>
		</ul> 
	</div>
</asp:Content>

