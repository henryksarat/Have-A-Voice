﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<TextBook>>" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Format" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="Social.Generic.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Buy Textbook | <%= Model.Get().BookTitle %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaDescriptionHolder" runat="server">
	<%= UniversityOfMe.Helpers.MetaHelper.MetaDescription("Buy the textbook " + Model.Get().BookTitle + " by " + Model.Get().BookAuthor) %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MetaKeywordsHolder" runat="server">
	<%= UniversityOfMe.Helpers.MetaHelper.MetaKeywords(Model.Get().BookTitle + ", " + Model.Get().BookAuthor + ", " + Model.Get().ISBN) %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last">
        <% Html.RenderPartial("Message"); %>

		<div class="banner black full red-top small">
			<span class="book"><%= Model.Get().BookTitle %></span>
			<div class="buttons">
                <% UserInformationModel<User> myUserInfo = UserInformationFactory.GetUserInformation();  %>
                <% if (myUserInfo != null && myUserInfo.Details.Id == Model.Get().UserId) { %>
				    <%= Html.ActionLink("Edit", "Edit", "TextBook", new { id = Model.Get().Id }, new { @class = "edit mr5" })%>
                    <%= Html.ActionLink("Mark as sold", "MarkAsNonActive", "TextBook", new { id = Model.Get().Id }, new { @class = "check sm mr5" })%>
                    <%= Html.ActionLink("Delete", "Delete", "TextBook", new { id = Model.Get().Id }, new { @class = "trash-delete" })%>
                <% } %>
			</div>
		</div>

        <div style="margin-bottom:10px; vertical-align: top">
            <div style="display:inline-block; vertical-align: top">
                <a href="https://twitter.com/share" class="twitter-share-button" data-count="none" data-via="uofme">Tweet</a><script type="text/javascript" src="//platform.twitter.com/widgets.js"></script>
            </div>
            <div style="display:inline-block; vertical-align: top">
                <div id="fb-root"></div>
                <script>    (function (d, s, id) {
                        var js, fjs = d.getElementsByTagName(s)[0];
                        if (d.getElementById(id)) { return; }
                        js = d.createElement(s); js.id = id;
                        js.src = "//connect.facebook.net/en_US/all.js#appId=236494866400848&xfbml=1";
                        fjs.parentNode.insertBefore(js, fjs);
                    } (document, 'script', 'facebook-jssdk'));</script>

                <div class="fb-like" data-send="true" data-layout="button_count" data-width="50" data-show-faces="false"></div>
            </div>
        </div>
					
		<div class="flft max-w207 mr21 center clearfix">
			<img src="<%= PhotoHelper.TextBookPhoto(Model.Get()) %>" alt="Textbook Cover" />
		</div>
		<div class="flft max-w590 wp69 clearfix">
					
			<div class="listing mb40">
				<div class="col">
					<label for="title">University</label>
				</div>
				<div class="col">
					<%= Model.Get().University.UniversityName %>
				</div>

				<div class="col">
					<label for="title">Title</label>
				</div>
				<div class="col">
					<%= Model.Get().BookTitle%>
				</div>
	
				<div class="clearfix"></div>

				<div class="col">
					<label for="author">Author</label>
				</div>
				<div class="col">
					<%= Model.Get().BookAuthor%>
				</div>
	
				<div class="clearfix"></div>

                <div class="col">
					<label for="author">ISBN</label>
				</div>
				<div class="col">
					<%= string.IsNullOrEmpty(Model.Get().ISBN) ? "N/A" : Model.Get().ISBN %>
				</div>
	
				<div class="clearfix"></div>
							
				<div class="col">
					<label for="condition">Condition</label>
				</div>
				<div class="col">
					<%= Model.Get().TextBookCondition.Display %>
				</div>
							
				<div class="clearfix"></div>
							
				<div class="col">
					<label for="class">Class Used For</label>
				</div>
				<div class="col">
					<%= string.IsNullOrEmpty(Model.Get().ClassCourse) ? "N/A" : Model.Get().ClassSubject + Model.Get().ClassCourse%>
				</div>
							
				<div class="clearfix"></div>
							
				<div class="col">
					<label for="edition">Edition</label>
				</div>
				<div class="col">
					<%= Model.Get().Edition == 0 ? "NA" : Model.Get().Edition.ToString() %>
				</div>
							
				<div class="clearfix"></div>
							
				<div class="col">
					<label for="price">Asking Price</label>
				</div>
				<div class="col">
					<%= MoneyFormatHelper.Format(Model.Get().Price) %>
				</div>
							
				<div class="clearfix"></div>
							
				<div class="col">
					<label for="detail">Details</label>
				</div>
				<div class="col">
					<%= string.IsNullOrEmpty(Model.Get().Details) ? "None" : Model.Get().Details %>
				</div>
							
				<div class="clearfix"></div>
				<div class="col">
                    <% using (Html.BeginForm("CreateByButtonClick", "Message", new { id = Model.Get().UserId, subject = "Regarding book: " + Model.Get().BookTitle })) {%>
					    <input type="submit" class="ml200 mt34 btn mail" value="Contact The Poster" />
                    <% } %>
				</div>
			</div>

		</div>					
	</div>
</asp:Content>

