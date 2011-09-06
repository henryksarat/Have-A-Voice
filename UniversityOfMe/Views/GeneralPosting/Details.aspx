<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<GeneralPosting>>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Helpers.Functionality" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	General Posting | <%= Model.Get().Title %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

	    <div class="banner black full red-top small"> 
		    <span class="post"> 
			    GENERAL POSTING: <%= Model.Get().Title %>
			    <span class="user">by <%= NameHelper.FullName(Model.Get().User) %> on <%= LocalDateHelper.ToLocalTime(Model.Get().DateTimeStamp)%></span> 
		    </span> 
		    <div class="buttons"> 
                <div class="flft mr13"> 
                    <% if(GeneralPostingHelper.IsSubscribed(UserInformationFactory.GetUserInformation().Details, Model.Get())) { %>
                        <%= Html.ActionLink("Unsubscribe", "Unsubscribe", "GeneralPosting", new { id = Model.Get().Id }, new { @class = "remove" })%>
                    <% } else { %>
                        <%= Html.ActionLink("Subscribe", "Subscribe", "GeneralPosting", new { id = Model.Get().Id }, new { @class = "add" })%>
                    <% } %>
                </div>
		    </div> 
	    </div> 
					
	    <h3><%= Model.Get().Title %></h3> 
	    <p class="gray mt31 mb42"> 
		    <%= Model.Get().Body %>
	    </p> 
 
	    <div class="banner full"> 
		    COMMENTS
	    </div> 
					
	    <div id="review"> 
		    <div class="create"> 
                <% using (Html.BeginForm("Create", "GeneralPostingReply")) {%>
                    <%= Html.Hidden("GeneralPostingId", Model.Get().Id)%>
			        <%= Html.TextArea("Reply", string.Empty, 6, 0, new { @class = "full" })%>
                    <%= Html.ValidationMessage("Reply", "*")%>
					
                    <div class="frgt mt13"> 
						<input type="submit" class="frgt btn site" name="post" value="Post" /> 
					</div> 	
			        <div class="clearfix"></div> 
                <% } %>
		    </div> 
						
		    <div class="clearfix"></div> 
						
		    <div class="review"> 
			    <table border="0" cellpadding="0" cellspacing="0"> 
                    <% foreach (GeneralPostingReply myReply in Model.Get().GeneralPostingReplies.OrderByDescending(r => r.DateTimeStamp)) { %>
				        <tr> 
					        <td class="avatar"> 
						        <a href="/<%= myReply.User.ShortUrl %>"><img src="<%= PhotoHelper.ProfilePicture(myReply.User) %>" class="profile big mr22" /></a>
					        </td> 
					        <td> 
						        <div class="red bld">
                                    <%= NameHelper.FullName(myReply.User)%>
								    <span class="gray small nrm"><%= LocalDateHelper.ToLocalTime(myReply.DateTimeStamp)%></span> 
						        </div> 
						        <%= myReply.Reply %>
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
</asp:Content>
