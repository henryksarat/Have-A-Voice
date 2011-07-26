<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<EventViewModel>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="Social.Generic.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Calendar
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript" language="javascript">
		$(function() {
	        $('#StartDate').datepicker({
	            changeMonth: true,
	            changeYear: true,
	            dateFormat: "mm-dd-yy",
	            yearRange: '2011:2012'
	        });

	        $('#EndDate').datepicker({
	            changeMonth: true,
	            changeYear: true,
	            dateFormat: "mm-dd-yy",
	            yearRange: '2011:2012'
	        });
		});
	</script>

	<% Html.RenderPartial("UserPanel", Model.NavigationModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation", Model.NavigationModel); %>
    </div>
    
    <div class="col-21 events">

        <div class="action-bar bold p-a10 m-btm20 color-4">
        	Calendar
        </div>
        
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

        <% UserInformationModel<User> myUserInformationModel = HAVUserInformationFactory.GetUserInformation(); %>
        <% bool myIsUser = myUserInformationModel.Details.Id == Model.NavigationModel.User.Id; %>
        <% if (myIsUser) { %>
		    <div class="create">
			    <div class="clear">&nbsp;</div>
			    <div class="col-3">
				    <div class="p-h5 fnt-14 c-white">
					    <b>Create Event</b>
				    </div>
			    </div>

			    <div class="col-18 create">
				    <% using (Html.BeginForm("AddEvent", "Calendar", FormMethod.Post, new { @class = "create" })) { %>
					    <div class="col-7">
						    <div class="col-2 m-rgt right c-white">
							    <label for="StartDate">Start Date:</label>
							    <div class="clear">&nbsp;</div>
						    </div>
						    <div class="col-2 fnt-12">
                                <%= Html.TextBox("StartDate", Model.Get().StartDate)%>
							    <div class="clear">&nbsp;</div>
						    </div>
                            <div class="col-2 fnt-12 m-lft10">
                                <%= Html.DropDownListFor(model => Model.Get().StartTime, Model.Get().StartTimes) %>
                                <div class="clear">&nbsp;</div>
                            </div>
                            <div class="center">
                                <span class="req">
                                    <%= Html.ValidationMessage("StartDate", "*")%>
                                </span>
                                <div class="clear">&nbsp;</div>
                            </div>
						    <div class="clear">&nbsp;</div>


                            <div class="col-2 m-rgt right c-white">
							    <label for="EndDate">End Date:</label>
							    <div class="clear">&nbsp;</div>
						    </div>
						    <div class="col-2 fnt-12">
                                <%= Html.TextBox("EndDate", Model.Get().EndDate)%>
							    <div class="clear">&nbsp;</div>
						    </div>
                            <div class="col-2 fnt-12 m-lft10">
                                <%= Html.DropDownListFor(model => Model.Get().EndTime, Model.Get().EndTimes) %>
                                <div class="clear">&nbsp;</div>
                            </div>
                            <div class="center">
                                <span class="req">
                                    <%= Html.ValidationMessage("EndDate", "*")%>
                                </span>
                                <div class="clear">&nbsp;</div>
                            </div>
						    <div class="clear">&nbsp;</div>
					    </div>
					    <div class="col-8">

						    <div class="col-3 m-rgt right c-white">
							    <label for="Information">Title:</label>
							    <div class="clear">&nbsp;</div>
						    </div>
						    <div class="col-5 fnt-12">
                                <div class="col-3">
							        <%= Html.TextBox("Title")%>
                                </div>
                                <div class="col-1 center">
						            <span class="req">
						    	        <%= Html.ValidationMessage("Title", "*") %>
						            </span>
                                </div>
						    	<div class="clear">&nbsp;</div>
						    </div>
						    <div class="clear">&nbsp;</div>

						    <div class="col-3 m-rgt right c-white">
							    <label for="Information">Information:</label>
							    <div class="clear">&nbsp;</div>
						    </div>
						    <div class="col-5 fnt-12">
                                <div class="col-4">
							        <%= Html.TextArea("Information", null, new { cols = "20", rows = "2", resize = "none" })%>
                                </div>
                                <div class="col-1 right">
						    	    <span class="req">
						    		    <%= Html.ValidationMessage("Information", "*") %>
						    	    </span>
                                </div>
						    	<div class="clear">&nbsp;</div>
						    </div>
						    <div class="clear">&nbsp;</div>
					    </div>
					    <div class="col-3 m-top40">
						    <input type="submit" value="Create" class="create" />
						    <div class="clear">&nbsp;</div>
					    </div>
					    <div class="clear">&nbsp;</div>
				    <% } %>
			    </div>

			    <div class="clear">&nbsp;</div>
	        </div>
        <% } %>
	    <div class="clear">&nbsp;</div>
    	<div class="spacer-30">&nbsp;</div>
    	
    	<% int cnt = 0; %>
	    <% foreach (var item in Model.Get().Results) { %>
	    	<div class="<% if(cnt % 2 == 0) { %>row<% } else { %>alt<% } %>">
				<div class="col-6">
					<div class="p-a5">
						<div class="date-tile col-6">
                            <div class="col-1 right">
                                Start:
                            </div>
							<div class="col-4 left m-lft10">
                                <span><%= LocalDateHelper.ToLocalTime(item.StartDate) %></span>
                            </div>
						</div>
					</div>
        			<div class="p-a5">
						<div class="date-tile col-6">
                            <div class="col-1 right">
                                End:
                            </div>
							<div class="col-4 left m-lft10">
                                <span><%= LocalDateHelper.ToLocalTime(item.EndDate) %></span>
                            </div>
						</div>
					</div>
				</div>
                <div class="col-12">
        		    <div class="col-8">
					    <div class="p-a5 bold">
						    <%= item.Title %>
					    </div>
				    </div>
				    <div class="col-12">
					    <div class="p-a5">
						    <%= PresentationHelper.ReplaceCarriageReturnWithBR(item.Information) %>
					    </div>
				    </div>
                </div>
				<div class="col-2 rigbt">
					<div class="p-a5">
						<%= Html.ActionLink("Delete", "DeleteEvent", new { id = item.Id }, new { @class = "delete" }) %>
					</div>
				</div>
				<div class="clear">&nbsp;</div>
			</div>
			<div class="clear">&nbsp;</div>
			<div class="spacer-10">&nbsp;</div>
			
			<% cnt++; %>
	    <% } %>
	</div>
</asp:Content>

