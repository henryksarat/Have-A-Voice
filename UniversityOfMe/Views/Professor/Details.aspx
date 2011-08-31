<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<CreateProfessorReviewModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= UOMConstants.TITLE %> - Professor <%= Model.Get().ProfessorName %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#submitPhoto").hide();
            $("#stars-wrapper1").stars();
            $('#request').click(function () {
                $("#submitPhoto").show();
                $("#requestToSubmit").hide();
            });
        });
    </script>
    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <% int myReviewCount = Model.Get().Professor.ProfessorReviews.Count; %>
    <% int myReviewScoreSum = Model.Get().Professor.ProfessorReviews.Sum(pr => pr.Rating); %>


	<div class="eight last">
        <% Html.RenderPartial("Message"); %>
		<div class="banner black full red-top small">
			<span class="prof">Prof. <%= Model.Get().ProfessorName %></span>
			<div class="buttons">
				<div class="rating">
				    <%= StarHelper.AveragedFiveStarImages(myReviewScoreSum, myReviewCount)%>
				    (<%= myReviewCount%> ratings)
				</div>
			</div>
		</div>

        <% Html.RenderPartial("Validation"); %>

		<div class="flft max-w207 mr21 center clearfix">
			<img src="<%= PhotoHelper.ProfessorPhoto(Model.Get().Professor) %>" alt="Prof. <%= Model.Get().ProfessorName %>" />

		    <div id="submitPhoto">
			    <% using (Html.BeginForm("SuggestProfessorPicture", "Professor", FormMethod.Post, new { enctype = "multipart/form-data", @class = "create btint-6" })) {%>
                    <%= Html.Hidden("ProfessorId", Model.Get().Professor.Id)%>
                    <%= Html.Hidden("FirstName", Model.Get().Professor.FirstName)%>
                    <%= Html.Hidden("LastName", Model.Get().Professor.LastName)%>
			        <input type="file" id="ProfessorImage" name="ProfessorImage" size="23" /><br />
                
                    <input type="submit" class="btn" value="Submit Suggestion" />
                <% } %>
            </div>
		    <div id="requestToSubmit">
                <input id="request" type="submit" class="btn" value="Suggest a photo" />
            </div>
		</div>
		<div class="flft max-w590 wp69 clearfix">
					
			<div class="listing mb40">
				<div class="col">
					<label for="university">University:</label>
				</div>
				<div class="col">
					<%= Model.Get().Professor.UniversityId %>
				</div>
	
				<div class="clearfix"></div>
							
				<div class="col">
					<label for="name">Name:</label>
				</div>
				<div class="col">
					<%= Model.Get().ProfessorName %>
				</div>
							
				<div class="clearfix"></div>
							
				<div class="col">
					<label for="score">Overall Score:</label>
				</div>
				<div class="col">
                    <% if (myReviewScoreSum == 0) { %>
                        There are no ratings yet
                    <% } else { %>
                        <%= (double)myReviewScoreSum / (double)myReviewCount%>
                    <% } %>
				</div>
							
				<div class="clearfix"></div>
			</div>
						
			<div class="banner title">
				REVIEWS
			</div>
						
			<div class="clearfix">
                <div class="professor-review-form create">
                    <div class="padding-col">
                        <% using (Html.BeginForm("Create", "ProfessorReview")) {%>
                            <%= Html.Hidden("ProfessorId", Model.Get().Professor.Id)%>
                            <%= Html.Hidden("ProfessorName", Model.Get().ProfessorName)%>
                            <div class="field-holder">
				                <label for="class">Class Code:</label>
				                <%= Html.TextBox("Class", string.Empty)%>
                                <%= Html.ValidationMessage("Class", "*", new { @class = "req" })%>
                            </div>

                            <div class="field-holder">							
				                <label for="AcademicTermId">Term:</label>
                                <%= Html.DropDownList("AcademicTermId", Model.Get().AcademicTerms)%>
                                <%= Html.ValidationMessage("AcademicTermId", "*", new { @class = "req" })%>
                            </div>

                            <div class="field-holder">
				                <label for="year">Year:</label>
                                <%= Html.DropDownList("Year", Model.Get().Years)%>
                                <%= Html.ValidationMessage("Year", "*", new { @class = "req" })%>
                            </div>

                            <div class="field-holder">
				                <label for="anonymous">Post as anonymous</label>							
				                <%= Html.CheckBox("Anonymous")%>
                            </div>

                            <div class="field-holder">                        							
                                <label for="Review">Review</label>							
				                <%= Html.TextArea("Review", new { @class = "textarea" })%>
                                <%= Html.ValidationMessage("Review", "*", new { @class = "req" })%>
                            </div>
							
				            <div class="mt13" style="margin-left: 215px">
					            <div class="mr17" style="width:100px">
	                                <div id="stars-wrapper1">
		                                <input type="radio" name="rating" value="1" title="Very poor" />
		                                <input type="radio" name="rating" value="2" title="Poor" />
		                                <input type="radio" name="rating" value="3" title="Not that bad" />
		                                <input type="radio" name="rating" value="4" title="Fair" />
		                                <input type="radio" name="rating" value="5" title="Average" />
	                                </div>
                                    <%= Html.ValidationMessage("Rating", "*", new { @class = "req" })%>
					            </div>
                                <input style="margin-left:20px" type="submit" class="btn site" name="post" value="Post Review" />
				            </div>
                        <% } %>
                    </div>
                </div>
			</div>
					
            <% if (Model.Get().Professor.ProfessorReviews.Count == 0) { %>
			    <div class="review">
				    <table border="0" cellpadding="0" cellspacing="0">
					    <tr>
						    <td>
                                There are no reviews
                            </td>
                        </tr>
                    </table>
                </div>
            <% } %>

            <% foreach (ProfessorReview myReview in Model.Get().Professor.ProfessorReviews.OrderByDescending(b => b.Id)) { %>
			    <div class="review">
				    <table border="0" cellpadding="0" cellspacing="0">
					    <tr>
						    <td>
                                <% if (!myReview.Anonymous) { %>
						            <a href="/<%= myReview.User.ShortUrl %>"><img src="<%= PhotoHelper.ProfilePicture(myReview.User) %>" class="profile big mr22" /></a>
                                <% } else { %>
                                    <img src="<%= PhotoHelper.AnonymousProfilePicture() %>" class="profile big mr22" />
                                <% } %>
						    </td>
						    <td>
							    <div class="red bld">
								    <div class="rating">
									    <%= StarHelper.AveragedFiveStarImages(myReview.Rating, 1)  %>
									    <span class="gray small nrm"><%= LocalDateHelper.ToLocalTime(myReview.DateTimeStamp)%></span>
								    </div>
								    <% if (!myReview.Anonymous) { %>
                                        <%= NameHelper.FullName(myReview.User) %>
                                    <% } else { %>
                                        Anonymous
                                    <% } %>
								    <br />
								    <span class="gray nrm small"><%= myReview.Class %> / <%= myReview.Year %> / <%= myReview.AcademicTerm.DisplayName %> </span>
							    </div>
						        <%= myReview.Review%>
						    </td>
					    </tr>
				    </table>
				    <div class="clearfix"></div>
			    </div>
            <% } %>
		</div>					
	</div>
</asp:Content>