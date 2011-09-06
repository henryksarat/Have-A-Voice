<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<ClassBoard>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Functionality" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="Social.Generic.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Class Discussion for <%= Model.Get().Class.ClassTitle %> (<%= Model.Get().Class.ClassCode %>)
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% UserInformationModel<User> myLoggedInUser = UserInformationFactory.GetUserInformation(); %>

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="create professor-review-form"> 
			<div class="banner black full red-top small"> 
				<span class="board">Class Discussion Post in <%= Model.Get().Class.ClassTitle %> ( <a class="header-linked" href="<%= URLHelper.BuildClassDiscussionUrl(Model.Get().Class) %>"><%= Model.Get().Class.ClassCode %></a> )</span> 
			</div> 

		    <div class="board padding-col"> 
			    <div class="prfl clearfix"> 
				    <div class="pCol"> 
                        <a href="<%= URLHelper.ProfileUrl(Model.Get().PostedByUser) %>">
					        <img src="<%= PhotoHelper.ProfilePicture(Model.Get().PostedByUser) %>" class="profile big" /> 
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
                            <% if(ClassBoardHelper.IsAllowedToDelete(myLoggedInUser, Model.Get())) {  %>
                                <%= Html.ActionLink("Delete", "Delete", "ClassBoard", new { classId = Model.Get().ClassId, classBoardId = Model.Get().Id, source = SiteSection.ClassBoard }, new { @class = "edit-item" })%>
                            <% } %>
					    </div> 
					    <%= Model.Get().Reply %>
					    <div class="create clearfix"> 
                            <% using (Html.BeginForm("Create", "ClassBoardReply", FormMethod.Post)) { %>
                                    <%= Html.Hidden("ClassId", Model.Get().ClassId)%>
                                    <%= Html.Hidden("ClassBoardId", Model.Get().Id)%>

                                    <%= Html.TextArea("BoardMessage", null, 2, 0, new { @class="full" })%>
                                    <%= Html.ValidationMessage("BoardMessage", "*")%>

						    <div class="frgt mt13"> 
							    <input type="submit" class="frgt btn site" name="post" value="Reply" /> 
						    </div> 
	                        <% } %>
					    </div> 
				    </div>							
			    </div> 

                <% foreach (ClassBoardReply myReply in Model.Get().ClassBoardReplies.Where(r => !r.Deleted).OrderByDescending(r => r.DateTimeStamp)) { %>						
			    <div class="prfl reply clearfix"> 
				    <div class="pCol"> 
                        <a href="<%= URLHelper.ProfileUrl(myReply.PostedByUser) %>">
					        <img src="<%= PhotoHelper.ProfilePicture(myReply.PostedByUser) %>" class="profile med" /> 
                        </a>
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
						    <%= NameHelper.FullName(myReply.PostedByUser) %>
                            <% if (ClassBoardReplyHelper.IsAllowedToDelete(myLoggedInUser, myReply)) {  %>
                                <%= Html.ActionLink("Delete", "Delete", "ClassBoardReply", new { classId = Model.Get().ClassId, classBoardId = Model.Get().Id, classBoardReplyId = myReply.Id }, new { @class = "edit-item" })%>
                            <% } %>
					    </div> 
					    <%= myReply.Reply %>
				    </div> 
			    </div> 
                <% } %>
            </div>
		</div> 
	</div> 
</asp:Content>
