<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.NotLoggedInModel>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Connecting you to the political world instantaneously
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("NotLoggedInUserPanel"); %>
    <div class="clear">&nbsp;</div>
    
    <div class="col-24 m-top20 not-logged">
        <div class="push-1 col-10 center p-t5 p-b5 t-tab b-wht">
			<img src="/Content/images/like.png" alt="Thumbs Up" align="top" />
            <span class="fnt-16 tint-6 bold">MEMBERS AGREE</span>
            <div class="clear">&nbsp;</div>
        </div>
		<div class="push-3 col-10 center p-t5 p-b5 t-tab b-wht">
			<span class="fnt-16 color-5 bold">MEMBERS DISAGREE</span>
			<img src="/Content/images/dislike.png" alt="Thumbs Down" align="top" />
			<div class="clear">&nbsp;</div>
		</div>
		<div class="clear"></div>

		<div class="col-24 b-wht">
			<div class="col-12" >
				<div class="p-a10">
					<div rel="match">
		                <%= IssueHelper.BuildIssueDisplay(Model.LikedIssues, false) %> 
		                <div class="clear">&nbsp;</div>
	                </div>
	                <div class="clear">&nbsp;</div>
		            <div class="right">
						<a href="/Authentication/Login" class="more-like">More Topics Liked By Members &gt;&gt;</a>
					</div>
					<div class="clear">&nbsp;</div>
	            </div>
	            <div class="clear">&nbsp;</div>
	        </div>
	
	        <div class="col-12">
	            <div class="p-a10 b-wht">
	            	<div rel="match">
		                <%= IssueHelper.BuildIssueDisplay(Model.DislikedIssues, false) %> 
		                <div class="clear">&nbsp;</div>
	                </div>
	                <div class="clear">&nbsp;</div>
	                <div class="right">
					    <a href="/Authentication/Login" class="more-dislike">More Topics Disliked By Members &gt;&gt;</a>
				    </div>
	                <div class="clear">&nbsp;</div>
	            </div>
	            <div class="clear">&nbsp;</div>
	        </div>
			<div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>
    </div>
    <div class="clear">&nbsp;</div>

    <div class="col-24 m-btm20 sub-main p-v15">
        <div class="col-12">
            <div class="push-1 col-8">
                <h5 class="popular">Popular Replies to Issues</h5>
                <ul class="bullet fnt-12">
                    <%= IssueReplyHelper.IssueReplyDisplay(Model.MostPopularIssueReplys) %>
                </ul>
                <div class="clear">&nbsp;</div>
            </div>
            <div class="clear">&nbsp;</div>
        </div>

        <div class="col-12">
            <div class="push-1 col-8">
                <h5 class="recent-comment">Recent Replies to Issues</h5>
                <ul class="bullet fnt-12">
                    <%= IssueReplyHelper.IssueReplyDisplay(Model.NewestIssueReplys) %>
                </ul>
                <div class="clear">&nbsp;</div>
            </div>
            <div class="clear">&nbsp;</div>
        </div>
        <div class="clear">&nbsp;</div>
    </div>
</asp:Content>
