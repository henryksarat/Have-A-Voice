﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<UniversityView>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	UniversityOfMe - <%= Model.Get().University.UniversityName %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

		<div class="form"> 
            <% int myWidgetNumberDisplayed = 0; %>
            <% bool myNeedOutDiv = true; %>

            <% for(int i = 0; i < 6; i++) { %>
                <% if (myWidgetNumberDisplayed == 0 && myNeedOutDiv) { %>
			        <div class="twoCol clearfix">
                    <% myNeedOutDiv = false; %>
                <% } %>

                    <% if(i == 0) { %>
                        <% if (FeatureHelper.IsFeatureEnabled(Model.User, Features.ProfessorWidget)) { %> 
                            <% if (myWidgetNumberDisplayed == 0) { %>
                                    <div class="lCol">     
                            <% } else if(myWidgetNumberDisplayed == 1) { %>
                                    <div class="rCol"> 
                            <% } %>
                            <% Html.RenderPartial("Professors", Model.Get().Professors); %>
                            <% myWidgetNumberDisplayed++; %>
                            </div>
                        <% } %>
                    <% } else if(i == 1) { %>				    
                        <% if (FeatureHelper.IsFeatureEnabled(Model.User, Features.ClassWidget)) { %> 
                            <% if (myWidgetNumberDisplayed == 0) { %>
                                    <div class="lCol">     
                            <% } else if(myWidgetNumberDisplayed == 1) { %>
                                    <div class="rCol"> 
                            <% } %>
                            <% Html.RenderPartial("Classes", Model.Get().Classes); %>
                            <% myWidgetNumberDisplayed++; %>
                            </div>
                        <% } %>
                    <% } else if(i == 2) { %>				    
                        <% if (FeatureHelper.IsFeatureEnabled(Model.User, Features.EventWidget)) { %> 
                            <% if (myWidgetNumberDisplayed == 0) { %>
                                    <div class="lCol">     
                            <% } else if(myWidgetNumberDisplayed == 1) { %>
                                    <div class="rCol"> 
                            <% } %>
                            <% Html.RenderPartial("Events", Model.Get().Events); %>
                            <% myWidgetNumberDisplayed++; %>
                            </div>
                        <% } %>
                    <% } else if(i == 3) { %>				    
                        <% if (FeatureHelper.IsFeatureEnabled(Model.User, Features.TextbookWidget)) { %> 
                            <% if (myWidgetNumberDisplayed == 0) { %>
                                    <div class="lCol">     
                            <% } else if(myWidgetNumberDisplayed == 1) { %>
                                    <div class="rCol"> 
                            <% } %>
                            <% Html.RenderPartial("Textbooks", Model.Get().TextBooks); %>
                            <% myWidgetNumberDisplayed++; %>
                            </div>
                        <% } %>
                    <% } else if(i == 4) { %>				    
                        <% if (FeatureHelper.IsFeatureEnabled(Model.User, Features.OrganizationWidget)) { %> 
                            <% if (myWidgetNumberDisplayed == 0) { %>
                                    <div class="lCol">     
                            <% } else if(myWidgetNumberDisplayed == 1) { %>
                                    <div class="rCol"> 
                            <% } %>
                            <% Html.RenderPartial("Organizations", Model.Get().Organizations); %>
                            <% myWidgetNumberDisplayed++; %>
                            </div>
                        <% } %>
                    <% } else if (i == 5) { %>				    
                        <% if (FeatureHelper.IsFeatureEnabled(Model.User, Features.GeneralPostingsWidget)) { %> 
                            <% if (myWidgetNumberDisplayed == 0) { %>
                                    <div class="lCol">     
                            <% } else if(myWidgetNumberDisplayed == 1) { %>
                                    <div class="rCol"> 
                            <% } %>
                            <% Html.RenderPartial("GeneralPostings", Model.Get().GeneralPostings); %>
                            <% myWidgetNumberDisplayed++; %>
                            </div>
                        <% } %>
                    <% } %>
				
                <% if (myWidgetNumberDisplayed == 2) { %>
                    <% myWidgetNumberDisplayed = 0; %>
                    <% myNeedOutDiv = true; %>
                    </div>
                <% } %>
		    <% } %>

            <% if (myWidgetNumberDisplayed != 0) { %>
                </div>
            <% } %>
          </div>
    </div>
</asp:Content>

