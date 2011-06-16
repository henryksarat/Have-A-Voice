<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<CreateClubModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	University Of Me - <%= Model.University.Id %> Create an Organization
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>
		<div class="create"> 
			<div class="banner black full red-top small"> 
				<span class="organization">CREATE ORGANIZATION</span> 
			</div> 
            <% using (Html.BeginForm("Create", "Club", FormMethod.Post, FormMethod.Post, new { enctype = "multipart/form-data", @class = "create btint-6" })) {%>
			    <label for="Name">Name:</label> 
			    <%= Html.TextBox("Name", Model.Get().Name, new { @class = "quarter" })%>
                <%= Html.ValidationMessage("Name", "*")%>

			    <label for="Name" class="mt13">Your title:</label> 
			    <%= Html.TextBox("Title", Model.Get().Title, new { @class = "quarter" })%>
                <%= Html.ValidationMessage("Title", "*")%>

			    <label for="ClubImage" class="mt13">Organization Image:</label> 
			    <input type="file" id="ClubImage" name="ClubImage" size="23" />

			    <label for="ClubType" class="mt13">Organization Type:</label> 
                <%= Html.DropDownListFor(model => model.Get().ClubType, Model.Get().ClubTypes)%>
                <%= Html.ValidationMessageFor(model => model.Get().ClubType, "*")%>

			    <label for="Description" class="mt13">Description:</label> 
                <%= Html.TextArea("Description", Model.Get().Description, 6, 0 ,new { @class = "full" }) %>
                <%= Html.ValidationMessage("Description", "*")%>

			    <div class="right"> 
				    <input type="submit" name="submit" class="btn site mr14" value="Submit" /> 
				    <input type="button" name="cancel" class="btn site" value="Cancel" /> 
			    </div> 
            <% } %>
		</div> 
	</div> 
</asp:Content>

