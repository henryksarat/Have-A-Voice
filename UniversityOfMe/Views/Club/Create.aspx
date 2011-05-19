<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<CreateClubModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

    <% using (Html.BeginForm("Create", "Club", FormMethod.Post, new { enctype = "multipart/form-data", @class = "create btint-6" })) {%>
        <fieldset>
            <legend>Fields</legend>

            <div class="editor-label">
                <%: Html.Label("Club Image") %>
            </div>
            <div class="editor-field">
                <input type="file" id="ClubImage" name="ClubImage" size="23" />
                <%: Html.ValidationMessageFor(model => model.Get().ClubImage, "*")%>
            </div>
                        
            <div class="editor-label">
                <%: Html.Label("ClubType") %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownListFor(model => model.Get().ClubType, Model.Get().ClubTypes)%>
                <%: Html.ValidationMessageFor(model => model.Get().ClubType, "*")%>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Get().Name)%>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Get().Name)%>
                <%: Html.ValidationMessageFor(model => model.Get().Name)%>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Get().Description)%>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Get().Description)%>
                <%: Html.ValidationMessageFor(model => model.Get().Description)%>
            </div>
            
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>

    <% } %>
</asp:Content>

