<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="push-2 col-20">
		<div class="col-20 spacer-30">&nbsp;</div>
		<div class="clear">&nbsp;</div>
		
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab b-wht">
    		<span class="fnt-16 tint-6 bold">CONTACT US</span>
    	</div>
    	<div class="clear">&nbsp;</div>
    	
    	<div class="b-wht">
    		<div class="push-1 col-18">
    			<div class="spacer-30">&nbsp;</div>
    			<div class="clear">&nbsp;</div>
			    <% using (Html.BeginForm("ContactUs", "Site", FormMethod.Post, new { @class = "create" })) { %>
					<% Html.RenderPartial("Message"); %>
			        <% Html.RenderPartial("Validation"); %>
					
					<div class="clear">&nbsp;</div>
								
					<div class="push-1 col-16 fnt-14 teal m-btm10">
						Please fill out the following form to get in contact with the have a voice staff. We will try to respond back within 24 hours.
						<div class="clear">&nbsp;</div>
					</div>
					<div class="clear">&nbsp;</div>
			
				    <div class="col-4 m-rgt right">
				        <label for="FirstName">Email:</label>
				    </div>
				    <div class="col-4">
				        <%= Html.TextBox("Email") %>
				    </div>
				    <div class="m-lft col-10">
				        <span class="req">
					        <%= Html.ValidationMessage("Email", "*") %>
				        </span>
				    </div>
				    <div class="clear">&nbsp;</div>
				    <div class="spacer-10">&nbsp;</div>
			
			        <div class="col-4 m-rgt right">
				        <label for="Gender">Inquiry Type:</label>
				    </div>
				    <div class="col-6">
				        <%= Html.DropDownList("InquiryType", new SelectList(HaveAVoice.Helpers.HAVConstants.INQUIRY_TYPES)) %>
				    </div>
				    <div class="col-8 m-lft">
				        <span class="req">
					        <%= Html.ValidationMessage("InquiryType", "*")%>
				        </span>
				    </div>
				    <div class="clear">&nbsp;</div>
				    <div class="spacer-10">&nbsp;</div>
			
				    <div class="col-4 m-rgt right">
				        <label for="LastName">Inquiry:</label>
				    </div>
				    <div class="col-8">
				        <%= Html.TextArea("Inquiry", null, new { rows = "3", cols = "48", resize = "none", style = "width:100%;" })%>
				    </div>
				    <div class="col-6 m-lft">
				        <span class="req">
					        <%= Html.ValidationMessage("Inquiry", "*")%>
				        </span>
				    </div>
				    <div class="clear">&nbsp;</div>
				    <div class="spacer-10">&nbsp;</div>
								
				    <div class="push-4 col-2 right m-top10 m-btm30">
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
