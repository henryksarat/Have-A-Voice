<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<ClassDetailsModel>>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="Social.Generic.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Functionality" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <% ClassViewType myViewType = (ClassViewType)ViewData["ClassViewType"]; %>
    <% if (myViewType == ClassViewType.Review) { %>
	    Class Review | <%= ClassHelper.CreateClassString(Model.Get().Class) %> | <%= Model.Get().Class.Title%> | <%= Model.Get().Class.University.UniversityName%>
    <% } else { %>
        Class Discussion | <%= ClassHelper.CreateClassString(Model.Get().Class)%> | <%= Model.Get().Class.Title%> | <%= Model.Get().Class.University.UniversityName%>
    <% } %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaDescriptionHolder" runat="server">
    <% ClassViewType myViewType = (ClassViewType)ViewData["ClassViewType"]; %>
    <% if (myViewType == ClassViewType.Review) { %>
        <%= UniversityOfMe.Helpers.MetaHelper.MetaDescription("Class Review for " + Model.Get().Class.Title + " (" + ClassHelper.CreateClassString(Model.Get().Class) + ") at " + Model.Get().Class.University.UniversityName)%>
    <% } else { %>
        <%= UniversityOfMe.Helpers.MetaHelper.MetaDescription("Class Discussion for " + Model.Get().Class.Title + " (" + ClassHelper.CreateClassString(Model.Get().Class) + ") at " + Model.Get().Class.University.UniversityName)%>
    <% } %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MetaKeywordsHolder" runat="server">
    <% ClassViewType myViewType = (ClassViewType)ViewData["ClassViewType"]; %>
    <% if (myViewType == ClassViewType.Review) { %>
        <%= UniversityOfMe.Helpers.MetaHelper.MetaKeywords("Class Review, " + ClassHelper.CreateClassString(Model.Get().Class) + ", " + Model.Get().Class.Title + ", " + Model.Get().Class.University.UniversityName)%>
    <% } else { %>
        <%= UniversityOfMe.Helpers.MetaHelper.MetaKeywords("Class Discussion, " + ClassHelper.CreateClassString(Model.Get().Class) + ", " + Model.Get().Class.Title + ", " + Model.Get().Class.University.UniversityName)%>
    <% } %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        $("#stars-wrapper1").stars();
        <% foreach (ClassReview myReview in Model.Get().Class.ClassReviews.OrderByDescending(b => b.Id)) { %>
            $("#Div<%= myReview.Id %>").stars({
                disabled: true
            });
        <% } %>
    });
</script>

    <% UserInformationModel<User> myUserInfo = UserInformationFactory.GetUserInformation(); %>
    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>
    <% ClassViewType myViewType = (ClassViewType)ViewData["ClassViewType"]; %>
    
    <div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>
	    <div class="banner black full red-top small"> 
		    <span class="class">CLASS <%= ClassHelper.CreateClassString(Model.Get().Class)%></span> 
		    <div class="buttons"> 
			    <div class="flft mr26"> 
                    <% if (ClassHelper.IsEnrolled(UniversityOfMe.UserInformation.UserInformationFactory.GetUserInformation(), Model.Get().Class)) { %>
                        <%= Html.ActionLink("Remove me from this class", "Delete", "ClassEnrollment", new { classId = Model.Get().Class.Id, classViewType = myViewType }, new { @class = "unroll" })%>
                    <% } else { %>
                        <%= Html.ActionLink("I'm enrolled in this class", "Create", "ClassEnrollment", new { classId = Model.Get().Class.Id, classViewType = myViewType }, new { @class = "enroll" })%>
                    <% } %> 
			    </div> 
			    <div class="rating"> 
                    <% int myTotalRatings = Model.Get().Class.ClassReviews.Count; %>
				    <%= StarHelper.AveragedFiveStarImages(Model.Get().Class.ClassReviews.Sum(r => r.Rating), Model.Get().Class.ClassReviews.Count)%>
				    (<%= myTotalRatings %> ratings)
			    </div> 
		    </div> 
	    </div> 
					
	    <table border="0" cellpadding="0" cellspacing="0" class="listing mb32"> 
		    <tr> 
			    <td> 
				    <label for="code">CLASS:</label> 
			    </td> 
			    <td> 
				    <%= ClassHelper.CreateClassString(Model.Get().Class)%>
			    </td> 
		    </tr> 
		    <tr> 
			    <td> 
				    <label for="title">CLASS TITLE:</label> 
			    </td> 
			    <td> 
				    <%= Model.Get().Class.Title%>
			    </td> 
		    </tr> 
		    <tr> 
			    <td> 
				    <label for="title"><%= Model.Get().Professors.Count() > 0 ? "INSTRUCTORS" : "INSTRUCTOR" %></label> 
			    </td> 
			    <td> 
                    <% string myProfessorDisplay = string.Empty; %>
				    <% foreach(Professor myProfessor in Model.Get().Professors) { %>
                        <% if(!string.IsNullOrEmpty(myProfessorDisplay)) { %>
                            <% myProfessorDisplay += ", "; %>
                        <% } %>
                        <% myProfessorDisplay = "<a class=\"itemlinked\" href=\"" + URLHelper.BuildProfessorUrl(myProfessor) + "\">" + myProfessor.FirstName + " " + myProfessor.LastName + "</a>"; %>
                    <% } %>

                    <%= myProfessorDisplay %>
			    </td> 
		    </tr> 
		    <tr> 
			    <td> 
				    <label for="year">YEAR:</label> 
			    </td> 
			    <td> 
				    <%= Model.Get().Class.Year%>
			    </td> 
		    </tr> 
		    <tr> 
			    <td> 
				    <label for="academic">ACADEMIC TERM:</label> 
			    </td> 
			    <td> 
				    <%= Model.Get().Class.AcademicTerm.DisplayName%>
			    </td> 
		    </tr> 
	    </table> 
					
	    <div class="banner title"> 
		    CURRENT MEMBERS ENROLLED
	    </div> 
	    <div class="box sm group"> 
            <% bool myAtLeastOneStudent = false; %>
		    <ul class="members"> 
                <% foreach (ClassEnrollment myEnrollment in Model.Get().Class.ClassEnrollments) { %>
			        <% if (PrivacyHelper.PrivacyAllows(myEnrollment.User, Social.Generic.Helpers.SocialPrivacySetting.Display_Class_Enrollment)) { %>
                        <% myAtLeastOneStudent = true; %>
                        <li> 
				            <a href="/<%= Model.Get().Class.User.ShortUrl %>"><img src="<%= PhotoHelper.ProfilePicture(myEnrollment.User) %>" class="profile med flft mr9" /></a>
				            <span class="name"><%= NameHelper.FullName(myEnrollment.User)%></span> 
				            Student
			            </li>
                    <% } %>
                <% } %>
                <% if (!myAtLeastOneStudent) { %>
                    There are no students currently enrolled in this class
                <% } %>
 		    </ul> 
            <% if (myAtLeastOneStudent) { %>
		        <%= Html.ActionLink("View all members", "List", "ClassEnrollment", new { id = Model.Get().Class.Id }, new { @class = "viewall" })%>
            <% } %>
		    <div class="clearfix"></div> 
	    </div> 
					
	    <div class="tabs"> 
		    <ul> 
			    <li class="<%= myViewType == ClassViewType.Discussion ? "active" : ""  %>"> 
                    <a href="<%= URLHelper.BuildClassDiscussionUrl(Model.Get().Class) %>">Class Board</a> 
			    </li> 
			    <li class="<%= myViewType == ClassViewType.Review ? "active" : ""  %>"> 
				    <a href="<%= URLHelper.BuildClassReviewUrl(Model.Get().Class) %>">Class Reviews</a> 
			    </li> 
			    <li>
				    <a href="<%= URLHelper.SearchTextbooks(Model.Get().Class.Subject, Model.Get().Class.Course) %>">Search Textbooks</a> 
			    </li> 
			    <li> 
				    <a href="<%= URLHelper.CreateTextBook(Model.Get().Class.UniversityId, Model.Get().Class.Subject, Model.Get().Class.Course) %>">Sell your Textbook</a> 
			    </li> 
		    </ul> 
	    </div> 
					
	    <div id="review"> 
            <% if (myViewType == ClassViewType.Review) { %>
                <% if (myUserInfo != null) { %>
                    <% using (Html.BeginForm("Create", "ClassReview")) {%>
                        <%= Html.Hidden("ClassId", Model.Get().Class.Id)%>
		                <div class="create"> 
                            <%= Html.TextArea("Review", string.Empty, 6, 0, new { @class = "full" })%>
                            <%= Html.ValidationMessage("Review", "*")%>
							
			                <div class="frgt mt13"> 
				                <input type="submit" class="frgt btn site" name="post" value="Post Review" /> 
				                <div class="rating mr17"> 
	                                <div id="stars-wrapper1">
		                                <input type="radio" name="rating" value="1" title="Very poor" />
		                                <input type="radio" name="rating" value="2" title="Poor" />
		                                <input type="radio" name="rating" value="3" title="Not that bad" />
		                                <input type="radio" name="rating" value="4" title="Fair" />
		                                <input type="radio" name="rating" value="5" title="Average" />
	                                </div>

                                    <%= Html.ValidationMessage("Rating", "*")%>
				                </div> 

                                <%= Html.CheckBox("Anonymous", false, new { @class = "frgt pt3 mr20" })%>
                                <label class="post" for="Anonymous">Post as anonymous</label> 
			                </div> 
			                <div class="clearfix"></div> 
		                </div> 
                    <% } %>
                <% } %>
            <% } else if (myViewType == ClassViewType.Discussion) { %>
                <% if (ClassHelper.IsEnrolled(UserInformationFactory.GetUserInformation(), Model.Get().Class)) { %>
                    <% using (Html.BeginForm("Create", "ClassBoard")) {%>
                        <%= Html.Hidden("ClassId", Model.Get().Class.Id)%>
		                <div class="create"> 
                            <%= Html.TextArea("BoardMessage", string.Empty, 6, 0, new { @class = "full" })%>
                            <%= Html.ValidationMessage("BoardMessage", "*")%>
			                <div class="frgt mt13"> 
				                <input type="submit" class="frgt btn site" name="post" value="Post To Discussion" /> 
                            </div>
                            <div class="clearfix"></div> 
		                </div> 
                    <% } %>
                <% } else { %>
                    To post to the class board you must enroll in the class.
                <% } %>
            <% } %>
						
		    <div class="clearfix"></div> 
						
		    <div class="review"> 
			    <table border="0" cellpadding="0" cellspacing="0"> 
                    <% if (myViewType == ClassViewType.Review) { %>
                        <% if (Model.Get().Class.ClassReviews.Count == 0) { %>
                            There are no reviews
                        <% } %>
                        <% foreach (ClassReview myReview in Model.Get().Class.ClassReviews.OrderByDescending(b => b.Id)) { %>
            	            <tr> 
					            <td class="avatar"> 
                                    <% if (!myReview.Anonymous) { %>
						                <a href="/<%= myReview.User.ShortUrl %>"><img src="<%= PhotoHelper.ProfilePicture(myReview.User) %>" class="profile big mr22" /></a>
                                    <% } else { %>
                                        <img src="<%= PhotoHelper.AnonymousProfilePicture() %>" class="profile big mr22" />
                                    <% } %>
					            </td> 
					            <td> 
						            <div class="red bld"><%= (!myReview.Anonymous) ? NameHelper.FullName(myReview.User) : NameHelper.Anonymous()%>
							            <div class="rating"> 
	                                        <div id="Div<%= myReview.Id %>" class="stars">
                                                <%= StarHelper.FiveStarReadOnly(myReview.Id, myReview.Rating)  %>
	                                        </div>
								            <span class="gray small nrm"><%= LocalDateHelper.ToLocalTime(myReview.DateTimeStamp)%></span> 
							            </div> 
						            </div> 
						            <%= myReview.Review%>
					            </td> 
				            </tr> 
                        <% } %>
                    <% } else if (myViewType == ClassViewType.Discussion) { %>
                        <% if (Model.Get().Class.ClassBoards.Where(b => !b.Deleted).Count<ClassBoard>() == 0) { %>
                            There are no postings made to the board
                        <% } %>
                        <% foreach (ClassBoard myBoard in Model.Get().Class.ClassBoards.Where(b => !b.Deleted).OrderByDescending(b => b.Id)) { %>
            	            <tr> 
					            <td class="avatar"> 
						            <a href="/<%= myBoard.PostedByUser.ShortUrl %>"><img src="<%= PhotoHelper.ProfilePicture(myBoard.PostedByUser) %>" class="profile big mr22" /></a>
					            </td> 
					            <td> 
						            <div class="red bld"><%= NameHelper.FullName(myBoard.PostedByUser)%> 
                                        <a class="edit-item" href="<%= URLHelper.BuildClassBoardUrl(myBoard) %>"><%= "View Details (" + myBoard.ClassBoardReplies.Where(r => !r.Deleted).Count<ClassBoardReply>() + " replies)" %></a>
                                        <% if(ClassBoardHelper.IsAllowedToDelete(myUserInfo, myBoard)) { %>
                                            <span class="nrm small gray">|</span>
                                            <%= Html.ActionLink("Delete", "Delete", "ClassBoard", new { classId = Model.Get().Class.Id, classBoardId = myBoard.Id, source = SiteSection.Class }, new { @class = "edit-item" })%> 
                                        <% } %>
							            <div class="rating"> 
								            <span class="gray small nrm"><%= LocalDateHelper.ToLocalTime(myBoard.DateTimeStamp)%></span> 
							            </div> 
						            </div> 
						            <%= myBoard.Reply %>
					            </td> 
				            </tr> 
                        <% } %>
                    <% } %>
			    </table> 
			    <div class="flft mr22"> 
								
			    </div> 
 
			    <div class="clearfix"></div> 
		    </div> 
	    </div> 
    </div>
</asp:Content>
