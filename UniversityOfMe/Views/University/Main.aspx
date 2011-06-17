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
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="form"> 
			<div class="twoCol clearfix">
				<div class="lCol"> 
					<span class="normal bold professor">Latest Professor Rating</span> 
					<ul> 

                        <% foreach (Professor myProfessor in Model.Get().Professors.Take<Professor>(5)) { %>
                            <li>
                                <% string myProfessorNameUrl = URLHelper.ToUrlFriendly(myProfessor.FirstName + " " + myProfessor.LastName); %>
                                <a class="itemlinked" href="<%= URLHelper.BuildProfessorUrl(myProfessor) %>"><%= myProfessor.FirstName + " " +  myProfessor.LastName %></a>                         
                                <% int myTotalReviews = myProfessor.ProfessorReviews.Count; %>
							    <div class="rating"> 
                                <% if (myTotalReviews > 0) { %>
                                    <%= StarHelper.AveragedFiveStarImages(myProfessor.ProfessorReviews.Sum(r => r.Rating), myTotalReviews) %>
								    <span class="gray tiny">(<%= myTotalReviews%> ratings)</span> 
                                <% } else { %>
								    <span class="gray tiny">No reviews yet</span> 
                                <% } %>
                                </div>
                            </li>
                        <% } %>
					</ul> 
                    <div class="flft mr9">
                        <%= Html.ActionLink("+", "Create", "Professor", null, new { @class="add-new-cross" })%>
                    </div>
                    <%= Html.ActionLink("Create New", "Create", "Professor", null, new { @class="add-new" })%>
                    <%= Html.ActionLink("View All", "List", "Professor", null, new { @class="view-all" })%>
				</div> 
				<div class="rCol"> 
					<span class="normal bold class">Latest Class Rating</span>
					<ul> 
                        <% foreach (Class myClass in Model.Get().Classes.Take<Class>(5)) { %>
                            <li>
                                <% int myTotalReviews = myClass.ClassReviews.Count; %>
							    <div class="rating">
                                    <% if (myTotalReviews > 0) { %>
								        <%= StarHelper.AveragedFiveStarImages(myClass.ClassReviews.Sum(r => r.Rating), myTotalReviews) %>
								        <span class="gray tiny">(<%= myTotalReviews %> ratings)</span> 
                                    <% } else { %>
                                        There are no reviews yet    
                                    <% } %>
								    <p class="pt7 lightgray">Board Posts: <%= myClass.ClassBoards.Count %></p> 
                                </div> 
							    <a class="itemlinked" href="<%= URLHelper.BuildClassDiscussionUrl(myClass) %>"><%= myClass.ClassCode %></a><br /> 
							    <span class="gray"><%= TextShortener.Shorten(myClass.ClassTitle, 20) %></span><br /> 
							    <span class="gold"><%= myClass.AcademicTerm.DisplayName %> <%= myClass.Year %></span> 
                    
                            </li>
                        <% } %>
					</ul> 
                    <div class="flft mr9">
                        <%= Html.ActionLink("+", "Create", "Class", null, new { @class="add-new-cross" })%>
                    </div>
                    <%= Html.ActionLink("Create New", "Create", "Class", null, new { @class="add-new" })%>
					<%= Html.ActionLink("View All", "List", "Class", null, new { @class="view-all" })%>
					</div>
			</div>
			<div class="twoCol clearfix">
				<div class="lCol"> 
				<span class="normal bold event">Latest Events on Campus</span> 
				<ul> 
                    <% foreach (Event myEvent in Model.Get().Events.Take<Event>(5)) { %>
						<li> 
							<a class="itemlinked" href="<%= URLHelper.BuildEventUrl(myEvent) %>"><%= myEvent.Title %></a>
							<div class="rating darkgray"> 
								<%= DateHelper.ToLocalTime(myEvent.StartDate) %>
							</div> 
						</li> 
                    <% } %>
				</ul>
                <div class="flft mr9">
                    <%= Html.ActionLink("+", "Create", "Event", null, new { @class="add-new-cross" })%>
                </div>
                <%= Html.ActionLink("Create New", "Create", "Event", null, new { @class="add-new" })%>
				<%= Html.ActionLink("View All", "List", "Event", null, new { @class="view-all" })%>
			</div>
			<div class="rCol"> 
				<span class="normal bold book">Buy/Sell Textbooks</span> 
				<ul> 
                    <% foreach (TextBook myTextBook in Model.Get().TextBooks.Take<TextBook>(5)) { %>
                        <li>
						    <a class="itemlinked" href="/<%= myTextBook.UniversityId %>/TextBook/Details/<%= myTextBook.Id %>"><%= myTextBook.BookTitle %></a>
							<span class="darkgray">( <%= myTextBook.ClassCode %>)</span> 
							<div class="rating"> 
                                <% if (myTextBook.BuySell.Equals("Buy")) { %>
                                    <a href="<%= URLHelper.BuildTextbookUrl(myTextBook) %>" class="buy">"Buy"</a> 
                                <% } else { %>
                                    <a href="<%= URLHelper.BuildTextbookUrl(myTextBook) %>" class="sell">"Sell"</a> 
                                <% } %>
							</div> 
                        </li>
                    <% } %>
				</ul> 
                <div class="flft mr9">
                    <%= Html.ActionLink("+", "Create", "TextBook", null, new { @class="add-new-cross" })%>
                </div>
                <%= Html.ActionLink("Create New", "Create", "TextBook", null, new { @class="add-new" })%>
				<%= Html.ActionLink("View All", "List", "TextBook", null, new { @class="view-all" })%>
			</div>
            </div>
			<div class="twoCol clearfix">
				<div class="lCol"> 
				<span class="normal bold organization">Most Active Organizations</span> 
				<ul> 
                    <% foreach (Club myClub in Model.Get().Organizations.Take<Club>(5)) { %>
                        <li>
                            <a class="itemlinked" href="/<%= myClub.UniversityId %>/Club/Details/<%= myClub.Id %>"><%= myClub.Name %></a></span> - <span class="gold"><%= myClub.ClubTypeDetails.DisplayName %>
                        </li>                         
                    <% } %>
				</ul> 
                <div class="flft mr9">
                    <%= Html.ActionLink("+", "Create", "Club", null, new { @class="add-new-cross" })%>
                </div>
                <%= Html.ActionLink("Create New", "Create", "Club", null, new { @class = "add-new" })%>
				<%= Html.ActionLink("View All", "List", "Club", null, new { @class="view-all" })%>
				</div>
				<div class="rCol"> 
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
                <div class="flft mr9">
                    <%= Html.ActionLink("+", "Create", "GeneralPosting", null, new { @class="add-new-cross" })%>
                </div>
                <%= Html.ActionLink("Create New", "Create", "GeneralPosting", null, new { @class="add-new" })%>
				<%= Html.ActionLink("View All", "List", "GeneralPosting", null, new { @class="view-all" })%>
				</div>
            </div>
		</div>
    </div>
</asp:Content>

