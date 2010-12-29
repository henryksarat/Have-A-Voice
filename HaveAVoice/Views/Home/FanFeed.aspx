<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInModel>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Logged In
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("UserPanel"); %>    
    <div class="col-3 left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>
    
    <div class="col-21">
        <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
    
        FanFeed:<br />
        <% foreach (var item in Model.IssueReplys) { %>
        
			<div class="row">
				<div class="col-2 center">
					<img src="<% /* = item.User.ProfilePictureURL */ %>" alt="<%= item.User.Username %>" class="profile" />
				</div>
				<div class="col-16">
					<div class="m-lft col-16 comment">
						<span class="speak-lft">&nbsp;</span>
						<div class="p-a10">
							<a class="name" href="#"><%= item.User.Username %></a>
							<%= item.Reply %>
							<div class="clear">&nbsp;</div>
							
							<div class="spacer-10">&nbsp;</div>
							<div class="options">
								<div class="col-6">&nbsp;</div>
								<div class="col-9">
									<div class="col-3 center">
										<a href="#" class="comment">COMMENT</a>
									</div>
									<div class="col-3 center">
										<a href="#" class="like">LIKE</a>
									</div>
									<div class="col-3 center">
										<a href="#" class="dislike">DISLIKE</a>
									</div>
								</div>
							</div>
							<div class="spacer-10">&nbsp;</div>
						</div>
					</div>
				</div>
				<div class="col-3">
					<div class="p-a5">
						<div class="date-tile">
							<span>3:47</span> AM
						</div>
					</div>
				</div>
				<div class="clear">&nbsp;</div>
				
        <% } %>
   
        <br /><br />

        <% using (Html.BeginForm("AddZipCodeFilter", "Home")) { %>
            Zip Code:
            <%= Html.TextBox("ZipCode") %>
            <%= Html.ValidationMessage("ZipCode", "*")%>
            <p>
                <input type="submit" value="Submit" />
            </p>
        <% } %>
        <br /><br />

        <% using (Html.BeginForm("AddCityStateFilter", "Home")) { %>
            City:
            <%= Html.TextBox("City", "")%>
            <%= Html.ValidationMessage("City", "*")%><br />
            State:
            <%= Html.TextBox("State", "")%>
            <%= Html.ValidationMessage("State", "*")%><br />
            <p>
                <input type="submit" value="Submit" />
            </p>
        <% } %>
    </div>
</asp:Content>