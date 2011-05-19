<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<CreateTextBookModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

    <% using (Html.BeginForm("Create", "TextBook", FormMethod.Post, new { enctype = "multipart/form-data", @class = "create btint-6" })) {%>
        <fieldset>
            <div class="editor-label">
                <%: Html.Label("Book Title") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Get().BookTitle)%>
                <%: Html.ValidationMessageFor(model => model.Get().BookTitle, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Book Condition") %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownListFor(model => model.Get().TextBookCondition, Model.Get().TextBookConditions)%>
                <%: Html.ValidationMessageFor(model => model.Get().TextBookCondition, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Book Image") %>
            </div>
            <div class="editor-field">
                <input type="file" id="BookImage" name="BookImage" size="23" />
                <%: Html.ValidationMessageFor(model => model.Get().BookImage, "*")%>
            </div>
                     
            <div class="editor-label">
                <%: Html.Label("Class Code") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Get().ClassCode)%>
                <%: Html.ValidationMessageFor(model => model.Get().ClassCode, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Edition") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Get().Edition)%>
                <%: Html.ValidationMessageFor(model => model.Get().Edition, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Buy/Sell") %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownListFor(model => model.Get().BuySell, Model.Get().BuySellOptions)%>
                <%: Html.ValidationMessageFor(model => model.Get().BuySell, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Price") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Get().Price)%>
                <%: Html.ValidationMessageFor(model => model.Get().Price, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Details") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Get().Details)%>
                <%: Html.ValidationMessageFor(model => model.Get().Details, "*")%>
            </div>
            
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>

    <% } %>
</asp:Content>
