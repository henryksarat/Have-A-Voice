<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<EventViewModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	University Of Me - <%= Model.University.Id %> Create Event
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript" language="javascript">
	    $(function () {
	        $('#StartDate').datepicker({
	            changeMonth: true,
	            changeYear: true,
	            dateFormat: "mm-dd-yy",
	            yearRange: '2011:2012'
	        });

	        $('#EndDate').datepicker({
	            changeMonth: true,
	            changeYear: true,
	            dateFormat: "mm-dd-yy",
	            yearRange: '2011:2012'
	        });
	    });
	</script>

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="create"> 
			<div class="banner black full red-top small"> 
				<span class="event">CREATE EVENT</span> 
			</div> 
            <% using (Html.BeginForm("Create", "Event", FormMethod.Post)) {%>
			    <label for="Title">Title:</label> 
			    <%= Html.TextBox("Title", Model.Get().Title, new { @class = "quarter" })%>
                <%= Html.ValidationMessage("Title", "*")%>

			    <label for="EventPrivacyOption" class="mt13">Event Privacy Option:</label> 
                <%= Html.DropDownListFor(model => model.Get().EventPrivacyOption, Model.Get().EventPrivacyOptions)%>
                <%= Html.ValidationMessageFor(model => model.Get().EventPrivacyOption, "*")%>

			    <label for="StartDate" class="mt13">Start Date:</label> 
                <%= Html.TextBox("StartDate", Model.Get().StartDate, new { @class = "quarter" })%>
                <%= Html.DropDownListFor(model => Model.Get().StartTime, Model.Get().StartTimes) %>
                <%= Html.ValidationMessage("StartDate", "*")%>

			    <label for="EndDate" class="mt13">End Date:</label> 
			    <%= Html.TextBox("EndDate", Model.Get().EndDate, new { @class = "quarter" })%>
                <%= Html.DropDownListFor(model => Model.Get().EndTime, Model.Get().EndTimes) %>
                <%= Html.ValidationMessage("EndDate", "*")%>

			    <label for="Information" class="mt13">Information:</label> 
                <%= Html.TextArea("Information", Model.Get().Information, 6, 0 ,new { @class = "full" }) %>
                <%= Html.ValidationMessage("Information", "*")%>

			    <div class="right"> 
				    <input type="submit" name="submit" class="btn site mr14" value="Submit" /> 
				    <input type="button" name="cancel" class="btn site" value="Cancel" /> 
			    </div> 
            <% } %>
		</div> 
	</div> 
</asp:Content>

