<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SelectList>" %>

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
					<% Html.RenderPartial("Validation"); %>
					<div class="clear">&nbsp;</div>
					
					<div class="push-3 col-16 fnt-14 teal m-btm10">
						Enter an email, authority type, and an authority position. An email will be then sent to the specified email that the authority will follow.
						<div class="clear">&nbsp;</div>
					</div>
					<div class="clear">&nbsp;</div>
					
	    			<div class="col-4 m-rgt right">
	    				<label for="FirstName">Email:</label>
	    			</div>
	    			<div class="col-4">
	    				<%= Html.TextBox("Email") %>
	    			</div>
	    			<div class="m-lft col-14">
	    				<span class="req">
		    				<%= Html.ValidationMessage("Email", "*") %>
	    				</span>
	    			</div>
	    			<div class="clear">&nbsp;</div>
	    			<div class="spacer-10">&nbsp;</div>

	    			<div class="col-4 m-rgt right">
	    				<label for="FirstName">Authority Type:</label>
	    			</div>
	    			<div class="col-4">
	    				<%= Html.DropDownList("AuthorityType", new SelectList(HaveAVoice.Helpers.HAVConstants.AUTHORITY_ROLES, "Select"))%>
	    			</div>
	    			<div class="m-lft col-14">
	    				<span class="req">
		    				<%= Html.ValidationMessage("AuthorityType", "*")%>
	    				</span>
	    			</div>
	    			<div class="clear">&nbsp;</div>
	    			<div class="spacer-10">&nbsp;</div>

	    			<div class="col-4 m-rgt right">
	    				<label for="FirstName">Authority Position:</label>
	    			</div>
	    			<div class="col-4">
	    				<%= Html.DropDownList("AuthorityPosition", Model)%>
	    			</div>
	    			<div class="m-lft col-14">
	    				<span class="req">
		    				<%= Html.ValidationMessage("AuthorityPosition", "*")%>
	    				</span>
	    			</div>
	    			<div class="clear">&nbsp;</div>
	    			<div class="spacer-10">&nbsp;</div>
	  			
					<div class="col-8 right m-top10">
						<input type="submit" value="Send" class="create" />
					</div>
				<% } %>
				<div class="clear">&nbsp;</div>
			</div>
			<div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>
	</div>
</asp:Content>
