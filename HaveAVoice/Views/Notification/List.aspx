﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.LoggedInListModel<Board>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Fans
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<% Html.RenderPartial("UserPanel", Model.UserModel); %>
    <div class="col-3 m-rgt left-nav">
        <% Html.RenderPartial("LeftNavigation"); %>
    </div>
    
    <div class="col-21">
        <%= Html.Encode(ViewData["Message"]) %>
        <div class="clear">&nbsp;</div>
    
	    <% foreach (var item in Model.Models) { %>
	        <%= Html.ActionLink("Now activity with this board message:" + item.Message, "View", "Board", new { id = item.Id }, null)%><br />
	    <% } %>
	    <div class="clear">&nbsp;</div>
	</div>
</asp:Content>