<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.Role>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Delete
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Delete</h2>
    
    <%= Html.Encode(ViewData["Message"]) %>

    <p>
    Are you sure you want to delete the role
    <%= Model.Name %>?
    </p>
    
    <% using (Html.BeginForm(new { id = Model.Id }))
       { %>
       
       <p>
            <input type="submit" value="Delete" />
       </p>
       <% } %>
</asp:Content>
