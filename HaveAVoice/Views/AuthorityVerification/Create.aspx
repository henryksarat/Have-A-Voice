<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-24">
        <div class="col-24 spacer-30">&nbsp;</div>
    
    	<div class="push-1 col-5 center p-t5 p-b5 t-tab b-wht">
    		<span class="fnt-16 tint-6 bold">AUTHORITY</span>
    	</div>
    	<div class="clear">&nbsp;</div>
    	
    	<div class="b-wht">
    		<div class="col-1">&nbsp;</div>
    		<div class="col-22">
    			<div class="spacer-30">&nbsp;</div>

				<% using (Html.BeginForm("Create", "AuthorityVerification", FormMethod.Post, new { @class = "create" })) { %>
					<% Html.RenderPartial("Message"); %>
					<%= Html.ValidationSummary("Send was unsuccessful. Please correct the errors and try again.") %>
					<div class="clear">&nbsp;</div>
					
					<div class="push-3 col-16 fnt-14 teal m-btm10">
						Enter an email of an authority to send to
						<div class="clear">&nbsp;</div>
					</div>
					<div class="clear">&nbsp;</div>
					
					<div class="col-4 m-rgt right">
						<label for="Email">Email:</label>
						<div class="clear">&nbsp;</div>
					</div>
					<div class="col-4">
						<%= Html.TextBox("Email")%>
						<div class="clear">&nbsp;</div>
					</div>
					<div class="col-14 m-lft">
						<span class="req">
							<%= Html.ValidationMessage("Email", "*") %>
						</span>
						<div class="clear">&nbsp;</div>
					</div>
					
					<div class="clear">&nbsp;</div>
					
					<div class="col-8 right m-top10">
						<input type="submit" value="Send" class="create" />
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
