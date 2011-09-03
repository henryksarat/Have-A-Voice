<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Social.Generic.Models.MessageModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Result
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="normal-page">
        <div class="padding-col">
    	    <h3><%= Model.Title %></h3>
	        <%= Model.Details %>
        </div>
    </div>

</asp:Content>
