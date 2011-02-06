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
			<div class="push-1 col-22">
				<div class="spacer-30">&nbsp;</div>
                <% Html.RenderPartial("Message"); %>
				<%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
				<div class="clear">&nbsp;</div>
				
				<% using (Html.BeginForm("Request", "Password", FormMethod.Post, new { @class = "create" })) { %>
					<div class="push-3 col-16 fnt-14 teal m-btm10">
						Enter your have a voice&trade; email address.<br />
						We will send you an email with instructions on how to change your password.
						<div class="clear">&nbsp;</div>
					</div>
					<div class="clear">&nbsp;</div>
					
					<div class="col-4 m-rgt right">
						<label for="Email">Email:</label>
						<div class="clear">&nbsp;</div>
					</div>
					<div class="col-4">
						<%= Html.TextBox("Email") %>
						<div class="clear">&nbsp;</div>
					</div>
					<div class="col-14 m-lft">
						<span class="req">
							<%= Html.ValidationMessage("Email", "*") %>
						</span>
						<div class="clear">&nbsp;</div>
					</div>
					<div class="clear">&nbsp;</div>
					
					<div class="col-8 right m-btm30 m-top10">
						<input type="submit" name="submit" class="create" value="Submit" />
						<div class="clear">&nbsp;</div>
					</div>
				<% } %>
				<div class="clear">&nbsp;</div>
			</div>
			<div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>
	</div>
</asp:Content>
