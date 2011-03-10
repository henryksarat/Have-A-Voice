<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.NotLoggedInModel>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Connecting you to the political world instantaneously
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("Message"); %>
    <% if (!HAVUserInformationFactory.IsLoggedIn()) { %>
        <% Html.RenderPartial("NotLoggedInUserPanel"); %>
    <% } %>
    <div class="clear">&nbsp;</div>
    
    <div class="col-24 m-top20 not-logged">
        <div class="push-1 col-10 center p-t5 p-b5 t-tab b-wht">
			<img src="/Content/images/like.png" alt="Thumbs Up" align="top" />
            <span class="fnt-16 tint-6 bold">MOST POPULAR</span>
            <div class="clear">&nbsp;</div>
        </div>
		<div class="push-3 col-10 center p-t5 p-b5 t-tab b-wht">
                <span class="fnt-16 tint-6 bold">NEWEST</span>
                <img src="/Content/images/like.png" alt="Newest" align="top" />	
			<div class="clear">&nbsp;</div>
		</div>
		<div class="clear"></div>

		<div class="col-24 b-wht">
			<div class="col-12" >
				<div class="p-a10">
					<div rel="match">
		                <%=  IssueHelper.BuildIssueDisplay(Model.MostPopular) %> 
		                <div class="clear">&nbsp;</div>
	                </div>
	                <div class="clear">&nbsp;</div>
                    <!--		            
                    <div class="right">
						<a href="/Authentication/Login" class="more-like">More Topics Liked By Members &gt;&gt;</a>
					</div>
                    -->
					<div class="clear">&nbsp;</div>
	            </div>
	            <div class="clear">&nbsp;</div>
	        </div>
	
	        <div class="col-12">
	            <div class="p-a10 b-wht">
	            	<div rel="match">
		                <%= IssueHelper.BuildIssueDisplay(Model.Newest) %> 
		                <div class="clear">&nbsp;</div>
	                </div>
	                <div class="clear">&nbsp;</div>
                    <!--
	                <div class="right">
					    <a href="/Authentication/Login" class="more-dislike">More Topics Disliked By Members &gt;&gt;</a>
				    </div>
                    -->
	                <div class="clear">&nbsp;</div>
	            </div>
	            <div class="clear">&nbsp;</div>
	        </div>
			<div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>
    </div>
    <div class="clear">&nbsp;</div>
</asp:Content>
