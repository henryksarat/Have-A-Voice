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
		<div class="create create-feature-form"> 
			<div class="banner black full small"> 
				<span class="event">CREATE EVENT</span> 
			</div> 

            <% Html.RenderPartial("Message"); %>
            <% Html.RenderPartial("Validation"); %>

            <div class="padding-col">
                <% using (Html.BeginForm("Create", "Event", FormMethod.Post)) {%>
			        <div class="field-holder">
                        <label for="Title">Title</label> 
			            <%= Html.TextBox("Title")%>
                        <%= Html.ValidationMessage("Title", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
			            <label for="EventPrivacyOption">Event Privacy Option</label> 
                        <%= Html.DropDownListFor(model => model.Get().EventPrivacyOption, Model.Get().EventPrivacyOptions)%>
                        <%= Html.ValidationMessageFor(model => model.Get().EventPrivacyOption, "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder-extra">
			            <label for="StartDate">Start Date</label> 
                        <%= Html.TextBox("StartDate", Model.Get().StartDate)%>
                        <%= Html.DropDownListFor(model => Model.Get().StartTime, Model.Get().StartTimes)%>
                        <%= Html.ValidationMessage("StartDate", "*", new { @class = "req" })%>
                    </div>
                    
                    <div class="field-holder-extra">
			            <label for="EndDate">End Date</label> 
			            <%= Html.TextBox("EndDate", Model.Get().EndDate)%>
                        <%= Html.DropDownListFor(model => Model.Get().EndTime, Model.Get().EndTimes)%>
                        <%= Html.ValidationMessage("EndDate", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder" style="vertical-align:top">
			            <label for="Information">Information</label> 
                        <%= Html.TextArea("Information", Model.Get().Information, 6, 0 ,new { @class = "textarea" }) %>
                        <%= Html.ValidationMessage("Information", "*", new { @class = "req" })%>
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

