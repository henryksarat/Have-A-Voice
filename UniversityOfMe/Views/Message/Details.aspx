<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<Message>>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Message | <%= Model.Get().Subject %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last">
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>
        <div class="create-feature-form create"> 
		    <div class="banner title black full small">
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
            <div class="padding-col">
		        <div class="box table wp90">
			        <div class="cell w60">
                        <a href="<%= URLHelper.ProfileUrl(Model.Get().FromUser) %>">
				            <img src="<%= PhotoHelper.ProfilePicture(Model.Get().FromUser) %>" class="profile big" />
                        </a>
			        </div>
			        <div class="cell pl23">
				        <span class="bold">From:</span>
				        <%= NameHelper.FullName(Model.Get().FromUser) %>
			        </div>
			        <div class="cell right">
				        <%= LocalDateHelper.ToLocalTime(Model.Get().DateTimeStamp)%>
			        </div>
			        <div class="clearfix"></div>
		        </div>
		        <p class="pt7 plr13">
			        <%= Model.Get().Body %>
		        </p>

                <% foreach(MessageReply myReply in Model.Get().MessageReplies.OrderBy(m => m.DateTimeStamp)) { %>
		            <div class="review clearfix">
			            <div class="flft w60 mr22">
                            <a href="<%= URLHelper.ProfileUrl(myReply.User) %>">
				                <img src="<%= PhotoHelper.ProfilePicture(myReply.User) %>" class="profile big" />
                            </a>
			            </div>
			            <div class="flft wp80">
				            <div class="msg-gray bld"><%= Model.User.Id == myReply.ReplyUserId ? "You said" : "They said" %>
					            <div class="frgt nrm small">
						            <%= LocalDateHelper.ToLocalTime(myReply.DateTimeStamp)%>
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
        </div>
	</div>

</asp:Content>
