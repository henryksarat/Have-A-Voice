<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UniversityOfMe.Models.Permission>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Permission
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Fields</legend>
            <p>
                <label for="Name">Name:</label>
                <%= Html.TextBox("Name") %>
                <%= Html.ValidationMessage("Name", "*") %>
            </p>
            <p>
                <label for="Description">Description:</label>
                <%= Html.TextBox("Description") %>
                <%= Html.ValidationMessage("Description", "*") %>
            </p>
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

