<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HaveAVoice.Models.View.NavigationModel>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.Enums" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="Social.Generic.Models" %>
<%@ Import Namespace="Social.Generic.Constants" %>
<%@ Import Namespace="HaveAVoice.Helpers.Profile" %>

<link rel="stylesheet" type="text/css" href="/Content/jquery.autocomplete.css" />

<script type="text/javascript" language="javascript" src="/Content/jquery.autocomplete.js"></script>

<script type="text/javascript">
	$(function() {
		$("a.search").click(function() {
			if ($(".pnl").is(":visible")) {
				$(".pnl").hide();
				$(this).parent().removeClass("selected");
			} else {
				$(".pnl").show();
				$(this).parent().addClass("selected");
			}
			return false;
		});
		
		$("a.people").click(function() {
			$(this).addClass("selected");
			$("a.issue").removeClass("selected");
			$("#SearchType").val("User");
			$("#SearchQuery").unautocomplete();
			$("#SearchQuery").focus().autocomplete("/Search/getUserAjaxResult");
			
			return false;
		});
		
		$("a.issue").click(function() {
			$(this).addClass("selected");
			$("a.people").removeClass("selected");
			$("#SearchType").val("Issues");
			$("#SearchQuery").unautocomplete();
			$("#SearchQuery").focus().autocomplete("/Search/getIssueAjaxResult");
			
			return false;
		});
		
		if ($("a.people").hasClass("selected")) {
			$("#SearchQuery").unautocomplete();
			$("#SearchQuery").focus().autocomplete("/Search/getUserAjaxResult");
		} else {
			$("#SearchQuery").unautocomplete();
			$("#SearchQuery").focus().autocomplete("/Search/getIssueAjaxResult");
		}
	});
</script>

<div class="col-24 user-panel">
    <% UserInformationModel<User> myUser = HAVUserInformationFactory.GetUserInformation(); %>
	<div class="col-3 center">
		<a href="/<%= Model.User.ShortUrl %>"><img src="<%= Model.ProfilePictureUrl %>" alt="<%= Model.FullName %>" class="profile" /></a>
	</div>
	<div class="col-15">
		<div class="m-lft col-15 m-rgt">
          
            <%= NavigationHelper.UserNavigation(Model.SiteSection, Model.UserMenuMetaData, Model.User) %>
            <% if (myUser != null && myUser.Details.Id == Model.User.Id) { %>
                <ul class="f-rgt fnt-12">
            	    <li class="search">
            		    <a href="#" class="search">Search</a>
            		    <div class="pnl">
            			    <% using (Html.BeginForm("DoSearch", "Search")) { %>
	            			    <a class="people selected" href="#" alt="Search users">Users</a>
	            			    <input type="text" name="SearchQuery" id="SearchQuery" />
	            			    <a class="issue" href="#" alt="Search issues">Issues</a>
	            			    <select id="SearchType" name="SearchType">
	            				    <option value="User" selected="selected">User</option>
	            				    <option value="Issues">Issues</option>
	            			    </select>
							    <div class="clear">&nbsp;</div>
							    <div class="right">
								    <input type="submit" class="button" value="Search" />
							    </div>
	            		    <% } %>
            		    </div>
            	    </li>
                </ul>
            <% } %>
            <div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>

		<div class="m-lft col-15 m-rgt user-control">
			<div class="m-lft10">
                <h1>
                    <% if (myUser != null && myUser.Details.Id == Model.User.Id) { %>
                        Hello,
                    <% } %>
				    <%= Model.FullName %>
			    </h1>
            </div>
            <% if (myUser != null) { %>
                <% if (Model.SiteSection == SiteSection.Profile) { %>
                    <% if (!FriendHelper.IsFriend(Model.User, myUser.Details)) { %>
            	        <div class="col-6 m-top10">
            	    	    <% using (Html.BeginForm("Add", "Friend", new { id = Model.User.Id })) { %>
				                <input type="submit" class="fan" value="Become a Friend" />
			                <% } %>
			                <div class="clear">&nbsp;</div>
		                </div>
		                <div class="clear">&nbsp:</div>
                    <% } %>
                    <% if (myUser.Details.Id != Model.User.Id) { %>
			            <a class="p-v5 m-top10 msg" href="/Message/Create/<%= Model.User.Id %>">Send <%= Model.User.FirstName%> a private message</a>
			        <% } %>
                <% } %>
            <% } %>
            <div class="clear">&nbsp;</div>
        	<% if(Model.SiteSection == SiteSection.MyProfile) { %>
				<div class="m-btm5 m-top20 m-lft col-14 m-rgt local">
                    <% if (Model.QuickNavigation == QuickNavigation.LocalIssue) { %>
					    <div class="p-a5">
						    <h4>Issues In Your <%= Model.LocalIssueLocation%></h4>
						    <div class="clear">&nbsp;</div>
						    Resident <a href="<%= LinkHelper.Profile(Model.LocalIssue.User) %>" class="name"><%= NameHelper.FullName(Model.LocalIssue.User)%></a> says, &quot;<%= Model.LocalIssue.Description%>&quot;<br />
                            In <%= Model.LocalIssue.User.Gender.ToUpper().Equals(Gender.MALE.ToUpper()) ? "his" : "her"%> issue: <b><a href="<%= LinkHelper.IssueUrl(Model.LocalIssue.Title) %>" class="issueregarding"><%= Model.LocalIssue.Title%></a></b>.
			            </div>
                    <% } else if(Model.QuickNavigation == QuickNavigation.SuggestedFriend) { %>
					    <div class="p-a5">
						    <h4>Friend Suggestion</h4>
						    <div class="clear">&nbsp;</div>
                            <div class="col-13">
                                <div class="col-10">
                                    Why not friend <a class="name" href="<%= LinkHelper.Profile(Model.FriendConnectionModel.User) %>">
                                        <%= NameHelper.FullName(Model.FriendConnectionModel.User) %>
                                    </a>?<br />Suggestion based off the question 
                                    <span class="italic">
                                        <%= Model.FriendConnectionModel.QuestionConnectionMadeFrom.DisplayQuestion %>
                                    </span>
                                </div>
                                <div class="col-3">
    		                        <div class="col-2 center">
                                        <a class="button" href="<%= LinkHelper.AddFriend(Model.FriendConnectionModel.User, "Profile", "Show") %>">Add Friend</a>
    			                        <div class="clear">&nbsp;</div>
    		                        </div>
    		                        <div class="col-1 center">
                                        <a class="button" href="<%= LinkHelper.IgnoreFriendSuggestion(Model.FriendConnectionModel.User.Id, "Profile", "Show") %>">Ignore</a>
    			                        <div class="clear">&nbsp;</div>
    		                        </div>
                                </div>
                            </div>
			            </div>
                    <% } else if (Model.QuickNavigation == QuickNavigation.IssueTip) { %>
                        <div class="p-a5">
                            <h4>Issue Tip</h4>
                            <div class="clear">&nbsp;</div>
                            There are no issues in your area. Why not <a class="name" href="<%= LinkHelper.CreateIssue() %>">raise a new issue</a>?
                        </div>
                    <% } else if (Model.QuickNavigation == QuickNavigation.SuggestFriendTip) { %>
                        <div class="p-a5">
                            <h4>Friend Suggestion Tip</h4>
                            <div class="clear">&nbsp;</div>
                            To find more people with similar interests, make sure to fill out your <a class="name" href="<%= LinkHelper.UserProfileQuestions() %>">profile questionnaire</a>.
                        </div>
                    <% } %>
			        <div class="clear">&nbsp;</div>
				</div>
			<% } %>
		</div>
	</div>
    <% bool myIsAllowed = PrivacyHelper.IsAllowed(Model.User, PrivacyAction.DisplayProfile); %>
    <% if (myIsAllowed) { %>
        <div class="col-6 round-3">
	        <div class="p-a10">
		        <h6 class="m-btm5"><% if (myUser != null && myUser.Details.Id == Model.User.Id) { %>My <% } %>Stats</h6>
		        <div class="col-1 teal fnt-14">
                    <% int myIssueCount = Model.User.Issues.Count; %>
                    <%= myIssueCount%>
                </div>
                <div class="col-4 c-white fnt-14">
                    <% if (myIssueCount == 1) { %>
                        Issue Raised
                    <% } else { %>
                        Issues Raised
                    <% } %>
                </div>
		        <div class="clear">&nbsp;</div>
		        <div class="col-6 fnt-12 p-v5">
		            <span class="green"><%= IssueDispositionHelper.NumberOfDisposition(Model.User, Disposition.Like)%> agrees</span>
		            <span class="teal m-lft10 m-rgt10">|</span>
		            <span class="red"><%= IssueDispositionHelper.NumberOfDisposition(Model.User, Disposition.Dislike)%> disagrees</span>
                </div>
                <div class="col-1 teal fnt-14">
                    <%  int myIssueReplyCount = Model.User.IssueReplys.Count; %>
            	    <%= myIssueReplyCount%>
            	    <div class="clear">&nbsp;</div>
                </div>
                <div class="col-4 c-white fnt-14">
                    <% if (myIssueReplyCount == 1) { %>
                        Idea Added
                    <% } else { %>
                        Ideas Added
                    <% } %>
            	    <div class="clear">&nbsp;</div>
                </div>
                <div class="clear">&nbsp;</div>
	            <div class="spacer-5">&nbsp;</div>

	            <div class="col-1 teal fnt-14">
                    <% int myFriends = Model.User.FriendedBy.Count; %>
		            <%= myFriends%>
		            <div class="clear">&nbsp;</div>
	            </div>
	            <div class="col-1 c-white fnt-14">
                    <% if (myFriends == 1) { %>
                        Friend
                    <% } else { %>
		                Friends
                    <% } %>
		            <div class="clear">&nbsp;</div>
	            </div>
        	    <div class="clear">&bnsp;</div>
	        </div>
	        <div class="clear">&nbsp;</div>
        </div>
    <% } %>
</div>
<div class="clear">&nbsp;</div>