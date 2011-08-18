<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<Friend>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="Social.ViewHelpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= UOMConstants.TITLE %> = Your Friends
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" language="javascript">
        //Original code from: http://kilianvalkhof.com/2010/javascript/how-to-build-a-fast-simple-list-filter-with-jquery/
        (function ($) {
            // custom css expression for a case-insensitive contains()
            jQuery.expr[':'].Contains = function (a, i, m) {
                return (a.textContent || a.innerText || "").toUpperCase().indexOf(m[3].toUpperCase()) >= 0;
            };


            function listFilter(header, list) { // header is any element, list is an unordered list
                // create and add the filter form to the header
                var form = $("<form>").attr({ "class": "filterform", "action": "#" }),
                input = $("<input>").attr({ "class": "filterinput", "type": "text", "value": "Filter by name" });
                $(form).append(input).appendTo(header);
                $(input).click(function () {
                    input.val("");
                })
                $(input).change(function () {
                    var filter = $(this).val();
                    if (filter) {
                        // this finds all links in a list that contain the input,
                        // and hide the ones not containing the input while showing the ones that do
                        $(list).find("div:not(:Contains(" + filter + "))").parent().hide();
                        $(list).find("div:Contains(" + filter + ")").parent().show();
                    } else {
                        $(list).find("li").show();
                    }
                    return false;
                })
        .keyup(function () {
            // fire the above change event after every letter
            $(this).change();
        });
            }


            //ondomready
            $(function () {
                listFilter($("#header"), $("#list"));
            });
        } (jQuery));
    </script>

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="banner black full red-top"> 
			FRIENDS
			<div id="header" class="buttons"> 
				<div class="clearfix"></div> 
			</div> 
		</div> 
		
        <ul id="list" class="friend-list clearfix"> 
            <% foreach (Friend myFriend in Model.Get()) { %>
			    <li> 
				    <div id="<%= myFriend.FriendedUser.FirstName %>"> 
					    <a href="<%= URLHelper.ProfileUrl(myFriend.FriendedUser) %>"><img src="<%= PhotoHelper.ProfilePicture(myFriend.FriendedUser) %>" class="profile lrg flft mr9" /></a>
					    <span class="name"><%= NameHelper.FullName(myFriend.FriendedUser) %></span>
					    <%= UniversityHelper.GetMainUniversity(myFriend.FriendedUser).UniversityName %><br /> 
					    <span class="red">&nbsp;</span> 
					    <a href="/Message/Create/<%= myFriend.FriendedUser.Id %>" class="frgt mail">Mail</a> 
				    </div> 
			    </li> 
            <% } %>
		</ul> 
	</div>

    

</asp:Content>

