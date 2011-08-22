<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<GeneralPosting>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	University Of Me - <%= Model.University.Id %> Create a General Posting
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
		<div class="create create-feature-form"> 
			<div class="banner black full red-top small"> 
				<span class="organization">CREATE A GENERAL POSTING</span> 
			</div> 

            <% Html.RenderPartial("Message"); %>
            <% Html.RenderPartial("Validation"); %>

            <div class="padding-col">
                <% using (Html.BeginForm("Create", "GeneralPosting", FormMethod.Post)) {%>
                    <div class="field-holder">
			            <label for="Title">Title:</label> 
			            <%= Html.TextBox("Title")%>
                        <%= Html.ValidationMessage("Title", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
			            <label for="Body">Body:</label> 
                        <%= Html.TextArea("Body", new { @class = "textarea" })%>
                        <%= Html.ValidationMessage("Body", "*", new { @class = "req" })%>
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
