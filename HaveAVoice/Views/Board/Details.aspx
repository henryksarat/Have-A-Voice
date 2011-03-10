<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInWrapperModel<Board>>" %>
<%@Import Namespace="HaveAVoice.Models" %>
<%@Import Namespace="HaveAVoice.Models.View" %>
<%@Import Namespace="HaveAVoice.Helpers" %>
<%@Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@Import Namespace="HaveAVoice.Services.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	View
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.RenderPartial("UserPanel", Model.NavigationModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation", Model.NavigationModel); %>
    </div>

	<div class="col-21">
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>
		<div class="clear">&nbsp;</div>


        <div class="board-alt p-v10 m-btm5">
	        <div class="col-2 center">
		        <img src="<%= PhotoHelper.ProfilePicture(Model.Model.PostedByUser) %>" alt="<%= NameHelper.FullName(Model.Model.PostedByUser) %>" class="profile" />
	        </div>
	        <div class="col-16 m-btm10">
		        <div class="m-lft col-16">
			        <div class="p-h10">
				        <a class="name" href="<%= LinkHelper.Profile(Model.Model.PostedByUser) %>"><%= NameHelper.FullName(Model.Model.PostedByUser) %></a>
                        <%= Model.Model.Message %>
				        <div class="clear">&nbsp;</div>
			        </div>
		        </div>
	        </div>
	        <div class="col-3 right">
		        <div class="p-h5">
			        <div class="date-tile">
				        <span><%= Model.Model.DateTimeStamp.ToString("MMM").ToUpper()%></span> <%= Model.Model.DateTimeStamp.ToString("dd")%>
			        </div>
		        </div>
	        </div>
	        <div class="clear">&nbsp;</div>
        </div>
        <div class="board-wrpr">
	        <% int j = 0; %>
	        <% foreach (BoardReply myReply in Model.Model.BoardReplies) { %>		
	            <div class="board-<% if (j % 2 == 0) { %>row<% } else { %>alt<% } %> p-v10 push-3 col-18 m-btm5">
		            <div class="col-1 center">
		                <img src="<%= PhotoHelper.ProfilePicture(myReply.User) %>" alt="<%= NameHelper.FullName(myReply.User) %>" class="profile sm" />
		            </div>
		            <div class="m-lft col-14">
		                <div class="p-h10">
		                    <a href="<%= LinkHelper.Profile(myReply.User) %>" class="name"><%= NameHelper.FullName(myReply.User)%></a>
		                    <%= myReply.Message %>
		                </div>
		            </div>
		            <div class="col-3 right">
		                <div class="p-h5">
		                    <div class="date-tile">
		                        <span><%= myReply.DateTimeStamp.ToString("MMM").ToUpper() %></span> <%= myReply.DateTimeStamp.ToString("dd") %>
		                    </div>
		                </div>
		            </div>
		            <div class="clear">&nbsp;</div>
		        </div>
		        <div class="clear">&nbsp;</div>
		        <% j++; %>
	        <% } %>
        </div>

        <% UserInformationModel myUserModel = HaveAVoice.Helpers.UserInformation.HAVUserInformationFactory.GetUserInformation();  %>
        <% if (myUserModel != null) { %>
            <div class="board-reply m-btm5">
	            <div class="push-3 col-19">
		            <div class="col-1 center">
			            <img src="<%= myUserModel.ProfilePictureUrl %>" alt="<%= myUserModel.FullName %>" class="profile sm" />
		            </div>
		            <% using (Html.BeginForm("Create", "BoardReply", new { source = SiteSection.BoardReply, sourceId = Model.Model.Id, boardId = Model.Model.Id })) { %>
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
	</div>
</asp:Content>