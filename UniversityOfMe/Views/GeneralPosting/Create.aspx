<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<GeneralPosting>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	University Of Me - <%= Model.University.Id %> Create a General Posting
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="create"> 
			<div class="banner black full red-top small"> 
				<span class="organization">CREATE A GENERAL POSTING</span> 
			</div> 
            <% using (Html.BeginForm("Create", "GeneralPosting", FormMethod.Post)) {%>
			    <label for="Title">Title:</label> 
			    <%= Html.TextBox("Title", string.Empty, new { @class = "quarter" })%>
                <%= Html.ValidationMessage("Title", "*")%>

			    <label for="Body" class="mt13">Body:</label> 
                <%= Html.TextArea("Body", string.Empty, 6, 0, new { @class = "full" })%>
                <%= Html.ValidationMessage("Body", "*")%>

			    <div class="right"> 
				    <input type="submit" name="submit" class="btn teal mr14" value="Submit" /> 
				    <input type="button" name="cancel" class="btn teal" value="Cancel" /> 
			    </div> 
            <% } %>
		</div> 
	</div> 
</asp:Content>
