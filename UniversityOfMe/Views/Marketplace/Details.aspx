﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<MarketplaceItem>>" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Format" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="Social.Generic.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Buy <%= Model.Get().ItemTypeId %> | <%= Model.Get().Title %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaDescriptionHolder" runat="server">
	<%= UniversityOfMe.Helpers.MetaHelper.MetaDescription("Buy the " + Model.Get().ItemTypeId + ": " + Model.Get().Title) %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MetaKeywordsHolder" runat="server">
	<%= UniversityOfMe.Helpers.MetaHelper.MetaKeywords(Model.Get().ItemType + ", " + Model.Get().Title) %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last">
        <% Html.RenderPartial("Message"); %>

		<div class="banner black full red-top small">
			<span class="cart"><%= Model.Get().Title %></span>
			<div class="buttons">
                <% UserInformationModel<User> myUserInfo = UserInformationFactory.GetUserInformation();  %>
                <% if (myUserInfo != null && myUserInfo.Details.Id == Model.Get().UserId) { %>
				    <%= Html.ActionLink("Edit", "Edit", "Marketplace", new { id = Model.Get().Id }, new { @class = "edit mr5" })%>
                    <%= Html.ActionLink("Mark as sold", "MarkAsNonActive", "Marketplace", new { id = Model.Get().Id }, new { @class = "check sm mr5" })%>
                    <%= Html.ActionLink("Delete", "Delete", "Marketplace", new { id = Model.Get().Id }, new { @class = "trash-delete" })%>
                <% } %>
			</div>
		</div>

        <% Html.RenderPartial("Validation"); %>
					
		<div class="flft max-w207 mr21 center clearfix">
			<img src="<%= PhotoHelper.ItemSellingPhoto(Model.Get()) %>" alt="Textbook Cover" />
		</div>
		<div class="flft max-w590 wp69 clearfix">
					
			<div class="listing mb40">
				<div class="col">
					<label for="title">University</label>
				</div>
				<div class="col">
					<%= Model.Get().UniversityId %>
				</div>

        		<div class="clearfix"></div>

				<div class="col">
					<label for="author">Catagory</label>
				</div>
				<div class="col">
					<%= Model.Get().ItemTypeId %>
				</div>

				<div class="col">
					<label for="title">Title</label>
				</div>
				<div class="col">
					<%= Model.Get().Title%>
				</div>
							
				<div class="clearfix"></div>

                <div class="col">
					<label for="title">Listing Expires</label>
				</div>
				<div class="col">
					<%= String.Format("{0:MM/dd/yyyy}", DateTime.Parse(LocalDateHelper.ToLocalTime(Model.Get().ExpireListing))) %>
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
					<%= string.IsNullOrEmpty(Model.Get().Description) ? "None" : Model.Get().Description%>
				</div>
							
				<div class="clearfix"></div>
				<div class="col">
                    <% using (Html.BeginForm("CreateByButtonClick", "Message", new { id = Model.Get().UserId, subject = "Regarding " + Model.Get().ItemTypeId + ": " + Model.Get().Title })) {%>
					    <input type="submit" class="ml200 mt34 btn mail" value="Contact The Poster" />
                    <% } %>
				</div>
			</div>

		</div>					
	</div>
</asp:Content>

