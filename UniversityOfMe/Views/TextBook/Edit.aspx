<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<TextBookViewModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	University Of Me - <%= Model.University.Id %> Edit Textbook Entry
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="create"> 
			<div class="banner black full red-top small"> 
				<span class="book">EDIT TEXTBOOK - <%= Model.Get().BookTitle %></span> 
			</div> 
            <% using (Html.BeginForm("Edit", "TextBook", FormMethod.Post, FormMethod.Post, new { enctype = "multipart/form-data", @class = "create btint-6" })) {%>
                <%= Html.Hidden("TextBookId", Model.Get().TextBookId) %>
			    <label for="BookTitle">Book Title:</label> 
			    <%= Html.TextBox("BookTitle", Model.Get().BookTitle, new { @class = "quarter" })%>
                <%= Html.ValidationMessage("BookTitle", "*")%>

			    <label for="TextBookCondition" class="mt13">Book Condition:</label> 
                <%= Html.DropDownListFor(model => model.Get().TextBookCondition, Model.Get().TextBookConditions)%>
                <%= Html.ValidationMessageFor(model => model.Get().TextBookCondition, "*")%>

                <label for="Image" class="mt13">Current Textbook Image:</label> 
                <img src="<%= Model.Get().TextBookImageUrl %>" />

			    <label for="BookImage" class="mt13">Book Image:</label> 
			    <input type="file" id="BookImage" name="BookImage" size="23" />

			    <label for="BookTitle" class="mt13">Class Code:</label> 
			    <%= Html.TextBox("ClassCode", Model.Get().ClassCode, new { @class = "quarter" })%>
                <%= Html.ValidationMessage("ClassCode", "*")%>

			    <label for="BookTitle" class="mt13">Edition:</label> 
			    <%= Html.TextBox("Edition", Model.Get().Edition.Equals("0") ? string.Empty : Model.Get().Edition, new { @class = "quarter" })%>
                <%= Html.ValidationMessage("Edition", "*")%>

			    <label for="BuySell" class="mt13">Buy/Sell:</label> 
                <%= Html.DropDownListFor(model => model.Get().BuySell, Model.Get().BuySellOptions)%>
                <%= Html.ValidationMessageFor(model => model.Get().BuySell, "*")%>

			    <label for="Price" class="mt13">Price:</label> 
			    <%= Html.TextBox("Price", Model.Get().Price, new { @class = "quarter" })%>
                <%= Html.ValidationMessage("Price", "*")%>

			    <label for="Details" class="mt13">Details:</label> 
                <%= Html.TextArea("Information", Model.Get().Details, 6, 0 ,new { @class = "full" }) %>
                <%= Html.ValidationMessage("Details", "*")%>

			    <div class="right"> 
				    <input type="submit" name="submit" class="btn site mr14" value="Submit" /> 
				    <input type="button" name="cancel" class="btn site" value="Cancel" /> 
			    </div> 
            <% } %>
		</div> 
	</div> 
</asp:Content>
