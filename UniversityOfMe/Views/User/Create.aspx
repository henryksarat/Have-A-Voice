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
	    <div class="banner black full small red-top"> 
		    REGISTER
	    </div> 
	    <p class="p20"> 
            <% Html.RenderPartial("Message"); %>
            <% Html.RenderPartial("Validation"); %>

            <% using (Html.BeginForm("Create", "User", FormMethod.Post, new {@class="page"})) { %>
		        <div class="input"> 
			        <%= Html.Label("First Name:") %>
                    <%= Html.TextBox("FirstName") %>
                    <%= Html.ValidationMessage("FirstName", "*")%>
		        </div> 

		        <div class="input"> 
			        <%= Html.Label("Last Name:") %>
                    <%= Html.TextBox("LastName") %>
                    <%= Html.ValidationMessage("LastName", "*")%>
		        </div> 
						
		        <div class="input"> 
			        <%= Html.Label("Email:") %>
                    <%= Html.TextBox("Email") %>
                    <%= Html.ValidationMessage("Email", "*")%>
                    
		        </div> 
						
		        <div class="input"> 
			        <%= Html.Label("Password:") %>
                    <%= Html.Password("Password")%>
                    <%= Html.ValidationMessage("Password", "*")%>
		        </div> 
						
		        <div class="input"> 
			        <%= Html.Label("Date Of Birth:") %>
                    <%= Html.TextBox("DateOfBirth", Model.getDateOfBirthFormatted())%>
                    <%= Html.ValidationMessage("DateOfBirth", "*")%>
		        </div> 
						
		        <div class="input"> 
			        <%= Html.Label("Gender:") %>
                    <%= Html.DropDownList("Gender", Model.Genders)%>
                    <%= Html.ValidationMessage("Gender", "*")%>
		        </div> 

		        <div class="input"> 
			        <%= Html.Label("UofMe Url:") %>
			        univeristyof.me/ <input type="shorturl" name="ShortUrl" id="ShortUrl" /> 
                    <%= Html.ValidationMessage("ShortUrl", "*")%>
		        </div> 

		        <div class="input"> 
			        <%= Html.Label("Agreement:") %>
                    <%= Html.CheckBox("Agreement") %>
			        I agree with the <a href="/Site/Terms" target="_blank">Terms of Use</a>.
                    <%= Html.ValidationMessage("Agreement", "*")%>
		        </div> 

			    <div class="input"> 
				    <input type="submit" name="submit" class="btn" value="Register" /> 
			    </div> 
            <% } %>
	    </p> 
    </div> 
</asp:Content>

