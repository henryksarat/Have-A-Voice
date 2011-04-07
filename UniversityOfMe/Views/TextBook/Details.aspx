<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UniversityOfMe.Models.TextBook>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>

    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

    <fieldset>
        <legend>Fields</legend>
        
        <div class="display-label">Id</div>
        <div class="display-field"><%: Model.Id %></div>
        
        <div class="display-label">UserId</div>
        <div class="display-field"><%: Model.UserId %></div>
        
        <div class="display-label">UniversityId</div>
        <div class="display-field"><%: Model.UniversityId %></div>
        
        <div class="display-label">TextBookConditionId</div>
        <div class="display-field"><%: Model.TextBookConditionId %></div>
        
        <div class="display-label">BookTitle</div>
        <div class="display-field"><%: Model.BookTitle %></div>
        
        <div class="display-label">BookPicture</div>
        <div class="display-field"><%: Model.BookPicture %></div>
        
        <div class="display-label">ClassCode</div>
        <div class="display-field"><%: Model.ClassCode %></div>
        
        <div class="display-label">BuySell</div>
        <div class="display-field"><%: Model.BuySell %></div>
        
        <div class="display-label">Edition</div>
        <div class="display-field"><%: Model.Edition %></div>
        
        <div class="display-label">Price</div>
        <div class="display-field"><%: String.Format("{0:F}", Model.Price) %></div>
        
        <div class="display-label">Details</div>
        <div class="display-field"><%: Model.Details %></div>
        
        <div class="display-label">DateTimeStamp</div>
        <div class="display-field"><%: String.Format("{0:g}", Model.DateTimeStamp) %></div>
        
        <div class="display-label">Active</div>
        <div class="display-field"><%: Model.Active %></div>

        <% if (HaveAVoice.Helpers.UserInformation.UserInformationFactory.GetUserInformation().Details.Id == Model.UserId) { %>
            <% using (Html.BeginForm("MarkAsNonActive", "TextBook", new { id = Model.Id })) {%>
                <p>
                    <input type="submit" value="Create" />
                </p>
            <% } %>
        <% } %>
        
    </fieldset>
    <p>

        <%: Html.ActionLink("Edit", "Edit", new { id=Model.Id }) %> |
        <%: Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

