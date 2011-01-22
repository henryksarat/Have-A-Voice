<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.RoleModel>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit</h2>

    <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>
    <%= Html.Encode(ViewData["Message"]) %>
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
            <p>Restrictions</p>
            <p>
                <% foreach (var restrictionSelection in (Model.RestrictionSelection() as List<Pair<Restriction, bool>>)) { %>
                        <%=Html.RadioButton("SelectedRestriction", restrictionSelection.First.Id.ToString(), restrictionSelection.Second)%>
                        <%=restrictionSelection.First.Name%>
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

