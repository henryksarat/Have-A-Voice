<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<EventViewModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

    <% using (Html.BeginForm()) {%>
        <fieldset>
            <legend>Fields</legend>
            
            <div class="editor-label">
                <%: Html.Label("Event privacy option") %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownListFor(model => model.Get().EventPrivacyOption, Model.Get().EventPrivacyOptions)%>
                <%: Html.ValidationMessageFor(model => model.Get().EventPrivacyOption, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.LabelFor(model => model.Get().Title)%>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Get().Title)%>
                <%: Html.ValidationMessageFor(model => model.Get().Title)%>
            </div>
            
            <div class="editor-label">
                <%: Html.Label("Start Date") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Get().StartDate)%>
                <%: Html.ValidationMessageFor(model => model.Get().StartDate)%>
            </div>
            
            <div class="editor-label">
                <%: Html.Label("End Date") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Get().EndDate)%>
                <%: Html.ValidationMessageFor(model => model.Get().EndDate)%>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Get().Information)%>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Get().Information)%>
                <%: Html.ValidationMessageFor(model => model.Get().Information)%>
            </div>
            
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>

    <% } %>
</asp:Content>

