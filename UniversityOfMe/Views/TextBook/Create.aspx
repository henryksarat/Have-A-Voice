<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<TextBookViewModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	University Of Me - <%= Model.University.Id %> Create Textbook
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
			                <label for="BookImage">Book Image</label> 
			                <input type="file" id="BookImage" name="BookImage" size="23" />
                            <%= Html.ValidationMessage("ClassCode", "*", new { @class = "req" })%>
                        </div>

                        <div class="field-holder">
			                <label for="ClassCode">Class Code</label> 
			                <%= Html.TextBox("ClassCode", Model.Get().ClassCode, new { @class = "quarter" })%>
                            <%= Html.ValidationMessage("ClassCode", "*", new { @class = "req" })%>
                        </div>
                        <div class="field-holder">
			                <label for="BookTitle">Edition</label> 
			                <%= Html.TextBox("Edition", Model.Get().Edition, new { @class = "quarter" })%>
                            <%= Html.ValidationMessage("Edition", "*", new { @class = "req" })%>
                        </div>
                        <div class="field-holder">
			                <label for="Price">Price</label> 
			                <%= Html.TextBox("Price", Model.Get().Price, new { @class = "quarter" })%>
                            <%= Html.ValidationMessage("Price", "*", new { @class = "req" })%>
                        </div>
                        <div class="field-holder">
			                <label for="Details">Details</label> 
                            <%= Html.TextArea("Information", Model.Get().Details, 6, 0 ,new { @class = "textarea" }) %>
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











