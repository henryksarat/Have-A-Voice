<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HaveAVoice.Models.View.BoardFeedModel>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="Social.Generic.Models" %>

<% int myCount = (int)ViewData["Count"]; %>
<div class="board-<% if(myCount % 2 == 0) { %>row<% } else { %>alt<% } %> p-v10 m-btm5">
    <%= SharedContentStyleHelper.ProfilePictureDiv(Model.PostingUser, false, "col-2 center", "profile") %>
    <%= BoardHelper.BoardInformationDiv("col-16 m-btm10", "m-lft col-16", "p-h10", "name", Model.PostingUser, Model.Message) %>
    <%= SharedContentStyleHelper.TimeStampDiv(Model.DateTimeStamp, "col-3 right", "p-h5", "date-tile", "MMM", "dd")  %>
	<div class="clear">&nbsp;</div>
</div>

<div class="board-wrpr">
	<% int j = 0; %>
	<% foreach (BoardReply myReply in Model.BoardReplys) { %>		
	    <div class="board-<% if (j % 2 == 0) { %>row<% } else { %>alt<% } %> p-v10 push-3 col-18 m-btm5">
            <%= SharedContentStyleHelper.ProfilePictureDiv(myReply.User, false, "col-1 center", "profile sm") %>
		    <%= BoardHelper.BoardInformationDiv("m-lft col-14", "p-h10", string.Empty, "name", myReply.User, myReply.Message) %>
		    <%= SharedContentStyleHelper.TimeStampDiv(myReply.DateTimeStamp, "col-3 right", "p-h5", "date-tile", "MMM", "dd") %>
		    <div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>
		<% j++; %>
	<% } %>
</div>
	
<% UserInformationModel<User> myUserModel = HaveAVoice.Helpers.UserInformation.HAVUserInformationFactory.GetUserInformation();  %>
<% if (myUserModel != null) { %>
    <div class="board-reply m-btm5">
	    <a href="#" rel="reply" class="push-17 alpha col-1 omega center button">Reply</a>
	    <div class="push-3 col-19" style="display:none;">
		    <div class="col-1 center">
                <% string myFullName = myUserModel.Details.FirstName + " " + myUserModel.Details.LastName; %>
                <% string myProfilePictureUrl = myUserModel.ProfilePictureUrl; %>
			    <img src="<%= myProfilePictureUrl %>" alt="<%= myFullName %>" class="profile sm" />
		    </div>
		    <% using (Html.BeginForm("Create", "BoardReply", new { source = SiteSection.Profile, sourceId = Model.OwnerUserId, boardId = Model.Id })) { %>
			    <div class="m-lft col-14">
				    <div class="alpha col-12">
			            <%= Html.ValidationMessage("BoardReply", "*")%>
			            <%= Html.TextArea("BoardReply")%>
				    </div>
				    <div class="col-2 right">
		                <input type="submit" value="Post" />
		                <div class="clear">&nbsp;</div>
		            </div>
				    <div class="clear">&nbsp;</div>
			    </div>
			    <div class="clear">&nbsp;</div>
		    <% } %>
	    </div>
	    <div class="clear">&nbsp;</div>
    </div>
<% } %>