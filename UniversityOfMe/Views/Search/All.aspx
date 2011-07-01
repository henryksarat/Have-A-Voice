<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<ISearchResult>>" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View.Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= UOMConstants.TITLE %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="four"> 
	<div class="banner title"> 
		FILTER RESULTS
	</div> 
	<div class="box spec"> 
		<ul class="filter"> 
			<li class="active"> 
				<a href="#" class="all">All Search Results</a> 
			</li> 
			<li> 
				<a href="#" class="friend">Friend Search Results</a> 
			</li> 
			<li> 
				<a href="#" class="prof">Latest Professor Ratings</a> 
			</li> 
			<li> 
				<a href="#" class="class">Latest Class Ratings</a> 
			</li> 
			<li> 
				<a href="#" class="event">Latest Events on Campus</a> 
			</li> 
			<li> 
				<a href="#" class="book">Buy/Sell Textbooks</a> 
			</li> 
			<li> 
				<a href="#" class="org">Most Active Organizations</a> 
			</li> 
			<li> 
				<a href="#" class="post">General Postings</a> 
			</li> 
		</ul> 
	</div> 
</div> 
				
<div class="eight last"> 
	<div class="banner title black full red-top small clearfix"> 
		<span>SEARCH RESULTS</span> 
		<div class="frgt max-w240 clearfix"> 
			<div class="search clearfix"> 
				<input type="text" class="inpt w227" /> 
				<div class="select"> 
					<a href="#" class="search">Search</a> 
					<ul class="options"> 
						<li class="active"> 
							<a href="#" class="all">All</a> 
						</li> 
						<li> 
							<a href="#" class="people">People</a> 
						</li> 
						<li> 
							<a href="#" class="case">Classes</a> 
						</li> 
						<li> 
							<a href="#" class="cal">Events</a> 
						</li> 
						<li> 
							<a href="#" class="text">Book</a> 
						</li> 
						<li> 
							<a href="#" class="paper">Paper</a> 
						</li> 
						<li> 
							<a href="#" class="org">Organizations</a> 
						</li> 
					</ul> 
				</div> 
			</div> 
		</div> 
	</div> 
	<div class="box filter mb0"> 
		<div class="input"> 
			<label for="filter-code">Search By</label> 
			<select id="filter-code" name="filter-code"> 
				<option>Class Code</option> 
				<option>Code 1</option> 
				<option>Code 2</option> 
				<option>Code 3</option> 
				<option>Code 4</option> 
			</select> 
		</div> 
		<div class="input"> 
			<label for="order-code">Order By</label> 
			<select id="order-code" name="order-code"> 
				<option>Class Code</option> 
				<option>Code 1</option> 
				<option>Code 2</option> 
				<option>Code 3</option> 
				<option>Code 4</option> 
			</select> 
		</div> 
	</div> 
	<div class="search result"> 
		<div class="res"> 
			<div class="res-con class clearfix"> 
				<div class="actions"> 
					<div class="rating"> 
						<span class="star"></span> 
						<span class="star"></span> 
						<span class="half"></span> 
						<span class="empty"></span> 
						<span class="empty"></span> 
						(43 ratings)
					</div> 
					<a href="#">21 Discussion Posts</a><br /> 
					<a href="#">3 Enrolled</a> 
				</div> 
				<span class="title">CIS 210</span><br /> 
				Introduction to Visual Basic<br /> 
				Summer 2011
			</div> 
		</div> 
        <% foreach (ISearchResult mySearchResult in Model) { %>
            <div class="res"> 
                <%= mySearchResult.CreateResult() %>
            </div>
        <% } %>
        <!--
		<div class="res"> 
			<div class="res-con class clearfix"> 
				<div class="actions"> 
					<div class="rating"> 
						<span class="star"></span> 
						<span class="star"></span> 
						<span class="half"></span> 
						<span class="empty"></span> 
						<span class="empty"></span> 
						(43 ratings)
					</div> 
					<a href="#">21 Discussion Posts</a><br /> 
					<a href="#">3 Enrolled</a> 
				</div> 
				<span class="title">CIS 210</span><br /> 
				Introduction to Visual Basic<br /> 
				Summer 2011
			</div> 
		</div> 
		<div class="res"> 
			<div class="res-con organization clearfix"> 
				<div class="actions"> 
					<a href="#" class="add">Request to Join Organization</a> 
				</div> 
				<img src="wanzo.png" /> 
				<span class="title">WANZO Club</span><br /> 
				13 Members
			</div> 
		</div> 
		<div class="res"> 
			<div class="res-con event clearfix"> 
				<div class="actions"> 
					<a href="#">1 Post to Event Board</a><br /> 
					<a href="#">4 Attending</a><br /> 
					<a href="#" class="add">I Am Attending</a><br /> 
					<a href="#" class="remove">I Am NOT Attending</a> 
				</div> 
				<span class="title">Beach Party</span><br /> 
				We're having a party on the beach and...<br /> 
				Start Date: Monday Aug 12, 2011 10:00 AM<br /> 
				End Date: Monday Aug 12, 2011 10:00 PM
			</div> 
		</div> 
		<div class="res"> 
			<div class="res-con post clearfix"> 
				<div class="actions"> 
					Date of Last Post: 08/12/2011<br /> 
					Number of Posts: <a href="#">4</a> 
				</div> 
				<span class="title">Jelly Beans</span> 
				<span class="tiny">By Henryk Sarat Yesterday</span><br /> 
				Some Details...
			</div> 
		</div> 
		<div class="res"> 
			<div class="res-con person clearfix"> 
				<div class="actions"> 
					<a href="#" class="pm">Send a Message</a><br /> 
					<a href="#" class="addfriend">Add as Friend</a><br /> 
					<a href="#" class="defriend">Remove as Friend</a><br /> 
					<a href="#" class="beer">Send Beer</a> 
				</div> 
				<img src="http://www.justin5mins.com/wp-content/uploads/2011/04/profile.jpg" /> 
				<span class="title">Henryk Sarat</span><br /> 
				University of Chicago
			</div> 
		</div> 
		<div class="res"> 
			<div class="res-con professor clearfix"> 
				<div class="actions"> 
					<div class="rating"> 
						<span class="star"></span> 
						<span class="star"></span> 
						<span class="half"></span> 
						<span class="empty"></span> 
						<span class="empty"></span> 
						(43 ratings)
					</div> 
				</div> 
				<span class="title">Prof. Martha Nussbaum</span> 
			</div> 
		</div> 
		<div class="res"> 
			<div class="res-con textbook clearfix"> 
				<div class="actions"> 
					<a href="#" class="pm">Contact Seller</a> 
				</div> 
				<img src="http://www.prelovac.com/vladimir/wp-content/uploads/2009/02/book2.png" /> 
				<div class="flft"> 
					<span class="title">WordPress Plugin Development</span><br /> 
					Condition: Slightly Used<br /> 
					Associated Class: CIS 210<br /> 
					Edition: 2nd<br /> 
					Asking Price: $123.45<br /> 
					Details: Beware the highlighting of useful information.
				</div> 
			</div> 
		</div> 
        -->
						
	</div> 
	<div class="pagination"> 
		<ul> 
			<li class="prev-off">&laquo; Previous</li> 
			<li class="active">1</li> 
			<li> 
				<a href="#">2</a> 
			</li> 
			<li> 
				<a href="#">3</a> 
			</li> 
			<li> 
				<a href="#">4</a> 
			</li> 
			<li class="next-on"> 
				<a href="#">Next &raquo;</a> 
			</li> 
		</ul> 
	</div> 
</div>
</asp:Content>

