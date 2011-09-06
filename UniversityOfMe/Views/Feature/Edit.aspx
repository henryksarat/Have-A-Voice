<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInListModel<Pair<Feature, bool>>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="Social.Generic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Customize Site Features
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        
        <div class="create-feature-form">
		    <div class="banner title black full red-top small"> 
			    <span class="edit">EDIT YOUR SITE SETTINGS</span> 
            </div>

            <% Html.RenderPartial("Validation"); %>

            <div class="padding-col">
		        <% using (Html.BeginForm()) { %>
                    <% foreach(Pair<Feature, bool> myPair in Model.Get()) { %>
		            <div class="wp100 mt13" style="display:inline-block">		    		
			            <div class="flft">
			                <div class="bold"><%= myPair.First.DisplayName%></div>
                            <div class="small mt-6"><%= myPair.First.Description %></div>
			            </div>
			            <div class="frgt mr26 small">
			                Yes <%= Html.RadioButton(myPair.First.Name, true, myPair.Second)%>
			                No <%= Html.RadioButton(myPair.First.Name, false, !myPair.Second)%>
			            </div>
		            </div>
                    <% } %>
		            <div class="right"> 
				        <input type="submit" class="btn site mr26" value="Save" />
		            </div>
		        <% } %>
            </div>
        </div>
	</div>
	<div class="clear">&nbsp;</div>

</asp:Content>
