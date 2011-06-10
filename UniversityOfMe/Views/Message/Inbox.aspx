<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<Social.Generic.Models.InboxMessage<User>>>" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="Social.ViewHelpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= UOMConstants.TITLE %> - Message Inbox
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" language="javascript">
        $(function () {
            $('.checkall').click(function () {
                $(this).parents('fieldset:eq(0)').find(':checkbox').attr('checked', this.checked);
            });
        });
    </script>

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>
    <% using (Html.BeginForm("Inbox", "Message", FormMethod.Post)) { %>

	    <div class="eight last"> 
            <% Html.RenderPartial("Message"); %>
            <% Html.RenderPartial("Validation"); %>
		    <div class="banner title black full red-top small"> 
			    <span>MESSAGE INBOX</span> 
			    <div class="buttons"> 
		           <button type="submit" class="imagebutton">
                        <img src="/Content/Images/trash.PNG" alt="submit" />
                    </button>
			    </div> 
		    </div> 
            <fieldset>
		        <ul class="mail"> 
			        <li class="hdr"> 
				        <div class="wrpr"> 
					        <div class="flft wp25"> 
						        <input class="checkall" type="checkbox" name="checkall" id="checkall" /> 
					        </div> 
					        <div class="flft wp40"> 
						        Subject
					        </div> 
					        <div class="flft wp15"> 
						        From
					        </div> 
					        <div class="flft wp20"> 
						        Date
					        </div> 
					        <div class="clearfix"></div> 
				        </div> 
			        </li> 
                    <% foreach (var item in Model.Get()) { %>
                        <li> 
						    <div class="<%= item.Viewed ? "wrpr" : "wrpr new" %>"> 
							    <div class="flft wp5"> 
								    <input type="checkbox" name="selectedMessages" id="<%= item.MessageId %>" value="<%= item.MessageId %>" /> 
							    </div> 
							    <div class="flft wp10"> 
								    <a href="<%= URLHelper.MessageUrl(item.MessageId) %>" class="mail">Message 1</a> 
							    </div> 
							    <div class="flft wp10"> 
								    <a href="<%= URLHelper.ProfileUrl(item.FromUser) %>"><img src="<%= PhotoHelper.ProfilePicture(item.FromUser) %>" alt="<%= NameHelper.FullName(item.FromUser) %>" title="<%= NameHelper.FullName(item.FromUser) %>" class="profile med" /></a>
							    </div> 
							    <div class="flft wp40"> 
								    <span class="name bld"><a class="name" href="<%= URLHelper.MessageUrl(item.MessageId) %>"><%= item.Subject %></a></span> 
								    <%= TextShortener.Shorten(item.LastReply, 50) %>
							    </div> 
							    <div class="flft wp15"> 
								    <a href="<%= URLHelper.ProfileUrl(item.FromUser) %>" class="red"><%= NameHelper.FullName(item.FromUser) %></a> 
							    </div> 
							    <div class="flft wp20"> 
								    <%= DateHelper.ToLocalTime(item.DateTimeStamp) %>
							    </div> 
							    <div class="clearfix"></div> 
						    </div> 
					    </li> 
                    <% } %>
                </ul> 
            </fieldset>
	    </div> 
    <%} %>
</asp:Content>

