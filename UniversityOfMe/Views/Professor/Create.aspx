<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<Professor>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	University Of Me - <%= Model.University.Id %> Create Professor
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="create"> 
			<div class="banner black full red-top small"> 
				<span class="professor">CREATE PROFESSOR</span> 
			</div> 
            <% using (Html.BeginForm("Create", "Professor", FormMethod.Post)) {%>
			    <label for="FirstName">First Name:</label> 
			    <input type="text" class="quarter" name="FirstName" id="FirstName" /> 
                <%= Html.ValidationMessage("FirstName", "*")%>

			    <label for="LastName" class="mt13">Last Name:</label> 
			    <input type="text" class="quarter" name="LastName" id="LastName" /> 
                <%= Html.ValidationMessage("LastName", "*")%>

			    <label for="ProfessorImage" class="mt13">Professor Image:</label> 
			    <input type="file" id="ProfessorImage" name="ProfessorImage" size="23" />

			    <div class="right"> 
				    <input type="submit" name="submit" class="btn teal mr14" value="Submit" /> 
				    <input type="button" name="cancel" class="btn teal" value="Cancel" /> 
			    </div> 
            <% } %>
		</div> 
	</div> 
</asp:Content>
