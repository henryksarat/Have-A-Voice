<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UniversityOfMe.Models.View.CreateTextBookModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create</h2>

    <% using (Html.BeginForm("Create", "TextBook", FormMethod.Post, new { enctype = "multipart/form-data", @class = "create btint-6" })) {%>
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

        <fieldset>
            <div class="editor-label">
                <%: Html.Label("Book Title") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.BookTitle)%>
                <%: Html.ValidationMessageFor(model => model.BookTitle, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Book Condition") %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownListFor(model => model.TextBookCondition, Model.TextBookConditions)%>
                <%: Html.ValidationMessageFor(model => model.TextBookCondition, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Book Image") %>
            </div>
            <div class="editor-field">
                <input type="file" id="BookImage" name="BookImage" size="23" />
                <%: Html.ValidationMessageFor(model => model.BookImage, "*")%>
            </div>
                     
            <div class="editor-label">
                <%: Html.Label("Class Code") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.ClassCode)%>
                <%: Html.ValidationMessageFor(model => model.ClassCode, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Edition") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Edition)%>
                <%: Html.ValidationMessageFor(model => model.Edition, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Buy/Sell") %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownListFor(model => model.BuySell, Model.BuySellOptions)%>
                <%: Html.ValidationMessageFor(model => model.BuySell, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Price") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Price)%>
                <%: Html.ValidationMessageFor(model => model.Price, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Details") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Details)%>
                <%: Html.ValidationMessageFor(model => model.Details, "*")%>
            </div>
            
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>

    <% } %>
</asp:Content>
