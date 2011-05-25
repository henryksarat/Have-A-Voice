<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<UniversityView>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UniversityOfMe - <%= Model.Get().University.UniversityName %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
		<div class="form"> 
			<ul class="full"> 
				<li class="professor"> 
					<span class="normal bold professor">Latest Professor Rating</span> 
					<ul> 

                        <% foreach (Professor myProfessor in Model.Get().Professors.Take<Professor>(5)) { %>
                            <li>
                                <% string myProfessorNameUrl = URLHelper.ToUrlFriendly(myProfessor.FirstName + " " + myProfessor.LastName); %>
                                <a class="itemlinked" href="/<%= myProfessor.UniversityId %>/Professor/Details/<%= myProfessorNameUrl %>"><%= myProfessor.FirstName + " " +  myProfessor.LastName %></a>                         
                                <% int myTotalReviews = 16; %>
                                <% if (myTotalReviews > 0) { %>
							        <div class="rating"> 
								        <span class="star"></span> 
								        <span class="star"></span> 
								        <span class="half"></span> 
								        <span class="empty"></span> 
								        <span class="empty"></span> 
								        <span class="gray tiny">(<%= myTotalReviews %> ratings)</span> 
							        </div>    
                                <% } %>
                            </li>
                        <% } %>
					</ul> 
                    <%= Html.ActionLink("View All", "List", "Professor", null, new { @class="view-all" })%>
					<div class="clearfix"></div> 
				</li> 
				<li class="class"> 
					<span class="normal bold class">Latest Class Rating</span>
					<ul> 
                        <% foreach (Class myClass in Model.Get().Classes.Take<Class>(5)) { %>
                            <li>
							    <div class="rating"> 
								    <span class="star"></span> 
								    <span class="star"></span> 
								    <span class="half"></span> 
								    <span class="empty"></span> 
								    <span class="empty"></span> 
								    <span class="gray tiny">(43 ratings)</span> 
								    <p class="pt7 lightgray">Board Posts: 28</p> 
							    </div> 
							    <a class="itemlinked" href="<%= URLHelper.BuildClassDiscussionUrl(myClass) %>"><%= myClass.ClassCode %></a><br /> 
							    <span class="gray"><%= TextShortener.Shorten(myClass.ClassTitle, 20) %></span><br /> 
							    <span class="gold"><%= myClass.AcademicTerm.DisplayName %> <%= myClass.Year %></span> 
                    
                            </li>
                        <% } %>
					</ul> 
					<%= Html.ActionLink("View All", "List", "Class", null, new { @class="view-all" })%>
					<div class="clearfix"></div> 
				</li> 
				<li class="event"> 
					<span class="normal bold event">Latest Events on Campus</span> 
					<ul> 
                        <% foreach (Event myEvent in Model.Get().Events.Take<Event>(5)) { %>
						    <li> 
							    <a class="itemlinked" href="/<%= myEvent.UniversityId %>/Event/Details/<%= myEvent.Id %>"><%= myEvent.Title %></a>
							    <div class="rating darkgray"> 
								    <%= myEvent.StartDate%>
							    </div> 
						    </li> 
                        <% } %>
					</ul> 
					<%= Html.ActionLink("View All", "List", "Event", null, new { @class="view-all" })%>
					<div class="clearfix"></div> 
				</li> 
				<li class="book"> 
					<span class="normal bold book">Buy/Sell Textbooks</span> 
					<ul> 
                        <% foreach (TextBook myTextBook in Model.Get().TextBooks.Take<TextBook>(5)) { %>
                            <li>
						        <a class="itemlinked" href="/<%= myTextBook.UniversityId %>/TextBook/Details/<%= myTextBook.Id %>"><%= myTextBook.BookTitle %></a>
							    <span class="darkgray">( <%= myTextBook.ClassCode %>)</span> 
							    <div class="rating"> 
                                    <% if (myTextBook.BuySell.Equals("Buy")) { %>
                                        <a href="/<%= myTextBook.UniversityId %>/TextBook/Details/<%= myTextBook.Id %>" class="buy">"Buy"</a> 
                                    <% } else { %>
                                        <a href="/<%= myTextBook.UniversityId %>/TextBook/Details/<%= myTextBook.Id %>" class="sell">"Sell"</a> 
                                    <% } %>
							    </div> 
                            </li>
                        <% } %>
					</ul> 
					<%= Html.ActionLink("View All", "List", "TextBook", null, new { @class="view-all" })%>
					<div class="clearfix"></div> 
				</li> 
				<li class="organization"> 
					<span class="normal bold organization">Most Active Organizations</span> 
					<ul> 
                        <% foreach (Club myClub in Model.Get().Organizations.Take<Club>(5)) { %>
                            <li>
                                <a class="itemlinked" href="/<%= myClub.UniversityId %>/Club/Details/<%= myClub.Id %>"><%= myClub.Name %></a></span> - <span class="gold"><%= myClub.ClubTypeDetails.DisplayName %>
                            </li>                         
                        <% } %>
					</ul> 
					<%= Html.ActionLink("View All", "List", "Club", null, new { @class="view-all" })%>
					<div class="clearfix"></div> 
				</li> 
				<li class="general"> 
					<span class="nomral bold general">General Postings</span> 
					<ul> 
                        <% foreach (GeneralPosting myGeneralPosting in Model.Get().GeneralPostings.Take<GeneralPosting>(5)) { %>                           
						    <li> 
							    <a class="itemlinked" href="/<%= myGeneralPosting.UniversityId %>/GeneralPosting/Details/<%= myGeneralPosting.Id %>"><%= TextShortener.Shorten(myGeneralPosting.Title, 20) %></a><br />
							    <span class="tiny darkgray">Posts: <%= myGeneralPosting.GeneralPostingReplies.Count %></span><br /> 
							    <span class="tiny darkgray">Last post: <%= DateHelper.ToLocalTime(myGeneralPosting.GeneralPostingReplies.OrderByDescending(gp => gp.DateTimeStamp).First().DateTimeStamp) %></span> 
						    </li>
                        <% } %>
					</ul> 
					<%= Html.ActionLink("View All", "List", "GeneralPosting", null, new { @class="view-all" })%>
					<div class="clearfix"></div> 
				</li> 
			</ul> 
						
			<div class="clearfix"></div>	
		</div> 
	</div> 
</asp:Content>

