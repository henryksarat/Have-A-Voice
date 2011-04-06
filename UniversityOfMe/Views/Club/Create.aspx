<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UniversityOfMe.Models.View.CreateClubModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create</h2>

    <% using (Html.BeginForm("Create", "Club", FormMethod.Post, new { enctype = "multipart/form-data", @class = "create btint-6" })) {%>
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>

        <fieldset>
            <legend>Fields</legend>

            <div class="editor-label">
                <%: Html.Label("Club Image") %>
            </div>
            <div class="editor-field">
                <input type="file" id="ClubImage" name="ClubImage" size="23" />
                <%: Html.ValidationMessageFor(model => model.ClubImage, "*")%>
            </div>
                        
            <div class="editor-label">
                <%: Html.Label("ClubType") %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownListFor(model => model.ClubType, Model.ClubTypes)%>
                <%: Html.ValidationMessageFor(model => model.ClubType, "*")%>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Name) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Name) %>
                <%: Html.ValidationMessageFor(model => model.Name) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Description) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Description) %>
                <%: Html.ValidationMessageFor(model => model.Description) %>
            </div>
            
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>

    <% } %>
</asp:Content>

