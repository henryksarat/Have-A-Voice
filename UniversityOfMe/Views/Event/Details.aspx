<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<Event>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="Social.Generic.Models" %>
<%@ Import Namespace="Social.Admin.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	University of Me - <%= Model.Get().Title %> at <%= Model.University.UniversityName %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>



	<div class="eight last"> 
		<div class="banner black full red-top small"> 
			<span class="event">EVENT: <%=Model.Get().Title %></span> 
		</div> 
					
		<table border="0" cellpadding="0" cellspacing="0" class="listing mb32"> 
			<tr> 
				<td> 
					<label for="title">TITLE:</label> 
				</td> 
				<td> 
					<%= Model.Get().Title %>
				</td> 
			</tr> 
			<tr> 
				<td> 
					<label for="startdate">START DATE:</label> 
				</td> 
				<td> 
					<%= Model.Get().StartDate %>
				</td> 
			</tr> 
			<tr> 
				<td> 
					<label for="endate">END DATE:</label> 
				</td> 
				<td> 
					<%= Model.Get().EndDate %>
				</td> 
			</tr> 
			<tr> 
				<td> 
					<label for="host">HOSTED BY:</label> 
				</td> 
				<td> 
					<a href="#" class="teal"><%= NameHelper.FullName(Model.Get().User) %></a> 
				</td> 
			</tr> 
			<tr> 
				<td> 
					<label for="desc">INFORMATION:</label> 
				</td> 
				<td> 
					<%= Model.Get().Information %>
				</td> 
			</tr> 
		</table> 
					
		<div class="banner full"> 
			COMMENTS
		</div> 
										
		<div id="review"> 
			<div class="create"> 
                <% using (Html.BeginForm("Create", "EventBoard")) {%>
				    <%= Html.TextArea("BoardMessage", string.Empty, 6, 0, new { @class = "full" })%>
                    <%= Html.Hidden("EventId", Model.Get().Id)%>
							
				    <div class="right mt13"> 
					    <input type="submit" class="btn site" name="post" value="Post" /> 
				    </div> 
				    <div class="clearfix"></div> 
                <% } %>
			</div> 
						
			<div class="clearfix"></div> 
						
			<div class="review"> 
				<table border="0" cellpadding="0" cellspacing="0"> 
                    <% if (Model.Get().EventBoards.Count == 0) { %>
                        There are no postings to the event board yet
                    <% } %>
                    <% foreach (EventBoard myEventBoard in Model.Get().EventBoards.OrderByDescending(e => e.DateTimeStamp)) { %>
					    <tr> 
						    <td class="avatar">
							    <a href="/<%= myEventBoard.User.ShortUrl %>"><img src="<%= PhotoHelper.ProfilePicture(myEventBoard.User) %>" class="profile big mr22" /></a>
						    </td> 
						    <td> 
							    <div class="red bld"> 
								    <%= NameHelper.FullName(myEventBoard.User)%>
								    <div class="frgt"> 
									    <span class="gray small nrm"><%= DateHelper.ToLocalTime(myEventBoard.DateTimeStamp) %></span> 
								    </div> 
							    </div> 
							    <%= myEventBoard.Message %>
						    </td> 
					    </tr> 
                    <% } %>
				</table> 
				<div class="flft mr22"> 
								
				</div> 
 
				<div class="clearfix"></div> 
			</div> 
		</div> 
	</div> 




























 
        <% UserInformationModel<User> myUserInfo = UserInformationFactory.GetUserInformation(); %>
        <% bool myAllowedToEdit = Model.Get().UserId == myUserInfo.Details.Id || PermissionHelper<User>.AllowedToPerformAction(myUserInfo, Social.Generic.Helpers.SocialPermission.Edit_Any_Event); %>
        <% bool myAllowedToDelete = Model.Get().UserId == myUserInfo.Details.Id || PermissionHelper<User>.AllowedToPerformAction(myUserInfo, Social.Generic.Helpers.SocialPermission.Delete_Any_Event); %>
        
        <% if (false) { %>
            <% using (Html.BeginForm("Delete", "Event", new { id = Model.Get().Id })) {%>
                <input type="submit" value="Delete" />
            <% } %>
        <% } %>

        <% if (false) { %>
            <%= Html.ActionLink("Edit", "Edit", "Event", new { id = Model.Get().Id }, null)%>
        <% } %>
</asp:Content>

