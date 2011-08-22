<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<TextBook>>" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Format" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= UOMConstants.TITLE %> - <%= Model.Get().BookTitle %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last">
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="banner black full red-top small">
			<span class="book"><%= Model.Get().BookTitle %></span>
			<div class="buttons">
                <% if (UserInformationFactory.GetUserInformation().Details.Id == Model.Get().UserId) { %>
				    <%= Html.ActionLink("Edit", "Edit", "TextBook", new { id = Model.Get().Id }, new { @class = "edit mr5" })%>
                    <%= Html.ActionLink("Mark as sold", "MarkAsNonActive", "TextBook", new { id = Model.Get().Id }, new { @class = "check sm mr5" })%>
                    <%= Html.ActionLink("Delete", "Delete", "TextBook", new { id = Model.Get().Id }, new { @class = "trash-delete" })%>
                <% } %>
			</div>
		</div>
					
		<div class="flft max-w207 mr21 center clearfix">
			<img src="<%= PhotoHelper.TextBookPhoto(Model.Get()) %>" alt="Textbook Cover" />
		</div>
		<div class="flft max-w590 wp69 clearfix">
					
			<div class="listing mb40">
				<div class="col">
					<label for="title">Title:</label>
				</div>
				<div class="col">
					<%= Model.Get().BookTitle%>
				</div>
	
				<div class="clearfix"></div>
							
				<div class="col">
					<label for="condition">Condition:</label>
				</div>
				<div class="col">
					<%= Model.Get().TextBookCondition.Display %>
				</div>
							
				<div class="clearfix"></div>
							
				<div class="col">
					<label for="class">Class Used For:</label>
				</div>
				<div class="col">
					<%= Model.Get().ClassCode %>
				</div>
							
				<div class="clearfix"></div>
							
				<div class="col">
					<label for="edition">Edition:</label>
				</div>
				<div class="col">
					<%= Model.Get().Edition == 0 ? "NA" : Model.Get().Edition.ToString() %>
				</div>
							
				<div class="clearfix"></div>
							
				<div class="col">
					<label for="price">Asking Price:</label>
				</div>
				<div class="col">
					<%= MoneyFormatHelper.Format(Model.Get().Price) %>
				</div>
							
				<div class="clearfix"></div>
							
				<div class="col">
					<label for="detail">Details:</label>
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

