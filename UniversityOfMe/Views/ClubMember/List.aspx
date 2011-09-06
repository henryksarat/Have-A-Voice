<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<ClubMember>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="Social.ViewHelpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Members in <%= Model.Get().FirstOrDefault<ClubMember>().Club.Name %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="banner black full red-top"> 
			<%= Model.Get().First<ClubMember>().Club.Name %> Members
		</div> 
		
        <ul id="list" class="friend-list clearfix"> 
            <% foreach (ClubMember myClubMember in Model.Get()) { %>
			    <li> 
                    <div> 
					    <a href="<%= URLHelper.ProfileUrl(myClubMember.ClubMemberUser) %>"><img src="<%= PhotoHelper.ProfilePicture(myClubMember.ClubMemberUser) %>" class="profile lrg flft mr9" /></a>
					    <span class="name"><%= NameHelper.FullName(myClubMember.ClubMemberUser)%></span>
					    <%= myClubMember.Title %><br /> 
					    <span class="red">&nbsp;</span> 
					    <a href="/Message/Create/<%= myClubMember.ClubMemberUser.Id %>" class="frgt mail">Mail</a> 
                    </div>
			    </li> 
            <% } %>
		</ul> 
	</div>
</asp:Content>

