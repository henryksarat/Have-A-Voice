<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Result
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="normal-page">
        <div class="padding-col">
            <% Html.RenderPartial("Message"); %>
            <% Html.RenderPartial("Validation"); %>

    	    <div class="heading">Contact Us</div>
            Please fill out the following form to get in contact with the University of Me staff. We will try to respond back within 24 hours.<br /><br />

            <% using (Html.BeginForm("ContactUs", "Site", FormMethod.Post, new { @class = "create" })) { %>
		        <div class="field-holder">
			        <%= Html.Label("Email")%>
                    <%= Html.TextBox("Email")%>
                    <%= Html.ValidationMessage("Email", "*", new { @class = "req" })%>
		        </div> 

                <div class="field-holder">                        							
                    <label for="Review">Inquiry Type</label>							
	                <%= Html.DropDownList("InquiryType", new SelectList(UniversityOfMe.Helpers.UOMConstants.INQUIRY_TYPES))%>
                    <%= Html.ValidationMessage("InquiryType", "*", new { @class = "req" })%>
                </div>

                <div class="field-holder">                        							
                    <label for="Review">Inquiry</label>							
	                <%= Html.TextArea("Inquiry", new { @class = "textarea" })%>
                    <%= Html.ValidationMessage("Inquiry", "*", new { @class = "req" })%>
                </div>

			    <div class="input"> 
				    <input type="submit" name="submit" class="btn" value="Submit" /> 
			    </div> 
            <% } %>
        </div>
    </div>

</asp:Content>

