<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.CreateUserModelBuilder>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<!DOCTYPE html>
<html>
	<head>
		<title>have a voice | giving you a voice in your community</title>
        <link rel="shortcut icon" href="/favicon.ico" />

        <link href="/Content/reset.css" rel="Stylesheet" type="text/css" />
        <link href="/Content/framework.css" rel="Stylesheet" type="text/css" />
        <link href="/Content/site.css" rel="stylesheet" type="text/css" />
        <link href="/Content/css/start/jquery-ui-1.8.14.custom.css" rel="stylesheet" type="text/css" />
        	
        <script type="text/javascript" src="/Content/js/jquery-1.5.1.min.js"></script>
        <script type="text/javascript" src="/Content/js/jquery-ui-1.8.14.custom.min.js"></script>
	</head>
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
    <% bool myIsLoggedin = HAVUserInformationFactory.IsLoggedIn(); %>
    <% string myLogoDivStyle = myIsLoggedin ? "lcol-logged-in" : "lcol";  %>
	<body>
        <div class="header2">
		    <div class="wrpr">
                <div class="col-wrpr">
			        <div class="<%= myLogoDivStyle %>">
				        <a href="<%= HAVConstants.BASE_URL %>">
                            <img src="/Content/images/hav.png" alt="have a voice" class="mt5 mb15" />
                        </a>
			        </div>
                    <% if (!myIsLoggedin) { %>
                        <% Html.RenderPartial("LogOnUserControl"); %>
                    <% } else { %>
                        <% Html.RenderPartial("NavigationUserControl"); %>
                    <% } %>
			    </div>
		    </div>
        </div>
		<div class="wrpr">
		<div class="col-wrpr mt44 mb32">
			<div class="lcol">
				<div class="w425">
					<div class="mainpage-slogan">
                        Giving you a voice in your community
                    </div>
					<ul class="actn">
						<li class="issue-main">
							<div class="mainpage-heading">
                                <a class="main-page-header" href="/Issue/List">Issues</a>
                            </div>
							Discuss or post an issue important to you and get direct feedback from other users, politicians, and political candidates.
						</li>
						<li class="friend-main">
							<div class="mainpage-heading">
                                Find Friends
                            </div>
							Connect with neighbors and find people nationwide who share similar political interests.
						</li>
						<li class="group-main">
							<div class="mainpage-heading">
                                <a class="main-page-header" href="/Group/List">Groups</a>
                            </div>
							Create your own or join an existing group. Groups can range from a small neighborhood to a large national group.
						</li>
						<li class="petition-main">
							<div class="mainpage-heading">
                                <a class="main-page-header" href="/Petition/List">Petitions</a>
                            </div>
							Get real change done in your neighborhood. Create an online petition for your neighbors to sign.
						</li>
						<li class="social-main">
							<div class="mainpage-heading">
                                Go Viral!
                            </div>
							Every issue, group, and petition you start and participate in can easily be shared on Facebook, Twitter, and Google+.
						</li>
					</ul>
						
					<script src="http://widgets.twimg.com/j/2/widget.js"></script>
					<script>
					    new TWTR.Widget({
					        version: 2,
					        type: 'profile',
					        rpp: 4,
					        interval: 6000,
					        width: 425,
					        height: 233,
					        theme: {
					            shell: {
					                background: '#4dc8ea',
					                color: '#ffffff'
					            },
					            tweets: {
					                background: '#ffffff',
					                color: '#555555',
					                links: '#3eb8da'
					            }
					        },
					        features: {
					            scrollbar: false,
					            loop: false,
					            live: false,
					            hashtags: false,
					            timestamp: false,
					            avatars: false,
					            behavior: 'default'
					        }
					    }).render().setUser('Haveavoice').start();
					</script>
				</div>
			</div>
				
			<div class="rcol">
				<div class="frm">
					<div class="w475">
						<div class="mainpage-signup">
                            Sign Up Now!
                        </div>
						<div class="reg">
                            <% using (Html.BeginForm("Create", "User", FormMethod.Post)) { %>
							    <div class="inpt">
								    <label for="FirstName">First Name:</label>
                                    <%= Html.TextBox("FirstName", string.Empty, new { @class = "txt" })%>
							    </div>
							    <div class="inpt">
								    <label for="lame">Last Name:</label>
								    <%= Html.TextBox("LastName", Model.LastName, new { @class = "txt" })%>
							    </div>
							    <div class="inpt">
								    <label for="email">Email Address:</label>
								    <%= Html.TextBox("Email", Model.Email, new { @class = "txt" })%>
							    </div>
							    <div class="inpt">
								    <label for="pass">Password:</label>
                                    <%= Html.Password("Password", string.Empty, new { @class = "txt" })%>
							    </div>
							    <div class="inpt">
								    <label for="gender">Gender:</label>
                                    <%= Html.DropDownList("Gender", Model.Genders) %>
							    </div>
							    <div class="inpt">
								    <label for="bdate">Birthday Date:</label>
								    <%= Html.TextBox("DateOfBirth", Model.getDateOfBirthFormatted(), new { @class = "txt" })%>
							    </div>
							    <div class="inpt">
								    <label for="state">State:</label>
								    <%= Html.DropDownList("State", Model.States) %>
							    </div>
							    <div class="multi-inpt">
								    <div class="inpt">
									    <label for="city">City:</label>
									    <%= Html.TextBox("City", Model.City, new { @class = "txt" })%>
								    </div>
								    <div class="inpt sm">
									    <label for="zip">Zip Code:</label>
									    <%= Html.TextBox("Zip", Model.Zip, new { @class = "txt" })%>
								    </div>
							    </div>
								
							    <div class="right mr62">
								    <input type="submit" class="btn orange long" value="Sign Up" /><br />
								    <div class="med mt12">
									    By clicking this button I accept the <a href="/Site/Terms">Terms of Use</a>.
								    </div>
							    </div>
                            <% } %>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>

  	<div class="footer2">
		<div class="wrpr ftwrpr">
			<div class="col-wrpr pt13">
				<div class="lcol">
					Copyright &copy; Have a Voice 2011. All rights reserved.
				</div>
				<div class="rcol">
					<div class="right">
						<a href="/Site/Terms">Terms of Use</a>
						|
						<a href="/Site/Privacy">Privacy Policy</a>
						|
						<a href="/Site/ContactUs">Contact Us</a>
                        |
						<a href="http://www.haveavoiceblog.com">Blog</a>
						|
						<a href="/Site/AboutUs">About Have a Voice</a>
                        <a href="http://www.haveavoiceblog.com" class="wordpress">Wordpress</a>
						<a href="http://www.twitter.com/haveavoice" class="twitter">Twitter</a>
						<a href="http://www.facebook.com/haveavoice" class="facebook">Facebook</a>
					</div>
				</div>
			</div>
		</div>
	</div> 
	</body>

</html>
