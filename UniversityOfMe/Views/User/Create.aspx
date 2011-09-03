<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UniversityOfMe.Models.View.CreateUserModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript" language="javascript">
	    $(function () {
	        $('#DateOfBirth').datepicker({
	            yearRange: '1900:2011',
	            changeMonth: true,
	            changeYear: true,
	            dateFormat: "mm-dd-yy",
	            yearRange: '1900:2011'
	        });
	    });
	</script>

    <div class="twelve"> 
        <div class="create-feature-form create">
	        <div class="banner black full small"> 
		        REGISTER
	        </div>  
                <% Html.RenderPartial("Message"); %>
                <% Html.RenderPartial("Validation"); %>

                <div class="padding-col">
                    <% if(Model.RegisteredUserCount <= 100) { %>
                        <% using (Html.BeginForm("Create", "User", FormMethod.Post)) { %>
		                    <div class="field-holder">
			                    <%= Html.Label("First Name:") %>
                                <%= Html.TextBox("FirstName") %>
                                <%= Html.ValidationMessage("FirstName", "*", new { @class = "req" })%>
		                    </div> 

		                    <div class="field-holder">
			                    <%= Html.Label("Last Name:") %>
                                <%= Html.TextBox("LastName") %>
                                <%= Html.ValidationMessage("LastName", "*", new { @class = "req" })%>
		                    </div> 
						
		                    <div class="field-holder">
			                    <%= Html.Label("Email:") %>
                                <%= Html.TextBox("Email") %>
                                <%= Html.ValidationMessage("Email", "*", new { @class = "req" })%>
                    
		                    </div> 
						
		                    <div class="field-holder">
			                    <%= Html.Label("Password:") %>
                                <%= Html.Password("Password")%>
                                <%= Html.ValidationMessage("Password", "*", new { @class = "req" })%>
		                    </div> 
						
		                    <div class="field-holder">
			                    <%= Html.Label("Date Of Birth:") %>
                                <%= Html.TextBox("DateOfBirth", Model.getDateOfBirthFormatted())%>
                                <%= Html.ValidationMessage("DateOfBirth", "*", new { @class = "req" })%>
		                    </div> 
						
		                    <div class="field-holder">
			                    <%= Html.Label("Gender:") %>
                                <%= Html.DropDownList("Gender", Model.Genders)%>
                                <%= Html.ValidationMessage("Gender", "*", new { @class = "req" })%>
		                    </div> 

		                    <div class="field-holder">
                                <span class="empty-label">&nbsp;</span>
                                <%= Html.CheckBox("Agreement") %>
			                    I agree with the <a href="/Site/Terms" target="_blank">Terms of Use</a>.
                                <%= Html.ValidationMessage("Agreement", "*", new { @class = "req" })%>
		                    </div> 

			                <div class="input"> 
				                <input type="submit" name="submit" class="btn" value="Register" /> 
			                </div> 
                        <% } %>
                    <% } else { %>
                            Sorry but we've reached the maximum amount of users for now. We will be raising the threshold shortly so come back soon!
                    <% } %>
            </div>
        </div> 
    </div>
</asp:Content>

