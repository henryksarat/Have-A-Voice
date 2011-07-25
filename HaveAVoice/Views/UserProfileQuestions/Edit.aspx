<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<Dictionary<string, IEnumerable<Pair<UserProfileQuestion, QuestionAnswer>>>>>" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Social.Generic" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers.ProfileQuestions" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Your User Profile Questionnaire
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

			<% Html.RenderPartial("PrivacyTabs", UserSettings.ProfileQuestions); %>

			<div class="clear">&nbsp;</div>

		    <% using (Html.BeginForm()) { %>
		    	<% foreach (string myGroup in Model.Model.Keys) { %>
                    <h4><%= myGroup %></h4>
                    <div class="clear">&nbsp;</div>
                    <% foreach(Pair<UserProfileQuestion, QuestionAnswer> myPair in Model.Model[myGroup]) { %>
		    		<div class="push-1 col-15 m-btm15">		    		
			            <div class="col-6">
			                <label><%= myPair.First.DisplayQuestion%></label>
                            <%= myPair.First.Description %>
			            </div>
			            <div class="push-1 col-8">
                            <% if (myPair.First.Name == ProfileQuestion.POLITICAL_AFFILIATION.ToString()) { %>
                                <%= Html.RadioButton(myPair.First.Name, PoliticalAffiliation.Democrat, (int)myPair.Second == (int)PoliticalAffiliation.Democrat)%> Democrat
                                <%= Html.RadioButton(myPair.First.Name, PoliticalAffiliation.Republican, (int)myPair.Second == (int)PoliticalAffiliation.Republican)%> Republican
                                <%= Html.RadioButton(myPair.First.Name, PoliticalAffiliation.Indepedent, (int)myPair.Second == (int)PoliticalAffiliation.Indepedent)%> Independent
                                <%= Html.RadioButton(myPair.First.Name, PoliticalAffiliation.NoAnswer, (int)myPair.Second == (int)PoliticalAffiliation.NoAnswer)%> No Answer
                            <% } else if (myPair.First.Name == ProfileQuestion.ABORTION.ToString()) { %>
			                    <%= Html.RadioButton(myPair.First.Name, AbortionAnswer.ProLife, (int)myPair.Second == (int)AbortionAnswer.ProLife)%> Pro-Life
			                    <%= Html.RadioButton(myPair.First.Name, AbortionAnswer.ProChoice, (int)myPair.Second == (int)AbortionAnswer.ProChoice)%> Pro-Choice
                                <%= Html.RadioButton(myPair.First.Name, QuestionAnswer.NoAnswer, (int)myPair.Second == (int)AbortionAnswer.NoAnswer)%> No Answer
                            <% } else { %>
			                    <%= Html.RadioButton(myPair.First.Name, QuestionAnswer.Yes, myPair.Second == QuestionAnswer.Yes)%> Yes
			                    <%= Html.RadioButton(myPair.First.Name, QuestionAnswer.No, myPair.Second == QuestionAnswer.No)%> No
                                <%= Html.RadioButton(myPair.First.Name, QuestionAnswer.NoAnswer, myPair.Second == QuestionAnswer.NoAnswer)%> No Answer
                            <% } %>
			            </div>
			            <div class="clear">&nbsp;</div>
		            </div>
		            <div class="clear">&nbsp;</div>
                    <% } %>
		    	<% } %>
		    	<div class="push-7 col-4">
					<input type="submit" class="button" value="Save" />
					<input type="button" class="button" value="Cancel" />
					<div class="clear">&nbsp;</div>
		    	</div>
			<% } %>
			<div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>
	</div>
</asp:Content>
