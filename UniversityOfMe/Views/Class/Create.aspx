<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<CreateClassModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= UOMConstants.TITLE %> - Create Class
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>
		<div class="create"> 
			<div class="banner black full red-top small"> 
				<span class="class">CREATE CLASS</span> 
			</div> 
            <% using (Html.BeginForm("Create", "Class", FormMethod.Post)) {%>
			    <label for="ClassCode">Class Code:</label> 
			    <input type="text" class="quarter" name="ClassCode" id="ClassCode" /> 
                <%= Html.ValidationMessage("ClassCode", "*")%>

			    <label for="ClassTitle" class="mt13">Class Title:</label> 
			    <input type="text" class="half" name="ClassTitle" id="ClassTitle" /> 
                <%= Html.ValidationMessage("ClassTitle", "*")%>

                <label for="AcademicTerm" class="mt13">Academic term</label>
                <%: Html.DropDownListFor(model => model.Get().AcademicTermId, Model.Get().AcademicTerms)%>
                <%: Html.ValidationMessageFor(model => model.Get().AcademicTermId, "*")%>

                <label for="Year" class="mt13">Year</label>
                <%: Html.DropDownListFor(model => model.Get().Year, Model.Get().Years)%>
                <%: Html.ValidationMessageFor(model => model.Get().Year, "*")%>

			    <label for="Details" class="mt13">Details:</label> 
			    <textarea name="desc" id="Details" class="full" rows="6"></textarea> 
                <%= Html.ValidationMessage("Details", "*")%>
			    <div class="right"> 
				    <input type="submit" name="submit" class="btn site mr14" value="Submit" /> 
				    <input type="button" name="cancel" class="btn site" value="Cancel" /> 
			    </div> 
            <% } %>
		</div> 
	</div> 
</asp:Content>
