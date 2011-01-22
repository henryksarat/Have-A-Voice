<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.SwitchUserRoles>"  %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SwitchUserRoles
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>SwitchUserRoles</h2>

        <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
        <% using (Html.BeginForm()) { %>
        <%= Html.Encode(ViewData["Message"]) %><br />
           <table>
            <tr>
                <td>
                    Please select a role to move users from <br />
                    <%= Html.DropDownList("CurrentRoleId", Model.CurrentRoles)%> <%= Html.ValidationMessage("CurrentRole", "*") %>
                    <p>
                        <input type="submit" name="button" value="Get users for this role" />
                    </p>
                </td>
                <td>
                    Please select a role to move users to <br />
                    <%= Html.DropDownList("MoveToRoleId", Model.MoveToRoles)%> <%= Html.ValidationMessage("MoveToRole", "*") %>
                    <p>
                        <input type="submit" name="button" value="Move users to this role" />
                    </p>
                </td>
            </tr>
           </table>            

           <br />
           <br />

            <b>Users</b> <%= Html.ValidationMessage("UsersToMove", "*") %><br /><br />
            <% foreach (var userSelection in (Model.Users as List<Pair<User, bool>>)) { %>
                <%= CheckBoxHelper.StandardCheckbox("SelectedUserIds", userSelection.First.Id.ToString(), userSelection.Second)%>
                <%= userSelection.First.Username%>
                <br />
            <%}%>   
        <% } %>


</asp:Content>
