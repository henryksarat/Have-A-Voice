<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<ClubViewModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Organization
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
		<div class="create create-feature-form"> 
			<div class="banner black full small"> 
				<span class="organization">EDIT ORGANIZATION: <%= Model.Get().Title %></span> 
			</div> 

            <% Html.RenderPartial("Message"); %>
            <% Html.RenderPartial("Validation"); %>

            <div class="padding-col">
                <% using (Html.BeginForm("Edit", "Club", FormMethod.Post, new { enctype = "multipart/form-data", @class = "create btint-6" })) {%>
                    <%= Html.Hidden("ClubId", Model.Get().ClubId) %>
			    
                    <div class="field-holder">
                        <label for="Name">Name</label> 
			            <%= Html.TextBox("Name", Model.Get().Name)%>
                        <%= Html.ValidationMessage("Name", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
                        <label for="Name">Current Image</label> 
                        <img src="<%= Model.Get().ClubImageUrl %>" />
                    </div>

                    <div class="field-holder">
			            <label for="ClubImage">New Image</label> 
			            <input type="file" id="ClubImage" name="ClubImage" size="23" />
                    </div>

                    <div class="field-holder">
			            <label for="ClubType">Organization Type</label> 
                        <%= Html.DropDownListFor(model => model.Get().ClubType, Model.Get().ClubTypes)%>
                        <%= Html.ValidationMessageFor(model => model.Get().ClubType, "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
			            <label for="Description">Description</label> 
                        <%= Html.TextArea("Description", Model.Get().Description, new { @class = "textarea" }) %>
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

