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
		<div class="create create-feature-form"> 
			<div class="banner black full red-top small"> 
				<span class="class">CREATE CLASS</span> 
			</div> 
            <% Html.RenderPartial("Message"); %>
            <% Html.RenderPartial("Validation"); %>
            <div class="padding-col">
                <% using (Html.BeginForm("Create", "Class", FormMethod.Post)) {%>
			        <div class="field-holder">
                        <label for="ClassCode">Class Code:</label> 
                        <%= Html.TextBox("ClassCode", string.Empty)%>
                        <%= Html.ValidationMessage("ClassCode", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
			            <label for="ClassTitle" class="mt13">Class Title:</label> 
                        <%= Html.TextBox("ClassTitle", string.Empty)%>
                        <%= Html.ValidationMessage("ClassTitle", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
                        <label for="AcademicTerm" class="mt13">Academic term</label>
                        <%= Html.DropDownListFor(model => model.Get().AcademicTermId, Model.Get().AcademicTerms)%>
                        <%= Html.ValidationMessageFor(model => model.Get().AcademicTermId, "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
                        <label for="Year" class="mt13">Year</label>
                        <%: Html.DropDownListFor(model => model.Get().Year, Model.Get().Years)%>
                        <%: Html.ValidationMessageFor(model => model.Get().Year, "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
			            <label for="Details">Details:</label> 
                        <%= Html.TextArea("Details", Model.Get().Details, 6, 0, new { @class = "textarea" })%>
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
</asp:Content>
