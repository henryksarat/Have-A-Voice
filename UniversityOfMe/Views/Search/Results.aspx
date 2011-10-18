<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SearchResultsModel>" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Constants" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Search" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Configuration" %>
<%@ Import Namespace="UniversityOfMe.Models.View.Search" %>
<%@ Import Namespace="Social.Generic.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Model.SearchType.ToString() %> Search Results
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaDescriptionHolder" runat="server">
	<%= UniversityOfMe.Helpers.MetaHelper.MetaDescription("Displaying all " + Model.SearchType.ToString() + "s") %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MetaKeywordsHolder" runat="server">
	<%= UniversityOfMe.Helpers.MetaHelper.MetaKeywords(Model.SearchType.ToString())%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<script>
    $(document).ready(function () {
        $("#search").bind("keydown", function (event) {
            // track enter key
            var keycode = (event.keyCode ? event.keyCode : (event.which ? event.which : event.charCode));
            if (keycode == 13) { // keycode for enter key
                
                var myType = $('.active-filter').attr('id');
                var mySearch = $('#search').val();
                var mySearchBy = $('#search-by').val();
                var myOrderBy = $('#order-by').val();
                if (!mySearchBy) { mySearchBy = "None"; }
                if (!myOrderBy) { myOrderBy = "None"; }
                $.post("/Search/DoAdvancedSearch", { searchType: myType, searchString: mySearch, page: 1, searchBy: mySearchBy, orderBy: myOrderBy });
                return false;
            } else {
                return true;
            }
        });
        $("#search").click(function () {
            <% if(string.IsNullOrEmpty(Model.SearchString)) { %>
                $("#search").val("");
            <% } %>
        });
    });

    var isShown = false;
    
    $(function () {
        $("a.search").click(function () {
            if ($("ul.options").is(":visible")) {
                $("div.select").css({ borderColor: "transparent", borderWidth: "0px", borderStyle: "none", top: "9px", right: "16px" });
                $("ul.options").slideUp("fast");
                isShown = false;
            } else {
                $("div.select").css({ borderColor: "#cdcdcd", borderWidth: "1px", borderStyle: "solid", top: "8px", right: "15px" });
                $("ul.options").slideDown("fast");
                isShown = true;
            }
            return false;
        });
        $("ul.options a").click(function () {
            $("ul.options li").removeClass("active-filter");
            $(this).parent("li").addClass("active-filter");

            return false;
        });
        $("body").click(function () {
            if (isShown) {
                $("div.select").css({ borderColor: "transparent", borderWidth: "0px", borderStyle: "none", top: "9px", right: "16px" });
                $("ul.options").slideUp("fast");
                isShown = false;
            }
        })
    });
</script> 

<div class="four"> 
	<div class="banner title"> 
		FILTER RESULTS
	</div> 
	<div class="box spec"> 
		<ul class="filter"> 
            <% string mySearchString = string.IsNullOrEmpty(Model.SearchString) ? string.Empty : Model.SearchString; %>
			<li class="<%= Model.SearchType == SearchFilter.ALL ? "active" : "" %>"> 
                <%= Html.ActionLink("Latest Marketplace Listings", "All", "Search", new { searchString = mySearchString, page = 1 }, new { @class = "all" })%>
			</li> 
    		<li class="<%= Model.SearchType == SearchFilter.CLASS ? "active" : "" %>"> 
                <%= Html.ActionLink("Current Classes", "Class", "Search", new { searchString = mySearchString, page = 1 }, new { @class = "class" })%>
			</li>
			<li class="<%= Model.SearchType == SearchFilter.TEXTBOOK ? "active" : "" %>"> 
                <%= Html.ActionLink("Textbook Listings", "Textbook", "Search", new { searchString = mySearchString, page = 1 }, new { @class = "book" })%>
			</li>
			<li class="<%= Model.SearchType == SearchFilter.APARTMENT ? "active" : "" %>">
                <%= Html.ActionLink("Apartment Listings", "Marketplace", "Search", new { searchString = mySearchString, page = 1, itemType = SearchFilter.APARTMENT.ToString() }, new { @class = "apartment" })%>
			</li> 
			<li class="<%= Model.SearchType == SearchFilter.CAR ? "active" : "" %>">
                <%= Html.ActionLink("Car Listings", "Marketplace", "Search", new { searchString = mySearchString, page = 1, itemType = SearchFilter.CAR.ToString() }, new { @class = "car" })%>
			</li>
			<li class="<%= Model.SearchType == SearchFilter.ELECTRONIC ? "active" : "" %>">
                <%= Html.ActionLink("Electronic Listings", "Marketplace", "Search", new { searchString = mySearchString, page = 1, itemType = SearchFilter.ELECTRONIC.ToString() }, new { @class = "electronic" })%>
			</li>
			<li class="<%= Model.SearchType == SearchFilter.FREE ? "active" : "" %>">
                <%= Html.ActionLink("Free Listings", "Marketplace", "Search", new { searchString = mySearchString, page = 1, itemType = SearchFilter.FREE.ToString() }, new { @class = "free" })%>
			</li>
			<li class="<%= Model.SearchType == SearchFilter.FURNITURE ? "active" : "" %>">
                <%= Html.ActionLink("Furniture Listings", "Marketplace", "Search", new { searchString = mySearchString, page = 1, itemType = SearchFilter.FURNITURE.ToString() }, new { @class = "furniture" })%>
			</li>
			<li class="<%= Model.SearchType == SearchFilter.GROUPON ? "active" : "" %>">
                <%= Html.ActionLink("Groupon Listings", "Marketplace", "Search", new { searchString = mySearchString, page = 1, itemType = SearchFilter.GROUPON.ToString() }, new { @class = "groupon" })%>
			</li>  
			<li class="<%= Model.SearchType == SearchFilter.MISCELLANEOUS ? "active" : "" %>">
                <%= Html.ActionLink("Miscellaneous Listings", "Marketplace", "Search", new { searchString = mySearchString, page = 1, itemType = SearchFilter.MISCELLANEOUS.ToString() }, new { @class = "miscellaneous" })%>
			</li>  
			<li class="<%= Model.SearchType == SearchFilter.SERVICE ? "active" : "" %>">
                <%= Html.ActionLink("Service Listings", "Marketplace", "Search", new { searchString = mySearchString, page = 1, itemType = SearchFilter.SERVICE.ToString() }, new { @class = "services" })%>
			</li>    
			<li class="<%= Model.SearchType == SearchFilter.ROOMATE ? "active" : "" %>">
                <%= Html.ActionLink("Roomate Listings", "Marketplace", "Search", new { searchString = mySearchString, page = 1, itemType = SearchFilter.ROOMATE.ToString() }, new { @class = "roomate" })%>
			</li> 
			<li class="<%= Model.SearchType == SearchFilter.VIDEOGAME ? "active" : "" %>">
                <%= Html.ActionLink("Videogame Listings", "Marketplace", "Search", new { searchString = mySearchString, page = 1, itemType = SearchFilter.VIDEOGAME.ToString() }, new { @class = "videogame" })%>
			</li>
    		<li class="<%= Model.SearchType == SearchFilter.WANTED ? "active" : "" %>">
                <%= Html.ActionLink("Wanted Listings", "Marketplace", "Search", new { searchString = mySearchString, page = 1, itemType = SearchFilter.WANTED.ToString() }, new { @class = "wanted" })%>
			</li>
			<li class="<%= Model.SearchType == SearchFilter.WORK ? "active" : "" %>">
                <%= Html.ActionLink("Work Listings", "Marketplace", "Search", new { searchString = mySearchString, page = 1, itemType = SearchFilter.WORK.ToString() }, new { @class = "work" })%>
			</li>
		</ul> 
	</div> 
</div> 
				
<div class="eight last"> 
	<div class="banner title black full red-top small clearfix"> 
		<span><%= Model.SearchType.ToString().ToUpper().Equals("ALL") ? "LATEST MARKETPLACE " : Model.SearchType.ToString().ToUpper()%> LISTINGS</span> 
		<div class="frgt max-w240 clearfix"> 
			<div class="search clearfix"> 
				<input id="search" type="text" class="inpt w227" value="<%= string.IsNullOrEmpty(Model.SearchString) ? SearchConstants.DEFAULT_SEARCH_TEXT : Model.SearchString %>" /> 
				<div class="select"> 
					<a href="#" class="search">Search</a> 
					<ul class="options">
            		    <li id="<%= SearchFilter.ALL %>" class="<%= Model.SearchType == SearchFilter.ALL ? "active-filter" : "" %>"> 
						    <a href="#" class="all">All</a> 
					    </li> 
					    <li id="<%= SearchFilter.TEXTBOOK %>" class="<%= Model.SearchType == SearchFilter.TEXTBOOK ? "active-filter" : "" %>"> 
						    <a href="#" class="book">Book</a> 
					    </li> 
            		    <li id="CLASS" class="<%= Model.SearchType == SearchFilter.CLASS ? "active-filter" : "" %>"> 
						    <a href="#" class="class">Class</a> 
					    </li> 
            		    <li id="<%= SearchFilter.APARTMENT %>" class="<%= Model.SearchType == SearchFilter.APARTMENT ? "active-filter" : "" %>"> 
						    <a href="#" class="apartment">Apartment</a> 
					    </li> 
            		    <li id="<%= SearchFilter.CAR %>" class="<%= Model.SearchType == SearchFilter.CAR ? "active-filter" : "" %>"> 
						    <a href="#" class="car">Car</a> 
					    </li> 
            		    <li id="<%= SearchFilter.ELECTRONIC %>" class="<%= Model.SearchType == SearchFilter.ELECTRONIC ? "active-filter" : "" %>"> 
						    <a href="#" class="electronic">Electronic</a> 
					    </li> 
            		    <li id="<%= SearchFilter.FREE %>" class="<%= Model.SearchType == SearchFilter.FREE ? "active-filter" : "" %>"> 
						    <a href="#" class="free">Free</a> 
					    </li> 
            		    <li id="<%= SearchFilter.FURNITURE %>" class="<%= Model.SearchType == SearchFilter.FURNITURE ? "active-filter" : "" %>"> 
						    <a href="#" class="furniture">Furniture</a> 
					    </li> 
            		    <li id="<%= SearchFilter.GROUPON %>" class="<%= Model.SearchType == SearchFilter.GROUPON ? "active-filter" : "" %>"> 
						    <a href="#" class="groupon">Groupon</a> 
					    </li> 
              		    <li id="<%= Model.SearchType == SearchFilter.MISCELLANEOUS %>" class="<%= Model.SearchType == SearchFilter.MISCELLANEOUS ? "active-filter" : "" %>"> 
						    <a href="#" class="roomate">Miscellaneous</a> 
					    </li> 
            		    <li id="<%= SearchFilter.ROOMATE %>" class="<%= Model.SearchType == SearchFilter.ROOMATE ? "active-filter" : "" %>"> 
						    <a href="#" class="roomate">Roomate</a> 
					    </li> 
            		    <li id="<%= SearchFilter.SERVICE %>" class="<%= Model.SearchType == SearchFilter.SERVICE ? "active-filter" : "" %>"> 
						    <a href="#" class="services">Service</a> 
					    </li> 
            		    <li id="<%= SearchFilter.VIDEOGAME %>" class="<%= Model.SearchType == SearchFilter.VIDEOGAME ? "active-filter" : "" %>"> 
						    <a href="#" class="videogame">Videogame</a> 
					    </li>
            		    <li id="<%= SearchFilter.WANTED %>" class="<%= Model.SearchType == SearchFilter.WANTED ? "active-filter" : "" %>"> 
						    <a href="#" class="wanted">Wanted</a> 
					    </li> 
            		    <li id="<%= SearchFilter.WORK %>" class="<%= Model.SearchType == SearchFilter.WORK ? "active-filter" : "" %>"> 
						    <a href="#" class="work">Work</a> 
					    </li> 
					</ul> 
				</div> 
			</div> 
		</div> 
	</div> 
    <% if (Model.SearchByOptions.Count() != 0 && Model.OrderByOptions.Count() != 0) { %>
	    <div class="box filter mb0"> 
            <div style="float:left">
                <% string myUniversityId = string.Empty; %>
                <% if(UserInformationFactory.IsLoggedIn()) { %>
                    <% UserInformationModel<User> myUserInfo = UserInformationFactory.GetUserInformation(); %>
                    <% myUniversityId = UniversityHelper.GetMainUniversity(myUserInfo.Details).Id; %>
                <% } %>
                <% if (Model.SearchType == SearchFilter.CLASS || Model.SearchType == SearchFilter.TEXTBOOK) { %>
                    <%= Html.ActionLink("Create a Textbook Listing", "Create", "Textbook", new { universityId = myUniversityId }, new { @class = "itemlinked" })%>
                <% } else { %>
                    <%= Html.ActionLink("Create a Listing", "Create", "Marketplace", new { universityId = myUniversityId }, new { @class = "itemlinked" })%>
                <% } %>
            </div>
		    <div class="input"> 
			    <label for="search-by">Search By</label> 
                <%= Html.DropDownList("search-by", Model.SearchByOptions)%>
		    </div> 
		    <div class="input"> 
			    <label for="order-by">Order By</label> 
                <%= Html.DropDownList("order-by", Model.OrderByOptions)%>
		    </div> 
	    </div>
    <% } %> 
	<div class="search result"> 
        <% foreach (ISearchResult mySearchResult in Model.SearchResults) { %>
            <div class="res"> 
                <%= mySearchResult.CreateResult() %>
            </div>
        <% } %>       		
	</div> 
	<div class="pagination"> 
		<ul> 
            <% string mySearchUrl = "/Search/" + Model.SearchType.ToString() + "?searchString=" + Model.SearchString + "&page="; %>
            <% if (Model.CurrentPage == 1) { %>
			    <li class="prev-off">&laquo; Previous</li> 
            <% } else { %>
                <li class="prev-off">
                    <a href="<%= mySearchUrl + (Model.CurrentPage - 1) %>">&laquo; Previous</a>
                </li> 
            <% } %>
            <% int myLowerBound = Model.CurrentPage - SiteConfiguration.PagesPadding() < 1 ? 1 : Model.CurrentPage - SiteConfiguration.PagesPadding(); %>
            <% int myUpperBound = Model.CurrentPage + SiteConfiguration.PagesPadding() > Model.TotalPages ? Model.TotalPages : Model.CurrentPage + SiteConfiguration.PagesPadding(); %>
            <% for (int myCounter = myLowerBound; myCounter <= myUpperBound; myCounter++) { %>
                <% if (myCounter == Model.CurrentPage) { %>
                    <li class="active"><%= myCounter %></li> 
                <% } else { %>
			        <li> 
				        <a href="<%= mySearchUrl + myCounter %>"><%= myCounter %></a> 
			        </li>
                <% } %>
            <% } %>
            <% if (Model.CurrentPage < Model.TotalPages) { %>
			    <li class="next-on"> 
				    <a href="<%= mySearchUrl + (Model.CurrentPage + 1) %>">Next &raquo;</a> 
			    </li> 
            <% }  %>
		</ul> 
	</div> 
</div>
</asp:Content>

