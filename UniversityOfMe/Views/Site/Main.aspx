<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UniversityOfMe.Models.View.CreateUserModel>" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Search" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Format" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	University Only Marketplace, Buy Sell Textbooks, No Fees, No Shipping
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaDescriptionHolder" runat="server">
	<%= UniversityOfMe.Helpers.MetaHelper.MetaDescription() %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MetaKeywordsHolder" runat="server">
	<%= UniversityOfMe.Helpers.MetaHelper.MetaKeywords() %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="six"> 
	    <div class="banner red bold"> 
            <div class="seo">
                <h1>Free Social Marketplace only for University Students</h1>
            </div>
		    <span class="special-corner"></span> 
	    </div> 
					
	    <div class="clearfix"></div> 
	    <p class="pt20"> 
	    Benefits of using University of Me:
        <br />
        -Sell anything in your dorm or apartment.<br />
        -No listing fees or shipping, all items are sold on campus!<br />
        -Tag a <a class="itemlinked" href="/Search/<%= SearchFilter.TEXTBOOK.ToString() %>?page=1">textbook</a> to a real <a class="itemlinked" href="/Search/<%= SearchFilter.CLASS.ToString() %>?page=1">class</a> within your school. <br />
        -Easily search for <a class="itemlinked" href="/Search/<%= SearchFilter.TEXTBOOK.ToString() %>?page=1">textbooks</a> tagged to a class.<br />
        -<a class="itemlinked" href="/Search/<%= SearchFilter.ALL.ToString() %>?page=1">Easily find items other students in your university are selling.</a><br />
        -Know that you are safe when using University of Me because a <span style="font-style:italic">university email is required</span>.<br />
        -See what items your friends are selling.<br />
        -Your email is never publicly posted so you are free from spambots.
        
        <br /><br />
        <div style="vertical-align:top">
            <% if(Model.NewestItem != null) { %>
                <div style="width:250px; display: inline-block; vertical-align:top">
                    <span class="<%= Model.NewestItem.ItemTypeId.ToLower() %>-icon">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                    <span style="font-weight:bold">Latest item</span><br />
                    <%= Model.NewestItem.Title %> @ <span style="font-style:italic"><%= Model.NewestItem.UniversityId %></span> for <%= MoneyFormatHelper.Format(Model.NewestItem.Price) %><br />
                    <img style="width:100px; height:100px" src="<%= PhotoHelper.ItemSellingPhoto(Model.NewestItem.ImageName) %>" /><br />
                    <%= string.IsNullOrEmpty(Model.NewestItem.Description) ? string.Empty : Model.NewestItem.Description %>
                </div>
            <% } %>

            <% if(Model.NewestTextbook != null) { %>
                <div style="width:250px; display: inline-block;vertical-align:top">
                    <span class="textbook-icon">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span style="font-weight:bold">Latest textbook</span><br />
                    <%= Model.NewestTextbook.BookTitle %> @ <span style="font-style:italic"><%= Model.NewestTextbook.UniversityId %></span> for <%= MoneyFormatHelper.Format(Model.NewestTextbook.Price) %><br />
                    <%= string.IsNullOrEmpty(Model.NewestTextbook.ClassSubject) ? string.Empty : "Used in: " + Model.NewestTextbook.ClassSubject + Model.NewestTextbook.ClassCourse + "<br />" %>
                    <img style="width:100px; height:100px" src="<%= PhotoHelper.TextBookPhoto(Model.NewestTextbook.BookPicture) %>" /><br />
                    <%= string.IsNullOrEmpty(Model.NewestTextbook.Details) ? string.Empty : Model.NewestTextbook.Details%>
                </div>
            <% } %>
        </div>
        <br />
        <script src="http://widgets.twimg.com/j/2/widget.js"></script>
        <script>
            new TWTR.Widget({
                version: 2,
                type: 'profile',
                rpp: 4,
                interval: 30000,
                width: 450,
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
			            <%= Html.Label("Gender:") %>
                        <%= Html.DropDownList("Gender", Model.Genders)%>
		            </div> 
		            <div class="input"> 
			            <span class="empty-indent">&nbsp;</span>
                        <%= Html.CheckBox("Agreement") %>
			            I agree with the <a class="itemlinked" href="/Site/Terms" target="_blank">Terms of Use</a>.
		            </div> 

			        <div class="input"> 
				        <input type="submit" name="submit" class="btn" value="Register" /> 
			        </div> 

			        <div class="input"> 
                        <%= Html.ActionLink("Click here to resend your activation email.", "Activation", "User", new { message = "2" }, new { @class = "itemlinked" })%>
			        </div> 
                <% } %>
            <% } else { %>
                <div class="form">
                    Sorry but we've reached the maximum amount of users for now. We will be raising the threshold shortly so come back soon!
                </div>
            <% } %>
            <br />
            <div class="normal-page" style="padding-top:10px; padding-left:10px; padding-right:10px; padding-bottom:10px">
                <div class="big red">From our blog</div>
                <div class="outer-blog">
                    <%= BlogReader.GetBlog() %>
                </div>
                <div class="right blog-read-more">
                    Read more at <a class="main-page-blog-read-more" href="http://blog.universityof.me">our blog</a>.
                </div>
            </div>
    </div>
</asp:Content>
