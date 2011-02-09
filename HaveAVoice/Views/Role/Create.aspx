<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.RoleModel>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create</h2>

    <% Html.RenderPartial("Message"); %>
    <%= ViewData["RestrictionMessage"] %><br />
    <%= ViewData["PermissionMessage"] %><br />
    <% Html.RenderPartial("Validation"); %>

    <% using (Html.BeginForm("Create", "Role")) {%>
        
        <fieldset>
            <legend>Fields</legend>
            <p>
                <label for="Name">Name:</label>
                <%= Html.TextBox("Name", Model.Role.Name) %>
                <%= Html.ValidationMessage("Name", "*") %>
            </p>
            <p>
                <label for="Description">Description:</label>
                <%= Html.TextBox("Description", Model.Role.Description) %>
                <%= Html.ValidationMessage("Description", "*") %>
            </p>
            <p>
                <label for="Permissions">Select the permissions to apply:</label>
                <% foreach (var permissionSelection in (Model.PermissionSelection() as List<Pair<Permission, bool>>)) { %>
                        <%=CheckBoxHelper.StandardCheckbox("SelectedPermissions", permissionSelection.First.Id.ToString(), permissionSelection.Second) %>
                        <%=permissionSelection.First.Name %>
                    <br />
                <%}%>
            </p>
            <p>

                <label for="Restriction">Choose a Restriction:</label><br />
                <%= Html.ValidationMessage("Restriction", "*") %>
                <% foreach (var restrictionSelection in (Model.RestrictionSelection() as List<Pair<Restriction, bool>>)) { %>
                        <%= Html.RadioButton("SelectedRestriction", restrictionSelection.First.Id, restrictionSelection.Second)%>
                        <%=restrictionSelection.First.Name%>
                    <br />
                <%}%>
            </p>
            <p>
                <table>
                    <tr>
                        <td colspan="2"><label for="DefaultRole">Default Role:</label></td>
                    </tr>
                    <tr>
                        <td><label for="DefaultRole">Yes</label> <%= Html.RadioButton("DefaultRole", true, Model.Role.DefaultRole) %></td>
                        <td><label for="DefaultRole">No</label> <%= Html.RadioButton("DefaultRole", false, !Model.Role.DefaultRole)%> </td>
                    </tr>
                </table>
            </p>
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset

    <% } %>
</asp:Content>

