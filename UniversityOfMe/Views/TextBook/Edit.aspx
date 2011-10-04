<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<TextBookViewModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Textbook | Edit | <%= Model.Get().BookTitle %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
		<div class="create-feature-form">
			<div class="banner black full small"> 
				<span class="book">EDIT TEXTBOOK - <%= Model.Get().BookTitle %></span> 
			</div> 

            <% Html.RenderPartial("Message"); %>
            <% Html.RenderPartial("Validation"); %>
            <div class="padding-col">
                <% using (Html.BeginForm("Edit", "TextBook", FormMethod.Post, FormMethod.Post, new { enctype = "multipart/form-data", @class = "create btint-6" })) {%>
                    <%= Html.Hidden("TextBookId", Model.Get().TextBookId) %>
                    
                    <div class="field-holder">
			            <label for="BookTitle">Book Title</label> 
			            <%= Html.TextBox("BookTitle", Model.Get().BookTitle, new { @class = "quarter" })%>
                        <%= Html.ValidationMessage("BookTitle", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
			            <label for="TextBookCondition">Book Condition</label> 
                        <%= Html.DropDownListFor(model => model.Get().TextBookCondition, Model.Get().TextBookConditions)%>
                        <%= Html.ValidationMessageFor(model => model.Get().TextBookCondition, "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
                        <label for="Image">Current Image</label> 
                        <img src="<%= Model.Get().TextBookImageUrl %>" />
                    </div>

                    <div class="field-holder">
			            <label for="BookImage">New Book Image</label> 
			            <input type="file" id="BookImage" name="BookImage" size="23" />
                    </div>

                    <div class="field-holder">
			            <label for="BookTitle" class="mt13">Class Section</label> 
			            <%= Html.TextBox("ClassCode", Model.Get().ClassSubject, new { @class = "quarter" })%>
                        <%= Html.ValidationMessage("ClassSubject", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
			            <label for="BookTitle" class="mt13">Class Course</label> 
			            <%= Html.TextBox("ClassCode", Model.Get().ClassCourse, new { @class = "quarter" })%>
                        <%= Html.ValidationMessage("ClassCode", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
			            <label for="ClassCode">Book Authors</label> 
			            <%= Html.TextBox("BookAuthor", string.Empty, new { @class = "quarter" })%>
                        <%= Html.ValidationMessage("BookAuthor", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
			            <label for="ClassCode">ISBN</label> 
			            <%= Html.TextBox("ISBN", string.Empty, new { @class = "quarter" })%>
                    </div>

                    <div class="field-holder">
			            <label for="BookTitle">Edition</label> 
			            <%= Html.TextBox("Edition", Model.Get().Edition.Equals("0") ? string.Empty : Model.Get().Edition)%>
                        <%= Html.ValidationMessage("Edition", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
			            <label for="Price">Price</label> 
			            <%= Html.TextBox("Price", Model.Get().Price)%>
                        <%= Html.ValidationMessage("Price", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
			            <label for="Details">Details</label> 
                        <%= Html.TextArea("Details", Model.Get().Details, new { @class = "textarea" })%>
                        <%= Html.ValidationMessage("Details", "*", new { @class = "req" })%>
                    </div>

			        <div class="field-holder">
                        <div class="right">
				            <input type="submit" name="submit" class="btn site button-padding" value="Submit" /> 
                        </div>
			        </div> 
                <% } %>
            </div>
		</div> 
	</div> 
</asp:Content>
