<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DisplayPetitionModel>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.Petitions" %>
<%@ Import Namespace="HaveAVoice.Helpers.Constants" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="Social.Generic.Models" %>
<%@ Import Namespace="Social.Generic.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Petition - <%= Model.Petition.Title %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% UserInformationModel<User> myUserInfo = HAVUserInformationFactory.GetUserInformation(); %>

<div class="col-24 m-btm30">
	<div class="spacer-30">&nbsp;</div>

	<% Html.RenderPartial("Message"); %>
	<% Html.RenderPartial("Validation"); %>
	<div class="clear">&nbsp;</div>

    <div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
        <a class="issue-create" href="/Petition/List">PETITIONS</a>
    	<div class="clear">&nbsp;</div>
    </div>
    <div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
    	<%= Html.ActionLink("CREATE PETITION", "Create", "Petition", null, new { @class = "issue-create" }) %>
    	<div class="clear">&nbsp;</div>
    </div>
    <div class="push-1 col-14 center p-t5 p-b5 t-tab b-wht">
    	<span class="fnt-16 tint-6 bold"><%= Model.Petition.Title%></span>
    	<div class="clear">&nbsp;</div>
    </div>
    <div class="clear">&nbsp;</div>
    	
    <div class="b-wht m-btm10">
    	<div class="spacer-10">&nbsp;</div>
		<div class="clear">&nbsp;</div>
	</div>
		
    <div class="push-20 alpha col-2 omega">
		<div class="p-a5">
            <a href="http://twitter.com/share" class="twitter-share-button" data-count="none" data-via="haveavoice_">Tweet</a><script type="text/javascript" src="http://platform.twitter.com/widgets.js"></script>
        </div>
        <div class="clear">&nbsp;</div>
    </div>

    <div class="push-20 alpha col-2">
		<div class="p-a5">
            <script src="http://connect.facebook.net/en_US/all.js#xfbml=1"></script><fb:like href="#" layout="button_count" show_faces="false" width="90" font="arial"></fb:like>
        </div>
        <div class="clear">&nbsp;</div>
    </div>
    <div class="clear">&nbsp;</div>

	    <div class="clear">&nbsp;</div>
	        
	    <div class="m-btm10">
            <div class="push-1 m-lft col-16 m-rgt comment">
                <div class="p-a10">
                    <h1><a href="/Petition/Details/<%= Model.Petition.Id %>"><%= Model.Petition.Title%></a></h1>
                    <br><%= Model.Petition.Description %>
                    <div class="clear">&nbsp;
                    </div>
                </div>
            </div>

            <div class="push-1 col-6 stats fnt-12 m-rgt">
                <div class="clear">&nbsp;</div>
                <div class="p-a5">
                    <h4 class="m-btm5">Petition Information:</h4>
                    <div class="m-btm5"><span class="bold">Creation Date:</span> <%= LocalDateHelper.ToLocalTime(Model.Petition.DateTimeStamp)%></div>
                    <div class="m-btm5">
                        <span class="bold">Created By:</span> 
                        <a class="petitionlink" href="<%= LinkHelper.Profile(Model.Petition.User) %>">
                            <%= NameHelper.FullName(Model.Petition.User) %>
                        </a>
                    </div>
                    <div class="m-lft10 m-btm5">
                        <% if(myUserInfo == null || myUserInfo.UserId != Model.Petition.UserId) { %>
                            <a class="petitionlink" href="<%= LinkHelper.SendMessage(Model.Petition.User, "Petition Question: " + Model.Petition.Title)%>">
                                Ask a the creator a question
                            </a>
                        <% } %>
                    </div>
                    <% int myPetitionSignatures = Model.Petition.PetitionSignatures.Count<PetitionSignature>(); %>
                    <div class="m-btm5"><span class="bold">Active: </span><%= Model.Petition.Active ? "YES" : "NO" %></div>
                    <div class="m-btm5"><span class="bold">City Origin: </span><%= Model.Petition.City %></div>
                    <div class="m-btm5"><span class="bold">State Origin: </span><%= Model.Petition.State%></div>
                    <div class="m-btm5"><span class="bold">Zip Code Origin: </span><%= Model.Petition.Zip%></div>
                    <div class="m-btm5"><span class="bold">Signatures: </span><%= myPetitionSignatures%></div>

                    <% if(PetitionHelper.IsOwner(myUserInfo, Model.Petition) && Model.Petition.Active) { %>
                        <%= Html.ActionLink("Close Petition", "MarkPetitionAsFinished", "Petition", new { id = Model.Petition.Id }, new { @class = "petitionlink" })%>
                    <% } %>
                </div>
            </div>
		</div>
		<div class="clear">&nbsp;</div>

        <div class="petition m-top10 m-btm30">
            <div class="push-6 m-lft col-14 m-rgt header">    
                <div class="p-a10">
                    Petition Signatures
                </div>
            </div>
        </div>
            <% bool myHasSigned = PetitionHelper.HasSigned(myUserInfo, Model.Petition.Id); %>
            <% if (!myHasSigned && Model.Petition.Active) { %>
	            <div class="clear">&nbsp;</div>
                <%= Html.Hidden("PetitionId", Model.Petition.Id)%>
                <% string myProfilePicture = Social.Generic.Constants.Constants.ANONYMOUS_PICTURE_URL; %>
                <% string myFullName = string.Empty; %>
                <% bool myIsLoggedIn = HAVUserInformationFactory.IsLoggedIn();  %>
                <div class="reply">
					<div class="row">
						<div class="col-14 comment push-6">
							<div class="msg-2">
								To sign this petition you must provide your address. Only the petition creator and administrators of Have A Voice will be able to view your contact information.
							</div>
							<div class="clear">&nbsp;</div>
						</div>
                        <div class="clear">&nbsp;</div>
					</div>
                    <div class="clear">&nbsp;</div>
				</div>
		        <div class="reply m-btm10 m-top10">
			        <div class="row">
				    <div class="push-5 col-2 center">
					    <img src="<%= myProfilePicture %>" alt="<%= myFullName %>" class="profile" />
					    <div class="clear">&nbsp;</div>
				    </div>
				    <div class="push-5 m-lft col-14 comment">

					    <span class="speak-lft">&nbsp;</span>
                        <% using (Html.BeginForm("Create", "PetitionSignature", FormMethod.Post)) { %>
					        <%= Html.Hidden("PetitionId", Model.Petition.Id) %>
                            <div class="col-9 p-a5">
	    			            <div class="col-3">
	    				            <label for="FirstName">Address</label>
	    			            </div>
	    			            <div class="col-4 m-rgt5">
	    				            <%= Html.TextBox("Address")%>
	    			            </div>
	    			            <div class="col-1">
	    				            <span class="req">
		    				            <%= Html.ValidationMessage("Address", "*")%>
	    				            </span>
	    			            </div>
                            </div>

                            <div class="col-13 p-a5">
                                <div class="col-9">
	    			                <div class="col-3">
	    				                <label for="Alias">City</label>
	    			                </div>
	    			                <div class="col-4 m-rgt5">
	    				                <%= Html.TextBox("City")%>
	    			                </div>
	    			                <div class="col-1">
	    				                <span class="req">
		    				                <%= Html.ValidationMessage("City", "*")%>
	    				                </span>
	    			                </div>
                                </div>
                                <div class="col-4">
                                    <div class="col-1">
                                        <%= Html.Label("State:")%>
                                    </div>
                                    <div class="col-2">
                                        <%= Html.DropDownList("State", Model.States)%>
                                    </div>
	    			                <div class="col-1">
	    				                <span class="req">
		    				                <%= Html.ValidationMessage("State", "*")%>
	    				                </span>
	    			                </div>
                                    </div>
                            </div>
                            <div class="col-9 p-a5">
	    			            <div class="col-3">
	    				            <label for="Alias">Zip Code</label>
	    			            </div>
	    			            <div class="col-4 m-rgt5">
	    				            <%= Html.TextBox("ZipCode")%>
	    			            </div>
	    			            <div class="col-1">
	    				            <span class="req">
		    				            <%= Html.ValidationMessage("ZipCode", "*")%>
	    				            </span>
	    			            </div>
                            </div>
                            <div class="col-9 p-a5">
	    			            <div class="col-3">
	    				            <label for="PhoneNumber">Email(optional)</label>
	    			            </div>
	    			            <div class="col-4 m-rgt5">
	    				            <%= Html.TextBox("Email")%>
	    			            </div>
	    			            <div class="col-1">
	    				            <span class="req">
		    				            <%= Html.ValidationMessage("Email", "*")%>
	    				            </span>
	    			            </div>
                            </div>
                            <div class="col-13 p-a5">
	    			            <div class="col-3">
	    				            <label for="PhoneNumber">Comment</label>
	    			            </div>
	    			            <div class="col-9 m-rgt5">
	    				            <%= Html.TextArea("Comment", string.Empty, 3, 45, new { resize = "none", @class = "comment" })%>
	    			            </div>
                            </div>
								
						    <div class="clear">&nbsp;</div>
						    <div class="col-13">
							    <input type="submit" value="Sign" class="button" />
							    <div class="clear">&nbsp;</div>
						    </div>
						    <div class="clear">&nbsp;</div>
					    </div>
				        <% } %>
                    </div>
				    <div class="clear">&nbsp;</div>
			    </div>
                </div>
        
        <% } %>

        <div class="clear">&nbsp;</div>
        <% foreach (PetitionSignature mySignature in Model.Petition.PetitionSignatures) { %>
            <div class="petition m-btm10 m-top10">
                <div class="push-5 col-2 center">
                    <a href="<%= LinkHelper.Profile(mySignature.User) %>">
                        <img alt="<%= NameHelper.FullName(mySignature.User) %>" class="profile" src="<%= PhotoHelper.ProfilePicture(mySignature.User) %>"/ >
                    </a>
                    <div class="clear">&nbsp;</div>
                </div>
                <div class="push-5 m-lft col-12 m-rgt comment">
                    <span class="speak-lft">&nbsp;</span>
                    <div class="p-a10">
                        <a class="petition-name" href="<%= LinkHelper.Profile(mySignature.User) %>">
                            <%= NameHelper.FullName(mySignature.User) %>
                            </a>
                            <% bool myHasComment = !string.IsNullOrEmpty(mySignature.Comment); %>
                            &nbsp;<%= myHasComment ? mySignature.Comment : "No comment" %>
                        <div class="clear">&nbsp;</div>
                        <% if(Model.ViewSignatureDetails) { %>
                            <div class="col-11 options">
                                <div class="p-v10">
                                    <div class="col-6 center"><span class="bold">Address:</span> <%= mySignature.Address %> <%= mySignature.City %>, <%= mySignature.State %> </div>
                                    <% bool myHasEmail = !string.IsNullOrEmpty(mySignature.Email); %>
                                    <div class="col-5 center"><span class="bold">Email:</span> <%= myHasEmail ? mySignature.Email : "NA"%> </div>
                                    <div class="clear">&nbsp;</div>
                                </div>
                            </div>
                        <% } %>
                    </div>
                </div>
                <div class="col-3 date-tile push-5">
                    <div class="p-a10">
                        <div class="">
                            <span><%= LocalDateHelper.ToLocalTime(mySignature.DateTimeStamp) %></span>
                        </div>
                    </div>
                    <div class="clear">&nbsp;</div>
                </div>
                <div class="clear">&nbsp;</div>
            </div>
            <div class="clear">&nbsp;</div>
        <% } %>
</div>
</asp:Content>