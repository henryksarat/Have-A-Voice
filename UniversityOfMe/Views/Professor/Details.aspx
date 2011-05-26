<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<CreateProfessorReviewModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#stars-wrapper1").stars();
        });
    </script>
    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <% Html.RenderPartial("Validation"); %><br />
    <% Html.RenderPartial("Message"); %>

    <% int myReviewCount = Model.Get().Professor.ProfessorReviews.Count; %>
    <% int myReviewScoreSum = Model.Get().Professor.ProfessorReviews.Sum(pr => pr.Rating); %>

	<div class="eight last"> 
		<div class="banner black full red-top small"> 
			<span class="professor">Prof. <%= Model.Get().ProfessorName %></span> 
			<div class="buttons"> 
				<div class="rating"> 
				    <%= StarHelper.AveragedFiveStarImages(myReviewScoreSum, myReviewCount)%>
				    (<%= myReviewCount%> ratings)
				</div> 
			</div> 
		</div> 
					
		<table border="0" cellpadding="0" cellspacing="0" class="listing mb32"> 
			<tr> 
				<td> 
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr><td style="border: 1px solid #8d8989; ; width:207px; height:278px; background-color:#f6f6f6; background-image:url(/Content/images/personwhite.png); background-repeat:no-repeat; background-position:center; "></td></tr>
                        <tr><td style="text-align:center"><input type="submit" class="btn site" name="post" value="Suggest Photo" /> </td></tr>
                    </table>
				</td> 
				<td> 
					<table border="0" cellpadding="0" cellspacing="0" class="listing mb32 ml20"> 
                        <tr style="height:30px">
                            <td>
                                <label for="university">University:</label> 
                            </td>
                            <td style="width:100%">
                                <%= Model.Get().Professor.UniversityId %>
                            </td>
                        </tr>
                        <tr style="height:30px">
                            <td>
                                <label for="name">Name:</label> 
                            </td>
                            <td style="width:100%">
                                <%= Model.Get().ProfessorName %>
                            </td>
                        </tr>
                        <tr  style="height:30px">
                            <td>
                                <label for="overall">Overall:</label> 
                            </td>
                            <td style="width:100%">
                                <% if (myReviewScoreSum == 0) { %>
                                    There are no ratings yet
                                <% } else { %>
                                    <%= (double)myReviewScoreSum / (double)myReviewCount%>
                                <% } %>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="banner title" style="height: 10px">
			                        REVIEWS
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div id="Div1"> 
			                        <div class="create mt13"> 
                                        <% using (Html.BeginForm("Create", "ProfessorReview")) {%>
                                            <%= Html.Hidden("ProfessorId", Model.Get().Professor.Id) %>
                                            <%= Html.Hidden("ProfessorName", Model.Get().ProfessorName) %>
				                            <div class="flft"> 
                                                <label class="post" for="anon">Class Title:</label>
                                                <%= Html.TextBox("Class", string.Empty, new { @class = "flft" })%>
                                                <%= Html.ValidationMessage("Class", "*", new { @class = "flft" })%>
                                            </div>
                                            <div class="flft ml60"> 
                                                <label class="post" for="anon">Academic Term:</label>
                                                <%= Html.DropDownList("AcademicTermId", Model.Get().AcademicTerms, new { @class = "flft" })%>
                                                <%= Html.ValidationMessage("AcademicTermId", "*", new { @class = "flft" })%>
				                            </div> 
				                            <div class="flft"> 
                                                <label class="post" for="anon">Year:</label>
                                                <%= Html.DropDownList("Year", Model.Get().Years, new { @class = "flft" })%>
                                                <%= Html.ValidationMessage("Year", "*", new { @class = "flft" })%>
				                            </div> 
                                            <div class="flft"> 
                                                <label class="post" for="anon">Post as anonymous:</label>
                                                <%= Html.CheckBox("Anonymous")%>
                                            </div>
				                            <div class="clearfix"></div> 
                                            <%= Html.TextArea("Review", string.Empty, 6, 0, new { @class = "full"}) %>
                                            <%= Html.ValidationMessage("Review", "*", new { @class = "flft" })%>
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
				                            </div> 
                                        <% } %>
			                        </div> 
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
		                        <div class="review"> 
			                        <table border="0" cellpadding="0" cellspacing="0"> 
                                            <% if (Model.Get().Professor.ProfessorReviews.Count == 0) { %>
                                                There are no reviews
                                            <% } %>
                                            <% foreach (ProfessorReview myReview in Model.Get().Professor.ProfessorReviews.OrderByDescending(b => b.Id)) { %>
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
                                                                    <%= StarHelper.AveragedFiveStarImages(myReview.Rating, 1)  %>
	                                                            </div>
								                                <span class="gray small nrm"><%= DateHelper.ToLocalTime(myReview.DateTimeStamp)%></span> 
							                                </div> 
						                                </div> 
                                                        <div class="mt-6 small"><%= myReview.Class %> / <%= myReview.Year %> / <%= myReview.AcademicTerm.DisplayName %> 
                                                        </div>
						                                <%= myReview.Review%>
					                                </td> 
				                                </tr> 
                                            <% } %>
			                        </table> 
			                        <div class="flft mr22"> 
								
			                        </div> 
 
			                        <div class="clearfix"></div> 
		                        </div> 
                            </td>
                        </tr>
                    </table>
				</td> 
			</tr> 
		</table> 
	</div> 
</asp:Content>