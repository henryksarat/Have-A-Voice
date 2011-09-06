<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UniversityOfMe.Models.Permission>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Delete Permission
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Delete</h2>
    
    <p>
    Are you sure you want to delete the permission
    <%= Model.Name %>?
    </p>
    
    <% using (Html.BeginForm(new { id = Model.Id }))
       { %>
       
       <p>
            <input type="submit" value="Delete" />
       </p>
       <% } %>
</asp:Content>
