<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<Professor>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	University Of Me - <%= Model.University.Id %> Create Professor
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
		<div class="create-feature-form create"> 
			<div class="banner black full small"> 
				<span class="professor">CREATE PROFESSOR</span> 
			</div> 
            <% Html.RenderPartial("Message"); %>
            <% Html.RenderPartial("Validation"); %>
            <div class="padding-col">
                <% using (Html.BeginForm("Create", "Professor", FormMethod.Post)) {%>
                    <div class="field-holder">
			            <label for="FirstName">First Name</label> 
                        <%= Html.TextBox("FirstName")%>
                        <%= Html.ValidationMessage("FirstName", "*", new { @class = "req"})%>
                    </div>

                    <div class="field-holder">
			            <label for="LastName">Last Name</label> 
                        <%= Html.TextBox("LastName")%>
                        <%= Html.ValidationMessage("LastName", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
			            <label for="ProfessorImage">Professor Image</label> 
			            <input type="file" id="ProfessorImage" name="ProfessorImage" size="23" />
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
