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
	    Welcome to the beta of University Of Me. The beta will only be open to a small batch of University of Chicago, it's first come first serve.
        However, we will be increasing the threshold periodically.
        <br /><br /> 
        UofMe is social networking only for university students, which means no spam bots, no parents, and 
        no randoms. UofMe brings the unviersity life full circle with campus dating, on-campus textbook bartering, university wide events, 
        class discussions (no more listservs!), class reviews, profossor reviews, photo sharing, club interaction, and more! 
        This website will be built with the students in mind, so if you have an idea for a new feature let us know!
        <br /><br />
        <script src="http://widgets.twimg.com/j/2/widget.js"></script>
        <script>
            new TWTR.Widget({
                version: 2,
                type: 'profile',
                rpp: 4,
                interval: 30000,
                width: 550,
                height: 500,
                theme: {
                    shell: {
                        background: '#971e20',
                        color: '#ffffff'
                    },
                    tweets: {
                        background: '#ffffff',
                        color: '#000000',
                        links: '#971e20'
                    }
                },
                features: {
                    scrollbar: false,
                    loop: false,
                    live: false,
                    hashtags: true,
                    timestamp: true,
                    avatars: false,
                    behavior: 'all'
                }
            }).render().setUser('uofme').start();
        </script>
	    </p> 
    </div> 
    <div class="six last"> 
            <% if(Model.RegisteredUserCount <= 100) { %>
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
            <% } else { %>
                <div class="form">
                    Sorry but we've reached the maximum amount of users for now. We will be raising the threshold shortly so come back soon!
                </div>
            <% } %>

    </div>
</asp:Content>
