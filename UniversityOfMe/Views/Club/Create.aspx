﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<ClubViewModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	University Of Me - <%= Model.University.Id %> Create an Organization
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
		<div class="create create-feature-form"> 
			<div class="banner black full red-top small"> 
				<span class="organization">CREATE ORGANIZATION</span> 
			</div> 
            
            <% Html.RenderPartial("Message"); %>
            <% Html.RenderPartial("Validation"); %>

            <div class="padding-col">
                <% using (Html.BeginForm("Create", "Club", FormMethod.Post, FormMethod.Post, new { enctype = "multipart/form-data", @class = "create btint-6" })) {%>
			        <div class="field-holder">
                        <label for="Name">Name:</label> 
			            <%= Html.TextBox("Name")%>
                        <%= Html.ValidationMessage("Name", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
			            <label for="Name" class="mt13">Your title:</label> 
			            <%= Html.TextBox("Title", Model.Get().Title, new { @class = "quarter" })%>
                        <%= Html.ValidationMessage("Title", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
			            <label for="ClubImage" class="mt13">Organization Image:</label> 
			            <input type="file" id="ClubImage" name="ClubImage" size="23" />
                    </div>

                    <div class="field-holder">
			            <label for="ClubType" class="mt13">Organization Type:</label> 
                        <%= Html.DropDownListFor(model => model.Get().ClubType, Model.Get().ClubTypes)%>
                        <%= Html.ValidationMessageFor(model => model.Get().ClubType, "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
			            <label for="Description" class="mt13">Description:</label> 
                        <%= Html.TextArea("Description", Model.Get().Description, 6, 0 ,new { @class = "textarea" }) %>
                        <%= Html.ValidationMessage("Description", "*", new { @class = "req" })%>
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

