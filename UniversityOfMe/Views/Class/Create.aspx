<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<CreateClassModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Class
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
		<div class="create create-feature-form"> 
			<div class="banner black full small"> 
				<span class="class">CREATE CLASS</span> 
			</div> 
            <% Html.RenderPartial("Message"); %>
            <% Html.RenderPartial("Validation"); %>
            <div class="padding-col">
                <% using (Html.BeginForm("Create", "Class", FormMethod.Post)) {%>
                    <div class="field-holder">
			            <label for="BookTitle">Create Class In</label> 
			            <%= Model.University.UniversityName %>
                    </div>

			        <div class="field-holder">
                        <label for="ClassSubject">Class Subject</label> 
                        <%= Html.TextBox("ClassSubject", string.Empty)%>
                        <%= Html.ValidationMessage("ClassSubject", "*", new { @class = "req" })%>
                    </div>
        	        <div class="field-holder">
                        <label for="ClassCode">Example:</label> 
                        CIS
                    </div>

        	        <div class="field-holder">
                        <label for="ClassCourse">Class Course Number</label> 
                        <%= Html.TextBox("ClassCourse", string.Empty)%>
                        <%= Html.ValidationMessage("ClassCourse", "*", new { @class = "req" })%>
                    </div>

          	        <div class="field-holder">
                        <label for="ClassCode">Example:</label> 
                        120
                    </div>

                    <div class="field-holder">
			            <label for="ClassTitle">Class Title</label> 
                        <%= Html.TextBox("ClassTitle", string.Empty)%>
                        <%= Html.ValidationMessage("ClassTitle", "*", new { @class = "req" })%>
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
