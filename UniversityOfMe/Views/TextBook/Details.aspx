<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<TextBook>>" %>
<%@ Import Namespace="UniversityOfMe.UserInformation" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

        
        <div class="display-label">Id</div>
        <div class="display-field"><%: Model.Get().Id %></div>
        
        <div class="display-label">UserId</div>
        <div class="display-field"><%: Model.Get().UserId%></div>
        
        <div class="display-label">UniversityId</div>
        <div class="display-field"><%: Model.Get().UniversityId%></div>
        
        <div class="display-label">TextBookConditionId</div>
        <div class="display-field"><%: Model.Get().TextBookConditionId%></div>
        
        <div class="display-label">BookTitle</div>
        <div class="display-field"><%: Model.Get().BookTitle%></div>
        
        <div class="display-label">BookPicture</div>
        <div class="display-field"><%: Model.Get().BookPicture%></div>
        
        <div class="display-label">ClassCode</div>
        <div class="display-field"><%: Model.Get().ClassCode%></div>
        
        <div class="display-label">BuySell</div>
        <div class="display-field"><%: Model.Get().BuySell%></div>
        
        <div class="display-label">Edition</div>
        <div class="display-field"><%: Model.Get().Edition%></div>
        
        <div class="display-label">Price</div>
        <div class="display-field"><%: String.Format("{0:F}", Model.Get().Price)%></div>
        
        <div class="display-label">Details</div>
        <div class="display-field"><%: Model.Get().Details%></div>
        
        <div class="display-label">DateTimeStamp</div>
        <div class="display-field"><%: String.Format("{0:g}", Model.Get().DateTimeStamp)%></div>
        
        <div class="display-label">Active</div>
        <div class="display-field"><%: Model.Get().Active%></div>

        <% if (UserInformationFactory.GetUserInformation().Details.Id == Model.Get().UserId) { %>
            <% using (Html.BeginForm("MarkAsNonActive", "TextBook", new { id = Model.Get().Id })) {%>
                <p>
                    <input type="submit" value="Delete Textbook Entry" />
                </p>
            <% } %>
        <% } %>
</asp:Content>

