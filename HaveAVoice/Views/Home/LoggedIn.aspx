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

        <h2>Welcome, <%= Model.User.Username %></h2>
    
        <br /><br />
        FanFeed:<br />
        <% foreach (var item in Model.FanIssueReplys) { %>
            Username: <%= item.User.Username %>
            IssueReply: <%= item.Reply %><br /><br />
        <% } %>

        <br /> /><br />
        Officials Feed:<br />
        <% foreach (var item in Model.OfficialsReplys) { %>
            Username: <%= item.User.Username %>
            IssueReply: <%= item.Reply %><br /><br />
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