<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<EditUserSpecificRegionModel>>" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Social.Generic" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers.ProfileQuestions" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Your Region Specifics
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-24">
	    <div class="col-6 profile m-top30">
	        <img src="<%= Model.NavigationModel.ProfilePictureUrl %>" alt="<%= Model.NavigationModel.FullName %>" class="profile" />
	        <br />
	        <div class="col-6 p-v10 details">
				<span class="blue">Name:</span> <%= NameHelper.FullName(Model.NavigationModel.User)%><br />
				<span class="blue">Gender:</span> <%= Model.NavigationModel.User.Gender %><br />
				<span class="blue">Site:</span> <%= Model.NavigationModel.User.Website %><br />
	            <span class="blue">Email:</span> <%= Model.NavigationModel.User.Email %><br />
			</div>

	        <div class="col-6 details">
			    <h4>About Me</h4>
	            <%= Model.NavigationModel.User.AboutMe %>
	        </div>
	    </div>
	    
		<div class="m-lft col-18 form">
		    <% Html.RenderPartial("Message"); %>

			<% Html.RenderPartial("PrivacyTabs", UserSettings.RegionSpecifics); %>

			<div class="clear">&nbsp;</div>

		    <% using (Html.BeginForm()) { %>
                <% if (Model.Get().HasRegionClash) { %>
	    		    <div class="col-18 m-btm20">
                        <div class="col-13 push-2 m-btm10">
                            With your specified area there seems to be questions regarding who your elected official may be. 
                            Your current settings will show feeds from your elected officials AND the elcted officials that overlap in your area.
                            For example, in Chicago, three wards can share the same zip code. So instead of filtering the feed to one alderman, it will be filtered to three.
                            We solve this by giving YOU the option to choose which area you believe you are in to narrow the results.
                        </div>

                        <div class="clear">&nbsp;</div>
                        <% foreach (Pair<UserPosition, SelectList> myPair in Model.Get().RegionClashes) { %>
              		        <div class="col-8 push-2">
	    			            <div class="col-4 fnt-14 alpha omega">
	    				            <%= myPair.First.Display%>
	    			            </div>
	    			            <div class="col-2">
	    				            <%= Html.DropDownList(myPair.First.Position, myPair.Second)%>
	    			            </div>
	    		            </div>
                        <% } %>
	    		    </div>
		    	    <div class="col-2 push-2">
					    <input type="submit" class="button" value="Save" />
					    <div class="clear">&nbsp;</div>
		    	    </div>
                <% } else { %>
                    <div class="col-18 center m-btm20">
                        <div class="col-13 push-2 m-btm10">
                            Good news! There seems to be no elected official clashes in your region that you have to resolve yet. Just continue using Have a Voice!
                        </div>
                    </div>
                <% } %>
			<% } %>
			<div class="clear">&nbsp;</div>
        </div>
	</div>
</asp:Content>
