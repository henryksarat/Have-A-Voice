<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Request Password
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-24">
        <div class="col-24 spacer-30">&nbsp;</div>
    
    	<div class="push-1 col-6 center p-t5 p-b5 t-tab b-wht">
    		<span class="fnt-16 tint-6 bold">PASSWORD REQUEST</span>
    	</div>
    	<div class="clear">&nbsp;</div>
    	
    	<div class="b-wht">
			<div class="col-1">&nbsp;</div>
			<div class="col-22">
				<div class="spacer-30">&nbsp;</div>
				<%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
				<%= Html.Encode(ViewData["Message"]) %>
				
				<% using (Html.BeginForm("Request", "Password", FormMethod.Post, new { @class = "create" })) {%>
					<div class="push-3 col-8 fnt-14">
						Please enter your email address.
					</div>
					<div class="clear">&nbsp;</div>
					<div class="spacer-10">&nbsp;</div>
					
					<div class="col-4 right">
						<label for="Email">Email:</label>
					</div>
					<div class="col-4">
						<%= Html.TextBox("Email")%>
					</div>
					<div class="col-14">
						<%= Html.ValidationMessage("Email", "*")%>
					</div>
					<div class="clear">&nbsp;</div>
					<div class="spacer-10">&nbsp;</div>
					
					<div class="col-8 right">
						<input type="submit" name="submit" class="create" value="Submit" />
					</div>
					<div class="clear">&nbsp;</div>
					
					<div class="spacer-30">&nbsp;</div>
				<% } %>
			</div>
			<div class="col-1">&nbsp;</div>
			<div class="clear">&nbsp;</div>
		</div>
</asp:Content>
