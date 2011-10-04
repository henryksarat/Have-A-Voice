<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<UniversityView>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Portal for <%= Model.Get().University.UniversityName %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>
        <% if (FeatureHelper.IsFeatureEnabled(Model.User, Features.SetStatusWidget)) { %> 
            <div style="width:100%" class="create-feature-form create">
                <% using (Html.BeginForm("Create", "UserStatus", FormMethod.Post)) { %>
                    <div class="right wp100 pr10">
                        <%= Html.ActionLink("Hide", "DisableFeature", "Profile", new { feature = Features.SetStatusWidget }, new { @class = "disable-feature pr10" })%>
                    </div>
                    <div class="padding-status-col" style="text-align:center">
                        <div>
                            <table border="3" style="width:80%; margin-left:10%; margin-right:10%; border-bottom-color:Black; border-bottom-width:thick">
                                <tr>
                                    <td style="width:150px; vertical-align:middle">
                                        <div>
                                            <label for="UserStatus">What are you doing?</label> 
                                        </div>
                                    </td>
                                    <td style="vertical-align:middle">
                                        <%= Html.TextBox("UserStatus")%>
                                        <%= Html.ValidationMessage("UserStatus", "*", new { @class = "req" })%>
                                    </td>
                                    <td>
                                        <input type="submit" name="submit" class="btn site ml20" value="Share" /> 
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:150px"></td>
                                    <td style="text-align: left"><%= Html.CheckBox("Everyone") %> Display to entire university</td>
                                    <td></td>
                                </tr>

                                <% if (Model.Get().HasCurrentStatus) { %>
                                        <tr>
                                            <td style="vertical-align:top">
                                                <label for="UserStatus">Current Status</label> 
                                            </td>
                                            <td colspan="2" style="vertical-align:top; text-align: left">
                                                <%= Model.Get().CurrentStatus.Status %>    
                                            </td>
                                        </tr>
                                <% } %>

                            </table>
                        </div>
                    </div>
                <% } %>
            </div>
        <% } %>

		<div class="form"> 
            <% int myWidgetNumberDisplayed = 0; %>
            <% bool myNeedOutDiv = true; %>

            <% for(int i = 0; i < 8; i++) { %>
                <% if (myWidgetNumberDisplayed == 0 && myNeedOutDiv) { %>
			        <div class="twoCol clearfix">
                    <% myNeedOutDiv = false; %>
                <% } %>
                    <% if(i == 0) { %>
                        <% if (FeatureHelper.IsFeatureEnabled(Model.User, Features.StatusWidget)) { %> 
                            <% if (myWidgetNumberDisplayed == 0) { %>
                                    <div class="lCol">     
                            <% } else if(myWidgetNumberDisplayed == 1) { %>
                                    <div class="rCol"> 
                            <% } %>
                            <% Html.RenderPartial("UserStatuses", Model.Get().UserStatuses); %>
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
                            <% if (FeatureHelper.IsFeatureEnabled(Model.User, Features.FlirtWidget)) { %> 
                                <% if (myWidgetNumberDisplayed == 0) { %>
                                        <div class="lCol">     
                                <% } else if(myWidgetNumberDisplayed == 1) { %>
                                        <div class="rCol"> 
                                <% } %>
                                <% Html.RenderPartial("Flirts", Model.Get().AnonymousFlirts); %>
                                <% myWidgetNumberDisplayed++; %>
                                </div>
                            <% } %>
                    <% } else if(i == 3) { %>				    
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
                    <% } else if(i == 4) { %>				    
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
                    <% } else if(i == 5) { %>				    
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
                    <% } else if(i == 6) { %>				    
                        <% if (FeatureHelper.IsFeatureEnabled(Model.User, Features.OrganizationWidget) && false) { %> 
                            <% if (myWidgetNumberDisplayed == 0) { %>
                                    <div class="lCol">     
                            <% } else if(myWidgetNumberDisplayed == 1) { %>
                                    <div class="rCol"> 
                            <% } %>
                            <% Html.RenderPartial("Organizations", Model.Get().Organizations); %>
                            <% myWidgetNumberDisplayed++; %>
                            </div>
                        <% } %>
                    <% } else if (i == 7) { %>				    
                        <% if (!true) { %> 
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

