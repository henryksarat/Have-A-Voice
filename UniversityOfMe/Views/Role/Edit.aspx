<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<RoleViewModel>" %>
<%@ Import Namespace="Social.Generic" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>
<%@ Import Namespace="Social.ViewHelpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit</h2>

    <% Html.RenderPartial("Message"); %>
    <%= ViewData["PermissionMessage"] %><br />
    <% Html.RenderPartial("Validation"); %>
    
    <% using (Html.BeginForm()) {%>
    
        <fieldset>
            <legend>Fields</legend>
            <p>
                <label for="Name">Name:</label>
                <%= Html.TextBox("Name", Model.Role.Name)%>
                <%= Html.ValidationMessage("Name", "*") %>
            </p>
            <p>
                <label for="Description">Description:</label>
                <%= Html.TextBox("Description", Model.Role.Description)%>
                <%= Html.ValidationMessage("Description", "*") %>
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
            <p>Permissions</p>
            <p>
                <% foreach (var permissionSelection in (Model.PermissionSelection() as List<Pair<Permission, bool>>)) { %>
                        <%=CheckBoxHelper.StandardCheckbox("SelectedPermissions", permissionSelection.First.Id.ToString(), permissionSelection.Second) %>
                        <%=permissionSelection.First.Name %>
                    <br />
                <%}%>              
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

