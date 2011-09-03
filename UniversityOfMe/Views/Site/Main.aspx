<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UniversityOfMe.Models.View.CreateUserModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Main
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

    <div class="six"> 
	    <div class="banner red bold"> 
		    University of Me helps connect University of Chicago students
		    <span class="special-corner"></span> 
	    </div> 
	    <div class="banner red mt-11 bold"> 
		    to what's important.
		    <span class="corner"></span> 
	    </div> 
					
	    <div class="clearfix"></div> 
					
	    <p class="pt20"> 
	    Welcome to the University Of Me, a site restricted to University of Chicago students by their email. This means no spam bots, no parents, and no randoms (almost!). UofMe brings the unviersity life full circle with profossor reviews, class reviews, class discussions, textbook bartering, sharing photos, campus dating, club interaction, and more!
	    </p> 
    </div> 
    <div class="six last"> 
            <% using (Html.BeginForm("Create", "User", FormMethod.Post, new {@class="form"})) { %>
		        <div class="big mb32 red">Create an account</div> 
		        <div class="input"> 
			        <%= Html.Label("First Name:") %>
                    <%= Html.TextBox("FirstName") %>
		        </div> 

		        <div class="input"> 
			        <%= Html.Label("Last Name:") %>
                    <%= Html.TextBox("LastName") %>
		        </div> 
						
		        <div class="input"> 
			        <%= Html.Label("Email:") %>
                    <%= Html.TextBox("Email") %>
                    
		        </div> 
						
		        <div class="input"> 
			        <%= Html.Label("Password:") %>
                    <%= Html.Password("Password")%>
		        </div> 
						
		        <div class="input"> 
			        <%: Html.Label("Date Of Birth:") %>
                    <%: Html.TextBox("DateOfBirth", Model.getDateOfBirthFormatted())%>
		        </div> 
						
		        <div class="input"> 
			        <%= Html.Label("Gender:") %>
                    <%= Html.DropDownList("Gender", Model.Genders)%>
		        </div> 
		        <div class="input"> 
			        <span class="empty-indent">&nbsp;</span>
                    <%= Html.CheckBox("Agreement") %>
			        I agree with the <a href="/Site/Terms" target="_blank">Terms of Use</a>.
		        </div> 

			    <div class="input"> 
				    <input type="submit" name="submit" class="btn" value="Register" /> 
			    </div> 
            <% } %>
    </div>
</asp:Content>
