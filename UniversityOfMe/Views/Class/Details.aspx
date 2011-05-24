<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<Class>>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        $("#stars-wrapper1").stars();
        <% foreach (ClassReview myReview in Model.Get().ClassReviews.OrderByDescending(b => b.Id)) { %>
            $("#Div<%= myReview.Id %>").stars({
                disabled: true
            });
        <% } %>
    });
</script>

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>
	    <div class="banner black full red-top small"> 
		    <span class="class">CLASS <%= Model.Get().ClassCode %></span> 
		    <div class="buttons"> 
			    <div class="flft mr26"> 
                    <% if(ClassHelper.IsEnrolled(UniversityOfMe.UserInformation.UserInformationFactory.GetUserInformation(), Model.Get())) { %>
                        <a href="#" class="unroll">Remove me from this class</a> 
                    <% } else { %>
				        <a href="#" class="enroll">I'm enrolled in this class</a> 
                    <% } %> 
			    </div> 
			    <div class="rating"> 
				    <span class="star"></span> 
				    <span class="star"></span> 
				    <span class="half"></span> 
				    <span class="empty"></span> 
				    <span class="empty"></span> 
				    (43 ratings)
			    </div> 
		    </div> 
	    </div> 
					
	    <table border="0" cellpadding="0" cellspacing="0" class="listing mb32"> 
		    <tr> 
			    <td> 
				    <label for="code">CLASS CODE:</label> 
			    </td> 
			    <td> 
				    <%= Model.Get().ClassCode %>
			    </td> 
		    </tr> 
		    <tr> 
			    <td> 
				    <label for="title">CLASS TITLE:</label> 
			    </td> 
			    <td> 
				    <%= Model.Get().ClassTitle %>
			    </td> 
		    </tr> 
		    <tr> 
			    <td> 
				    <label for="year">YEAR:</label> 
			    </td> 
			    <td> 
				    <%= Model.Get().Year %>
			    </td> 
		    </tr> 
		    <tr> 
			    <td> 
				    <label for="academic">ACADEMIC TERM:</label> 
			    </td> 
			    <td> 
				    <%= Model.Get().AcademicTerm.DisplayName %>
			    </td> 
		    </tr> 
		    <tr> 
			    <td> 
				    <label for="desc">DESCRIPTION:</label> 
			    </td> 
			    <td> 
				    <%= Model.Get().Details %>
			    </td> 
		    </tr> 
	    </table> 
					
	    <div class="banner title"> 
		    CURRENT MEMBERS ENROLLED
	    </div> 
	    <div class="box sm group"> 
		    <ul class="members"> 
                <% foreach (ClassEnrollment myEnrollment in Model.Get().ClassEnrollments) { %>
			        <li> 
				        <a href="/<%= Model.Get().User.ShortUrl %>"><img src="<%= PhotoHelper.ProfilePicture(myEnrollment.User) %>" class="profile med flft mr9" /></a>
				        <span class="name"><%= NameHelper.FullName(myEnrollment.User) %></span> 
				        Student
			        </li>
                <% } %>
 		    </ul> 
		    <a href="#" class="viewall">View all members</a> 
		    <div class="clearfix"></div> 
	    </div> 
					
	    <div class="tabs"> 
		    <ul> 
                <% ClassViewType myViewType = (ClassViewType)ViewData["ClassViewType"]; %>
			    <li class="<%= myViewType == ClassViewType.Discussion ? "active" : ""  %>"> 
                    <a href="<%= URLHelper.BuildClassDiscussionUrl(Model.Get()) %>">Class Board</a> 
			    </li> 
			    <li class="<%= myViewType == ClassViewType.Review ? "active" : ""  %>"> 
				    <a href="<%= URLHelper.BuildClassReviewUrl(Model.Get()) %>">Class Review</a> 
			    </li> 
		    </ul> 
	    </div> 
					
	    <div id="review"> 
            <% if (myViewType == ClassViewType.Review) { %>
                <% using (Html.BeginForm("Create", "ClassReview")) {%>
                    <%= Html.Hidden("ClassId", Model.Get().Id)%>
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
            <% } else if (myViewType == ClassViewType.Discussion) { %>
                <% using (Html.BeginForm("Create", "ClassBoard")) {%>
                    <%= Html.Hidden("ClassId", Model.Get().Id) %>
		            <div class="create"> 
                        <%= Html.TextArea("BoardMessage", string.Empty, 6, 0, new { @class = "full" })%>
                        <%= Html.ValidationMessage("BoardMessage", "*")%>
			            <div class="frgt mt13"> 
				            <input type="submit" class="frgt btn site" name="post" value="Post To Discussion" /> 
                        </div>
                        <div class="clearfix"></div> 
		            </div> 
                <% } %>
            <% } %>
						
		    <div class="clearfix"></div> 
						
		    <div class="review"> 
			    <table border="0" cellpadding="0" cellspacing="0"> 
                    <% if (myViewType == ClassViewType.Review) { %>
                        <% if (Model.Get().ClassReviews.Count == 0) { %>
                            There are no reviews
                        <% } %>
                        <% foreach (ClassReview myReview in Model.Get().ClassReviews.OrderByDescending(b => b.Id)) { %>
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
								            <span class="gray small nrm"><%= DateHelper.ToLocalTime(myReview.DateTimeStamp)%></span> 
							            </div> 
						            </div> 
						            <%= myReview.Review%>
					            </td> 
				            </tr> 
                        <% } %>
                    <% } else if (myViewType == ClassViewType.Discussion) { %>
                        <% if (Model.Get().ClassBoards.Count == 0) { %>
                            There are no postings made to the board
                        <% } %>
                        <% foreach (ClassBoard myBoard in Model.Get().ClassBoards.OrderByDescending(b => b.Id)) { %>
            	            <tr> 
					            <td class="avatar"> 
						            <a href="/<%= myBoard.User.ShortUrl %>"><img src="<%= PhotoHelper.ProfilePicture(myBoard.User) %>" class="profile big mr22" /></a>
					            </td> 
					            <td> 
						            <div class="red bld"><%= NameHelper.FullName(myBoard.User) %>
							            <div class="rating"> 
								            <span class="gray small nrm"><%= DateHelper.ToLocalTime(myBoard.DateTimeStamp)%></span> 
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
