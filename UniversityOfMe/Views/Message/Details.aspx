<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<Message>>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UniversityOf.Me - Message
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last">
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="banner title black full red-top small">
			<span><%= Model.Get().Subject %></span>
			<div class="buttons">
                <% using (Html.BeginForm("Delete", "Message")) {%>
                    <%= Html.Hidden("MessageId", Model.Get().Id) %>
			        <button type="submit" class="imagebutton">
                        <img src="/Content/Images/trash.PNG" alt="submit" />
                    </button>
                <% } %>
			</div>
		</div>
		<div class="box table wp100">
			<div class="cell w60">
				<img src="<%= PhotoHelper.ProfilePicture(Model.Get().FromUser) %>" class="profile big" />
			</div>
			<div class="cell pl23">
				<label for="from">From:</label>
				<%= NameHelper.FullName(Model.Get().FromUser) %>
			</div>
			<div class="cell right">
				<%= DateHelper.ToLocalTime(Model.Get().DateTimeStamp) %>
			</div>
			<div class="clearfix"></div>
		</div>
		<p class="pt18 plr13">
			<%= Model.Get().Body %>
		</p>

        <% foreach(MessageReply myReply in Model.Get().MessageReplies.OrderByDescending(m => m.DateTimeStamp)) { %>
		    <div class="review clearfix">
			    <div class="flft w60 mr22">
				    <img src="<%= PhotoHelper.ProfilePicture(myReply.User) %>" class="profile big" />
			    </div>
			    <div class="flft wp80">
				    <div class="msg-gray bld"><%= Model.User.Id == myReply.ReplyUserId ? "You said" : "They said" %>
					    <div class="frgt nrm small">
						    <%= DateHelper.ToLocalTime(myReply.DateTimeStamp) %>
					    </div>
				    </div>
				    <%= myReply.Body %>
			    </div>
		    </div>
        <% } %>

		<div class="create">
			<span class="reply">Reply</span>
            <% using (Html.BeginForm("CreateReply", "Message")) {%>
                <%= Html.Hidden("MessageId", Model.Get().Id) %>
                <%= Html.TextArea("Reply", string.Empty, 6, 0, new { @class = "full" } ) %>
			    <div class="right">
				    <input type="submit" class="btn" value="Reply" name="submit" id="submit" />
			    </div>
            <% } %>
            <%= Html.ActionLink("Back to messages", "Inbox", "Message", new { @class = "back" })%>
		</div>
	</div>

</asp:Content>
