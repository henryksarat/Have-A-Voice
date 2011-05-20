<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<Social.Generic.Models.InboxMessage>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="Social.ViewHelpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Inbox
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <div class="seven"> 
	    <div class="banner black full small red-top"> 
		    MESSAGE INBOX
	    </div> 


            <% using (Html.BeginForm("Inbox", "Message")) { %>

                <% foreach (var item in Model.Get()) { %>

                    <div class="message new">
                        <div class="col-1">
                            <div class="p-a10">checkbox</div>
                        </div>
                        <div class="col-1">
                            <div class="p-v10">
                                <img src="<%= item.FromUserProfilePictureUrl %>" class="profile sm" />
                            </div>
                            <div class="col-4 m-lft m-rgt">
                                <div class="p-t10">
                                    <%= item.FromUser %>
                                </div>
                                <div class="clearfix"></div>
                                <div class="p-b10 fnt-10">
                                    <%= item.DateTimeStamp %>
                                </div>
                            </div>
                        </div>
                        <div class="col-13 m-lft m-rgt">
                            <div class="p-t10">
                                <%= item.Subject %>
                            </div>
                            <div class="clearfix"></div>
                            <div class="p-b10">
                                <%= item.LastReply %>
                            </div>
                        </div>
                        <div class="col-2 m-lft m-rgt p-v10 right">
                           &nbsp;
                        </div>
                        <div class="clearfix"></div>
                    </div>
                <% } %>

		        <input type="submit" value="Delete" />
            <%} %>

    </div> 
</asp:Content>

