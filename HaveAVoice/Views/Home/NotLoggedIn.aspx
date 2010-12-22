<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.NotLoggedInModel>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Login
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <%= Html.Encode(ViewData["Message"]) %>
    </p>
    <%/* Html.RenderPartial("Tabs"); */%>
    <% Html.RenderPartial("UserPanel"); %>
    <div class="col-24 m-top20 not-logged">
        <div class="col-1">&nbsp;</div>
        <div class="col-10 center p-t5 p-b5 t-tab">
			<img src="/Content/images/like.png" alt="Thumbs Up" align="top" />
            <span class="fnt-16 tint-6 bold">MEMBERS LIKE</span>
        </div>
        <div class="col-2">&nbsp;</div>
		<div class="col-10 center p-t5 p-b5 t-tab">
			<span class="fnt-16 color-5 bold">MEMBERS DISLIKE</span>
			<img src="/Content/images/dislike.png" alt="Thumbs Down" align="top" />
		</div>
        <div class="col-1">&nbsp;</div>
		<div class="clear"></div>

		<div class="col-12">
			<div rel="match" class="p-a10 b-wht">
                <% foreach (var item in Model.LikedIssues) { %>
                    <%= IssueHelper.BuildIssueDisplay(item.Issue, true) %> 
                <% } %>
                <div class="right">
					<a href="#" class="more-like">More Topics Liked By Members &gt;&gt;</a>
				</div>
				<div class="clear">&nbsp;</div>
            </div>
        </div>

        <div class="col-12">
            <div rel="match" class="p-a10 b-wht">
                <% foreach (var item in Model.DislikedIssues) { %>
                    <%= IssueHelper.BuildIssueDisplay(item.Issue, false) %> 
                <% } %>
                <div class="right">
				    <a href="#" class="more-dislike">More Topics Disliked By Members &gt;&gt;</a>
			    </div>
                <div class="clear">&nbsp;</div>
            </div>
        </div>

        <!--AD SPACE-->
        <div class="col-24 spacer-30">&nbsp;</div>

		<div class="col-24 m-btm25 fnt-10">
			<div class="iab-leaderboard">
				<img src="http://www.iab.net/media/image/728X90.gif" alt="iab Advertisement" />
				<a class="advertise">Advertise with us</a>
			</div>
			<div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>
        <!--END AD SPACE-->

    </div>
    <div class="clear">&nbsp;</div>

    <div class="col-24 m-btm20 sub-main p-v15">
        <div class="col-12">
            <div class="push-1 col-8">
                <h5 class="popular">Popular Posts</h5>
                <ul class="bullet fnt-12">
                    <% foreach (var item in Model.MostPopularIssueReplys) { %>
                        <li><%= IssueReplyHelper.IssueReplyDisplay(item) %></li>
                    <% } %>
                </ul>
            </div>
        </div>

        <div class="col-12">
            <div class="push-1 col-8">
                <h5 class="comment">Recent Comments</h5>
                <ul class="bullet fnt-12">
                    <% foreach (var item in Model.NewestIssueReplys) { %>
                        <li><%= IssueReplyHelper.IssueReplyDisplay(item) %></li>
                    <% } %>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
