﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<TextBookViewModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create a Textbook Ad
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <div class="create-feature-form">
		    <div class="create"> 
			    <div class="banner black full small"> 
				<span class="book">CREATE TEXTBOOK</span> 
			</div> 

            <% Html.RenderPartial("Validation"); %>

                <div class="padding-col">
                    <% using (Html.BeginForm("Create", "TextBook", FormMethod.Post, FormMethod.Post, new { enctype = "multipart/form-data", @class = "create btint-6" })) {%>

                        <% if(string.IsNullOrEmpty(Model.Get().ClassSubject) || string.IsNullOrEmpty(Model.Get().ClassCourse)) { %>
                            <div class="field-holder">
                                <span class="empty-label">&nbsp;</span>
                                <a class="itemlinked" href="<%= URLHelper.SearchAllClasses() %>">Click here to select a class to tag your textbook to</a>
                            </div>
                        <% } %>

                        <div class="field-holder">
			                <label for="BookTitle">List in</label> 
			                <%= Model.University.UniversityName %>
                        </div>

                        <div class="field-holder">
			                <label for="BookTitle">Book Title</label> 
			                <%= Html.TextBox("BookTitle", string.Empty, new { @class = "quarter" })%>
                            <%= Html.ValidationMessage("BookTitle", "*", new { @class = "req" })%>
                        </div>

                        <div class="field-holder">
			                <label for="TextBookCondition">Book Condition</label> 
                            <%= Html.DropDownListFor(model => model.Get().TextBookCondition, Model.Get().TextBookConditions)%>
                            <%= Html.ValidationMessageFor(model => model.Get().TextBookCondition, "*", new { @class = "req" })%>
                        </div>

                        <div class="field-holder">
			                <label for="BookImage">Book Image</label> 
			                <input type="file" id="BookImage" name="BookImage" size="23" />
                            <%= Html.ValidationMessage("BookImage", "*", new { @class = "req" })%>
                        </div>

                        <div class="field-holder">
			                <label for="ClassSubject">Class Subject</label> 
			                <%= Html.TextBox("ClassSubject", string.Empty, new { @class = "quarter" })%>
                            <%= Html.ValidationMessage("ClassSubject", "*", new { @class = "req" })%>
                        </div>

                        <div class="field-holder">
			                <label for="ClassCourse">Class Course</label> 
			                <%= Html.TextBox("ClassCourse", string.Empty, new { @class = "quarter" })%>
                            <%= Html.ValidationMessage("ClassCourse", "*", new { @class = "req" })%>
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
			                <%= Html.TextBox("Edition", Model.Get().Edition, new { @class = "quarter" })%>
                            <%= Html.ValidationMessage("Edition", "*", new { @class = "req" })%>
                        </div>
                        <div class="field-holder">
			                <label for="Price">Price</label> 
			                <%= Html.TextBox("Price", string.Empty, new { @class = "quarter" })%>
                            <%= Html.ValidationMessage("Price", "*", new { @class = "req" })%>
                        </div>
                        <div class="field-holder">
			                <label for="Details">Details</label> 
                            <%= Html.TextArea("Details", string.Empty, 6, 0, new { @class = "textarea" })%>
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
	</div> 
</asp:Content>











