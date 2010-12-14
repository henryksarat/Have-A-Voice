<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.User>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ListenToUser
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>ListenToUser</h2>

    You are about to become a listener of <%= Html.Encode(Model.Username) %>, which means you will get updates when he posts new comments on issues.

    <% using (Html.BeginForm(new { id = Model.Id }))
       { %>
       
       <p>
            <input type="submit" value="Listen" />
       </p>
       <% } %>
</asp:Content>

