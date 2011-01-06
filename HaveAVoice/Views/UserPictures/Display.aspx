<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInWrapperModel<string>>" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Gallery
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Gallery</h2><br /><br />

    <%= Html.Encode(ViewData["Message"]) %><br /><br />
    
    <table>
        <tr>
            <td><img src="<%= Model.Model %>" /></td>
        </tr>
    </table>
</asp:Content>