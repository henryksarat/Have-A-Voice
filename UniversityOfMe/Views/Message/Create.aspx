<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<CreateMessageModel<User>>>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="Social.Messaging.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Inbox
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

        <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>
	    
        <div class="eight last"> 
            <% Html.RenderPartial("Message"); %>
            <% Html.RenderPartial("Validation"); %>
            <div class="create-message-form">
    		    <div class="banner title black full red-top small"> 
			        <span>Compose Message</span> 
		        </div> 
	        <% using (Html.BeginForm()) {%>
                <%= Html.Hidden("ToUserId", Model.Get().SendToUser.Id) %>
                <div class="twoColEditUser clearfix">
                    <div class="lCol center">  
                        <div>
                            <a href="<%= URLHelper.ProfileUrl(Model.Get().SendToUser) %>">
                                <img src="<%= PhotoHelper.ProfilePicture(Model.Get().SendToUser) %>" class="profile lrg" />
                            </a>
                        </div>
                    
                        <a class="itemlinked" href="<%= URLHelper.ProfileUrl(Model.Get().SendToUser) %>"><%= NameHelper.FullName(Model.Get().SendToUser)%></a>
                    </div>

                    <div class="rCol">  
                        <div class="create">
                            <div class="field-holder">
                                <label for="Subject">Subject:</label>
                                <%= Html.TextBox("Subject", Model.Get().DefaultSubject)%>
                                <%= Html.ValidationMessage("Subject", "*", new { @class = "req" })%>
                            </div>
                            <div class="field-holder">
                                <label for="Body">Body:</label>
                                <%= Html.TextArea("Body", null, 8, 0, new { @class = "textarea" })%>
                                <%= Html.ValidationMessage("Body", "*", new { @class = "req" })%>
                            </div>
                            <div>
			                <div class="field-holder center" style="margin-left:185px">
				                <input type="submit" name="submit" class="btn blue" value="Send" /> 
			                </div> 
                            </div>
                        </div>
                    </div>        
                </div>
            <% } %>
            </div>
        </div>
</asp:Content>

