<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Petition>>" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.Petitions" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="Social.Generic.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="Social.Generic.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Find a Group
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% UserInformationModel<User> myUserInfo = HAVUserInformationFactory.GetUserInformation(); %>

<div class="col-24">
    <div class="spacer-30">&nbsp;</div>
    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>
    
    <div class="clear">&nbsp;</div>
    
    <div class="push-1 col-4 center p-t5 p-b5 t-tab b-wht">
    	<span class="fnt-16 tint-6 bold">PETITIONS</span>
    	<div class="clear">&nbsp;</div>
    </div>
    <div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
    	<%= Html.ActionLink("CREATE NEW", "Create", "Petition" , null, new { @class = "issue-create" }) %>
    	<div class="clear">&nbsp;</div>
    </div>
    <div class="clear">&nbsp;</div>
    	
    <div class="b-wht m-btm10">
    	<div class="spacer-10">&nbsp;</div>
		<div class="clear">&nbsp;</div>
	</div>
    

	<% foreach (Petition myPetition in Model) { %>
	    <div class="issue-container m-btm10">            
		    <div class="push-3 col-19 m-lft issue m-rgt">
		    	<div class="p-a5">
                    <div class="push-1 col-11">
			    	    <h1><a href="/Petition/Details/<%= myPetition.Id %>"><%= myPetition.Title %></a></h1>
					    <br />
                    
			    	    <%= myPetition.Description %>
                        <br />
			    	    <div class="clear">&nbsp;</div>
                    </div>
                    <div class="push-1 col-6 right">
                            <span class="color-1">
                                <% int mySignatures = myPetition.PetitionSignatures.Count; %>
                                Signatures: <%= mySignatures%><br />
                                <% if (PetitionHelper.IsOwner(myUserInfo, myPetition)) { %>
                                    You are an admin<br />
                                    <% if(!myPetition.Active) { %>
                                        This group is NOT active
                                    <% } %>
                                <% } %>
                            </span>
                    </div>
		    	</div>
		    		
                <div class="p-a5">
			        <div class="push-4 col-3 center">
			            <span class="color-1">
			            </span>
			        </div>
			            
	                <div class="clear">&nbsp;</div>
			    </div>
		    </div>		    
		    <div class="clear">&nbsp;</div>
		</div>
	<% } %>
</div>
</asp:Content>