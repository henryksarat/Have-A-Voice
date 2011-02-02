<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInListModel<Event>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Calendar
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.RenderPartial("UserPanel", Model.NavigationModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>
    
    <div class="col-21 events">
        <% Html.RenderPartial("Message"); %>

        <% UserInformationModel myUserInformationModel = HAVUserInformationFactory.GetUserInformation(); %>
        <% bool myIsUser = myUserInformationModel.Details.Id == Model.SourceUserIdOfContent; %>
        <% if (myIsUser) { %>
		    <div class="create">
			    <div class="col-21">
				    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
			    </div>
			    <div class="clear">&nbsp;</div>
			    <div class="col-3">
				    <div class="p-h5 fnt-14 c-white">
					    <b>Create Event</b>
				    </div>
			    </div>

			    <div class="col-18">
				    <% using (Html.BeginForm("AddEvent", "Calendar", FormMethod.Post, new { @class = "create" })) { %>
					    <div class="col-6">
						    <div class="col-2 m-rgt right c-white">
							    <label for="Date">Date:</label>
						    </div>
						    <div class="col-4 fnt-12">
							    <%= Html.TextBox("Date", DateTime.UtcNow.AddMinutes(1))%>
						    </div>
						    <div class="clear">&nbsp;</div>
						    <div class="col-6">
							    <%= Html.ValidationMessage("Date", "*")%>
						    </div>
					    </div>
					    <div class="col-9">
						    <div class="col-4 m-rgt right c-white">
							    <label for="Information">Information:</label>
						    </div>
						    <div class="col-4 fnt-12">
							    <%= Html.TextArea("Information", null, new { cols = "20", rows = "2", resize = "none" })%>
						    </div>
						    <div class="clear">&nbsp;</div>
						    <div class="col-8">
							    <%= Html.ValidationMessage("Information", "*")%>
						    </div>
					    </div>
					    <div class="col-3">
						    <input type="submit" value="Create" class="create" />
					    </div>
					    <div class="clear">&nbsp;</div>
				    <% } %>
			    </div>

			    <div class="clear">&nbsp;</div>
	        </div>
        <% } %>
	    <div class="clear">&nbsp;</div>
    	<div class="spacer-30">&nbsp;</div>
    	
    	<% int cnt = 0; %>
	    <% foreach (var item in Model.Models) { %>
	    	<div class="<% if(cnt % 2 == 0) { %>row<% } else { %>alt<% } %>">
				<div class="col-6">
					<div class="p-a5">
						<div class="date-tile">
							<span><%= item.Date.ToString("MMMM dd").ToUpper() %></span>
							<%= item.Date.ToString("yyyy") %>
						</div>
					</div>
				</div>
				<div class="col-13">
					<div class="p-a5">
						<%= PresentationHelper.ReplaceCarriageReturnWithBR(item.Information) %>
					</div>
				</div>
				<div class="col-2 center">
					<div class="p-a5">
						<%= Html.ActionLink("Delete", "DeleteEvent", new { id = item.Id }, new { @class = "delete" }) %>
					</div>
				</div>
				<div class="clear">&nbsp;</div>
			</div>
			<div class="clear">&nbsp;</div>
			<div class="spacer-10">&nbsp;</div>
			
			<% cnt++; %>
	    <% } %>
	</div>
</asp:Content>
