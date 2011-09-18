<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SearchResultsModel>" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Constants" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Search" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Configuration" %>
<%@ Import Namespace="UniversityOfMe.Models.View.Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Model.SearchType.ToString() %> Search Results
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
			<li class="<%= Model.SearchType == SearchFilter.All ? "active" : "" %>"> 
                <%= Html.ActionLink("All Search Results", "All", "Search", new { searchString = mySearchString, page = 1 }, new { @class = "all" })%>
			</li> 
			<li class="<%= Model.SearchType == SearchFilter.User ? "active" : "" %>"> 
				<%= Html.ActionLink("User Search Results", "User", "Search", new { searchString = mySearchString, page = 1 }, new { @class = "friend" })%>
			</li> 
			<li class="<%= Model.SearchType == SearchFilter.Professor ? "active" : "" %>"> 
                <%= Html.ActionLink("Professor Search Results", "Professor", "Search", new { searchString = mySearchString, page = 1 }, new { @class = "prof" })%>
			</li> 
			<li class="<%= Model.SearchType == SearchFilter.Class ? "active" : "" %>"> 
				<%= Html.ActionLink("Class Search Results", "Class", "Search", new { searchString = mySearchString, page = 1 }, new { @class = "class" })%>
			</li> 
			<li class="<%= Model.SearchType == SearchFilter.Event ? "active" : "" %>"> 
				<%= Html.ActionLink("Event Search Results", "Event", "Search", new { searchString = mySearchString, page = 1 }, new { @class = "event" })%>
			</li> 
			<li class="<%= Model.SearchType == SearchFilter.Textbook ? "active" : "" %>"> 
                <%= Html.ActionLink("Textbook Search Results", "Textbook", "Search", new { searchString = mySearchString, page = 1 }, new { @class = "book" })%>
			</li> 
			<li class="<%= Model.SearchType == SearchFilter.Organization ? "active" : "" %>"> 
                <%= Html.ActionLink("Club Search Results", "Organization", "Search", new { searchString = mySearchString, page = 1 }, new { @class = "org" })%>
			</li> 
			<li class="<%= Model.SearchType == SearchFilter.GeneralPosting ? "active" : "" %>"> 
                <%= Html.ActionLink("General Posting Search Results", "GeneralPosting", "Search", new { searchString = Model.SearchString, page = 1 }, new { @class = "post" })%>
			</li> 
		</ul> 
	</div> 
</div> 
				
<div class="eight last"> 
	<div class="banner title black full red-top small clearfix"> 
		<span><%= Model.SearchType.ToString().ToUpper() %> SEARCH RESULTS</span> 
		<div class="frgt max-w240 clearfix"> 
			<div class="search clearfix"> 
				<input id="search" type="text" class="inpt w227" value="<%= string.IsNullOrEmpty(Model.SearchString) ? SearchConstants.DEFAULT_SEARCH_TEXT : Model.SearchString %>" /> 
				<div class="select"> 
					<a href="#" class="search">Search</a> 
					<ul class="options">
					    <li id="<%= SearchFilter.All %>" class="<%= Model.SearchType == SearchFilter.All ? "active-filter" : "" %>"> 
						    <a href="#" class="all">All</a> 
					    </li> 
					    <li id="<%= SearchFilter.User %>" class="<%= Model.SearchType == SearchFilter.User ? "active-filter" : "" %>"> 
						    <a href="" class="people">People</a> 
					    </li> 
					    <li id="<%= SearchFilter.Professor %>" class="<%= Model.SearchType == SearchFilter.Professor ? "active-filter" : "" %>"> 
						    <a href="#" class="professor">Professors</a> 
					    </li> 
					    <li id="<%= SearchFilter.Class %>" class="<%= Model.SearchType == SearchFilter.Class ? "active-filter" : "" %>"> 
						    <a href="#" class="case">Classes</a> 
					    </li> 
					    <li  id="<%= SearchFilter.Event %>" class="<%= Model.SearchType == SearchFilter.Event ? "active-filter" : "" %>"> 
						    <a href="#" class="cal">Events</a> 
					    </li> 
					    <li id="<%= SearchFilter.Textbook %>" class="<%= Model.SearchType == SearchFilter.Textbook ? "active-filter" : "" %>"> 
						    <a href="#" class="text">Book</a> 
					    </li> 
					    <li id="<%= SearchFilter.GeneralPosting %>" class="<%= Model.SearchType == SearchFilter.GeneralPosting ? "active-filter" : "" %>"> 
						    <a href="#" class="paper">Paper</a> 
					    </li> 
					    <li id="<%= SearchFilter.Organization %>" class="<%= Model.SearchType == SearchFilter.Organization ? "active-filter" : "" %>">  
						    <a href="#" class="org">Organizations</a> 
					    </li> 
					</ul> 
				</div> 
			</div> 
		</div> 
	</div> 
    <% if (Model.SearchByOptions.Count() != 0 && Model.OrderByOptions.Count() != 0) { %>
	    <div class="box filter mb0"> 
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
            <% string mySearchUrl = "/Search/All?searchString=" + Model.SearchString + "&page="; %>
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

