<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<User>>" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Inbox
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

        <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>
	    
        <div class="eight last"> 
		    <div class="banner title black full red-top small"> 
			    <span>Compose Message</span> 
		    </div> 
	    </div> 

        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

        <div class="mt50">
            <% using (Html.BeginForm()) {%>
                <%= Html.Hidden("ToUserId", Model.Get().Id) %>

                <div class="ml60" style="float:left; text-align:center">
                    <div>
                        <img src="<%= PhotoHelper.ProfilePicture(Model.Get()) %>" class="profile lrg" />
                    </div>

                    <a class="itemlinked" href=""><%= NameHelper.FullName(Model.Get()) %></a>
                </div>
                <div class="mr20 create wp40" style="float:right">

                    <div>
                        <div>
                            <%= Html.TextBox("Subject", string.Empty, new { @class= "half" })%>
                            <%= Html.ValidationMessage("Subject", "*")%>
                        </div>
                    </div>
                    <div>
                        <div>
                            <%= Html.TextArea("Body", null, 8, 0, new { @class = "full" })%>
                            <%= Html.ValidationMessage("Body", "*")%>
                        </div>
                    </div>
                    <div>
			        <div class="right"> 
				        <input type="submit" name="submit" class="btn blue" value="Send" /> 
			        </div> 
                    </div>
                </div>                
            <% } %>
        </div>
</asp:Content>

