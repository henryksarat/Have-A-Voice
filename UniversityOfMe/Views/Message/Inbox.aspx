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

                    <% string myIcon = item.Viewed == true ? "/Content/images/readmessage.png" : "/Content/images/unreadmessage.png"; %>

                    <div class="message">
                        <div class="col-1" style="width:5%">
                            <div class="p-a10">
                                <%= Html.CheckBox("something") %>
                            </div>
                        </div>
                        <div style="width:10%; float:left;">
                            <div class="p-a10">                            
                                <img src="<%= myIcon %>" />
                            </div>
                        </div>
                        <div style="width:10%; float:left">
                            <div class="p-v10">
                                <img src="<%= item.FromUserProfilePictureUrl %>" class="profile sm" />
                            </div>
                        </div>
                        <div style="width:40%; float:left">
                            <div class="p-t10">
                                <%= item.Subject %>
                            </div>
                            <div class="p-b10">
                                <%= item.LastReply %>
                            </div>
                        </div>
                        <div style="width:15%; float:left">
                            <div class="p-t10" >
                                <%= item.FromUser %>
                            </div>
                        </div>
                        <div  style="width:15%; float:left">
                            <div class="p-b10">
                                <%= DateHelper.ToLocalTime(item.DateTimeStamp) %>
                            </div>
                        </div>
                    </div>
                <% } %>


                <br />
		        <input type="submit" value="Delete" />
            <%} %>

    </div> 
</asp:Content>

