<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% using (Html.BeginForm("Create", "AuthorityVerification", FormMethod.Post, new { @class = "create" })) { %>
	    <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

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
	        <label for="Gender">Inquiry Type:</label>
	    </div>
	    <div class="col-4">
	        <%= Html.DropDownList("InquiryType", new SelectList(HaveAVoice.Helpers.HAVConstants.INQUIRY_TYPES)) %>
	    </div>
	    <div class="col-14 m-lft">
	        <span class="req">
		        <%= Html.ValidationMessage("InquiryType", "*")%>
	        </span>
	    </div>
	    <div class="clear">&nbsp;</div>
	    <div class="spacer-10">&nbsp;</div>

	    <div class="col-4 m-rgt right">
	        <label for="LastName">Inquiry:</label>
	    </div>
	    <div class="col-4">
	        <%= Html.TextArea("Inquiry")%>
	    </div>
	    <div class="col-14 m-lft">
	        <span class="req">
		        <%= Html.ValidationMessage("Inquiry", "*")%>
	        </span>
	    </div>
	    <div class="clear">&nbsp;</div>
	    <div class="spacer-10">&nbsp;</div>
					
	    <div class="col-8 right m-top10">
		    <input type="submit" value="Send" class="create" />
		    <div class="clear">&nbsp;</div>
	    </div>
    <% } %>

</asp:Content>
