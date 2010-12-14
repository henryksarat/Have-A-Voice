<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Admin Home</h2>

    <%= Html.ActionLink("Role Management", "Index", "Role", null, null) %>
    <%= Html.ActionLink("Permission Management", "Index", "Permission", null, null) %>
    <%= Html.ActionLink("Restriction Management", "Index", "Restriction", null, null) %>
    <%= Html.ActionLink("Edit User Roles", "SwitchUserRoles", "Role", null, null) %>

</asp:Content>
