<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Main
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
	    <form class="form"> 
		    <div class="big mb32 red">Create an account</div> 
		    <div class="input"> 
			    <label for="name">First Name:</label> 
			    <input type="text" name="name" id="firstName" /> 
		    </div> 

		    <div class="input"> 
			    <label for="name">Last Name:</label> 
			    <input type="text" name="name" id="lastName" /> 
		    </div> 
						
		    <div class="input"> 
			    <label for="email">Email:</label> 
			    <input type="email" name="email" id="email" /> 
		    </div> 
						
		    <div class="input"> 
			    <label for="pass">Password:</label> 
			    <input type="password" name="pass" id="pass" /> 
		    </div> 
						
		    <div class="input"> 
			    <label for="confirm">Re-Password:</label> 
			    <input type="password" name="confirm" id="confirm" /> 
		    </div> 
						
		    <div class="input"> 
			    <label for="gender">Gender:</label> 
			    <select name="gender" id="gender"> 
				    <option>Select Sex</option> 
				    <option value="M">Male</option> 
				    <option value="F">Female</option> 
			    </select> 
		    </div> 
			<div class="input"> 
				<input type="submit" name="submit" class="btn" value="Register" /> 
			</div> 
	    </form> 
    </div>
</asp:Content>
