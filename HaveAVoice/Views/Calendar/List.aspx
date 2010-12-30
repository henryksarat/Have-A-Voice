<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInModel<Event>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Calendar
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>
    
    <div class="col-21 events">
    	<%= Html.Encode(ViewData["Message"]) %>
        <%= Html.Encode(TempData["Message"]) %>
		<div class="clear">&nbsp;</div>
    
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
						<%= item.Information %>
					</div>
				</div>
				<div class="col-2 center">
					<div class="p-a5">
						<%= Html.ActionLink("Delete", "DeleteEvent", new { eventId = item.Id }, new { @class = "delete" }) %>
					</div>
				</div>
				<div class="clear">&nbsp;</div>
			</div>
			<div class="clear">&nbsp;</div>
			<div class="spacer-10">&nbsp;</div>
	    <% } %>
    
    <b>Add event:</b> <br />
    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %><br /><br />
    <% using (Html.BeginForm("AddEvent", "Calendar")) { %>
        Date: <%= Html.TextBox("Date", DateTime.UtcNow.AddMinutes(1))%>
        <%= Html.ValidationMessage("Date", "*") %><br />
        Information: <%= Html.TextArea("Information")%>
        <%= Html.ValidationMessage("Information", "*")%><br />
        <input type="submit" value="Create" /><br />
    <% } %>
	</div>
</asp:Content>
