<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<UniversityOfMe.Models.View.LeftNavigation>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Search" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<script>
    $(document).ready(function () {
        var myBg = <%= "'" + Model.BackgroundImage + "'" %>;
        var myBgStyle = 'url(/Content/images/' + myBg + ')';
        $('body').css('background-image', myBgStyle);
        $("#submitPhoto").hide();
        $('#addphoto').click(function () {
            $("#newphotos").show();
        });
        $("#newphotos").hide();

        $("#datingtooltip a[title]").tooltip();
        $("#search").bind("keydown", function (event) {
            // track enter key
            var keycode = (event.keyCode ? event.keyCode : (event.which ? event.which : event.charCode));
            if (keycode == 13) { // keycode for enter key
                var myType = $('.active-filter').attr('id');
                var mySearch = $('#search').val();
                $.post("/Search/DoSearch", { searchType: myType, searchString: mySearch, page: 1 });
                return false;
            } else {
                return true;
            }
        });
        var myAlreadyCleared = 0;
        $("#search").click(function () {
            if(myAlreadyCleared == 0) {
                $("#search").val("");
                myAlreadyCleared = 1;
            }
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
    <% if (Model.IsLoggedIn) { %>
	    <div class="banner full mb72"> 
        
		    <div class="search clearfix"> 
			    <input id="search" type="text" class="inpt" value="type something and hit enter" /> 
			    <div class="select"> 
				    <a href="#" class="search">Search</a> 
				    <ul class="options"> 
					    <li id="<%= SearchFilter.ALL %>" class="active-filter"> 
						    <a href="#" class="all">All</a> 
					    </li> 
					    <li id="<%= SearchFilter.USER %>"> 
						    <a href="#" class="people"><%= SearchFilter.USER.ToString()%></a> 
					    </li> 
					    <li id="<%= SearchFilter.TEXTBOOK %>"> 
						    <a href="#" class="text"><%= SearchFilter.TEXTBOOK.ToString()%></a> 
                        </li>
					    <li id="<%= SearchFilter.APARTMENT %>"> 
						    <a href="#" class="apartment"><%= SearchFilter.APARTMENT.ToString()%></a> 
                        </li>
					    <li id="<%= SearchFilter.VIDEOGAME %>"> 
						    <a href="#" class="videogame"><%= SearchFilter.VIDEOGAME.ToString()%></a> 
                        </li>
					    <li id="<%= SearchFilter.GROUPON %>"> 
						    <a href="#" class="groupon"><%= SearchFilter.GROUPON.ToString()%></a> 
                        </li>
				    </ul> 
			    </div>
		    </div> 
		    <span class="corner"></span> 
	    </div> 

        <% if (!Model.HasProfilePicture) { %>
            <div class="banner title"> 
		        U of Me PRO TIP
	        </div> 
	        <div class="box"> 
                <div>
                    You have no profile picture! <a id="addphoto" class="itemlinked" href="#">Click here to upload one.</a>	
                    <div id="newphotos">
		                <div class="mb25 center"> 
                            <% using (Html.BeginForm("CreateProfilePicture", "Photo", FormMethod.Post, new { enctype = "multipart/form-data" })) { %>
		                        <input type="file" name="profileFile" id="profileFile" class="right" /> 
		                        <input type="submit" name="upload" class="btn site" value="Upload" /> 
                            <% } %>
		                </div> 
                    </div>
                </div>
	        </div> 
        <% } %>
    <% } else { %>
	    <div class="banner title"> 
		    REGISTER AN ACCOUNT
	    </div> 
	    <div class="box"> 
            <a class="itemlinked" href="/User/Create">Click here to create a new account</a><br />
            We are currently open to the following schools:<br />
            <% IEnumerable<University> myUniversities = UniversityHelper.ValidUniversities(); %>
            <% foreach (University myUniversity in myUniversities) { %>
                <div style="margin-top:-5px">
                    <%= myUniversity.UniversityName%>
                </div>
            <% } %>
	    </div> 
    <% } %>

    <div class="banner title"> 
		Marketplace Catagories
	</div> 
	<div class="box"> 
        <div>
            <a class="itemlinked" href="<%= URLHelper.SearchTextbooksByClass() %>">Textbooks</a><br />
            <% foreach (ItemType myItemType in Model.ItemTypes) { %>
                <a class="itemlinked" href="<%= URLHelper.SearchMarketplace(myItemType) %>"><%= myItemType.DisplayName %>s</a>
                <br />
            <% } %>
        </div>
	</div> 
</div> 