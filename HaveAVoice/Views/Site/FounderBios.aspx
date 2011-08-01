<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	About Us
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="push-2 col-20">
		<div class="col-20 spacer-30">&nbsp;</div>
		<div class="clear">&nbsp;</div>
		
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
    		<a class="issue-create" href="/Site/AboutUs">About Us</a>
    	</div>
        <div class="push-1 col-4 center p-t5 p-b5 t-tab b-wht">
    	    <span class="fnt-16 tint-6 bold">Founder Bios</span>
    	    <div class="clear">&nbsp;</div>
        </div>
    	<div class="clear">&nbsp;</div>
    	
    	<div class="b-wht">
    		<div class="push-1 col-18">
    			<div class="spacer-30">&nbsp;</div>
    			<div class="clear">&nbsp;</div>

				<div class="p-a10 m-a10 fnt-14">
					<h4 class="fnt-16">Henryk Sarat</h4>
                    <h4 class="fnt-12">Founder and Chief Executive Officer</h4>
                    <h4 class="fnt-12">Follow henryk on twitter 
                        <a class="sitelink" href="http://www.twitter.com/henryksarat" target=”_blank”>
                            @henryksarat
                        </a>
                    </h4>
                    Henryk received his Master's in Computer Science from the University of Chicago. 
                    After graduating, he worked for the Dutch high-frequency trading firm IMC as a software developer. 
                    While working at the firm, Henryk was inspired(read <a class="sitelink" href="/Site/AboutUs">About Us</a> for the story) 
                    to create Have a Voice and started developing it part-time. Henryk finally launched Have a Voice on 
                    February 9th, 2011. <br /><br />

                    <h4 class="fnt-16">Joe Sesso</h4>
                    <h4 class="fnt-12">Co-Founder and Chief Marketing Officer</h4>
                    <h4 class="fnt-12">Follow joe on twitter 
                        <a class="sitelink" href="http://www.twitter.com/joesesso" target=”_blank”>
                            @joesesso
                        </a>
                    </h4>
                    Before co-founding Have a Voice, Joe Sesso was the National Speaker for the 
                    world's largest real estate website, Realtor.com. During his tenure as National 
                    Speaker, he spoke to more than 25,000 people in 48 states about technology and 
                    social media.<br /><br />

                    He is an award-winning author and is a member of the National Speakers Association 
                    and Toastmasters International. He has a Bachelors degree from Indiana University 
                    and currently lives in Chicago.<br /><br />
				<div class="clear">&nbsp;</div>
			</div>
			<div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>
	</div>
</asp:Content>
