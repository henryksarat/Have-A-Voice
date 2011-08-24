<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<Board>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Functionality" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="Social.Generic.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UniversityOf.Me - <%=NameHelper.FullName(Model.User) %>'s Profile
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% UserInformationModel<User> myLoggedInUser = UserInformationFactory.GetUserInformation(); %>

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="create"> 
			<div class="banner black full red-top small"> 
				<span class="board">Board Post on <%= NameHelper.FullName(Model.Get().OwnerUser) %>'s Board</span> 
			</div> 

		    <div class="board"> 
			    <div class="prfl clearfix"> 
				    <div class="pCol center"> 
                        <a href="<%= URLHelper.ProfileUrl(Model.Get().PostedByUser) %>">
                            <img src="<%= PhotoHelper.ProfilePicture(Model.Get().PostedByUser) %>" class="profile lrg" />
                        </a>
				    </div> 
				    <div class="cCol"> 
					    <div class="red bld"> 
						    <div class="frgt"> 
							    <span class="gray small nrm"> 
								    <%= LocalDateHelper.ToLocalTime(Model.Get().DateTimeStamp, "{0:MMMM dd, yyyy h:mm tt}")%>
							    </span> 
						    </div> 
						    <%= NameHelper.FullName(Model.Get().PostedByUser) %>
                            <% if(BoardHelper.IsAllowedToDelete(myLoggedInUser, Model.Get())) {  %>
                                <%= Html.ActionLink("Delete", "Delete", "Board", new { sourceId = Model.Get().Id, boardId = Model.Get().Id, sourceController = "Board", sourceAction = "Details" }, new { @class = "small nrm" })%>
                            <% } %>
					    </div> 
					    <%= Model.Get().Message %>
					    <div class="create clearfix"> 
                            <% using (Html.BeginForm("Create", "BoardReply", FormMethod.Post)) { %>
                                    <%= Html.Hidden("BoardId", Model.Get().Id)%>
                                    <%= Html.Hidden("SourceId", Model.Get().Id)%>
                                    <%= Html.Hidden("SiteSection", SiteSection.Board) %>

                                    <%= Html.TextArea("BoardReply", null, 2, 0, new { @class="full" })%>
                                    <%= Html.ValidationMessage("BoardReply", "*")%>

						    <div class="frgt mt13"> 
							    <input type="submit" class="frgt btn site" name="post" value="Reply" /> 
						    </div> 
	                        <% } %>
					    </div> 
				    </div>							
			    </div> 

                <% foreach (BoardReply myReply in Model.Get().BoardReplies.Where(br => !br.Deleted).OrderByDescending(br => br.DateTimeStamp)) { %>						
			    <div class="prfl reply clearfix"> 
				    <div class="pCol"> 
					    <img src="<%= PhotoHelper.ProfilePicture(myReply.User) %>" class="profile med" /> 
				    </div> 
				    <div class="cCol"> 
					    <div class="red bld"> 
						    <div class="frgt"> 
							    <span class="gray small nrm"> 
								    <!-- <a href="#" class="">Edit</a> 
								    |
								    <a href="#" class="mr20">Remove</a> -->
								    <%= LocalDateHelper.ToLocalTime(myReply.DateTimeStamp, "{0:MMMM dd, yyyy h:mm tt}")%>
							    </span> 
						    </div> 
						    <%= NameHelper.FullName(myReply.User) %>
                            <% if (BoardReplyHelper.IsAllowedToDelete(myLoggedInUser, myReply)) {  %>
                                <%= Html.ActionLink("Delete", "Delete", "BoardReply", new { sourceId = Model.Get().Id, boardReplyId = myReply.Id, sourceController = "Board", sourceAction = "Details" }, new { @class = "small nrm" })%>
                            <% } %>
					    </div> 
					    <%= myReply.Message %>
				    </div> 
			    </div> 
                <% } %>
            </div>
		</div> 
	</div> 
</asp:Content>
