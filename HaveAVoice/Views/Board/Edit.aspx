﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.BoardWrapper>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	EditBoardMessage
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit Message</h2>

    <% Html.RenderPartial("Message"); %>

    <p>
        <%= Html.ValidationSummary("Board post was unsuccessful. Please correct the errors and try again.") %>
    </p>
 
    <p>Original Board Message:</p>
    <p><%= Model.Message %></p>

    <% using (Html.BeginForm()) {%>
        <p>
            <%= Html.TextArea("Message", new { style = "width:300px; height: 200px" })%>
            <%= Html.ValidationMessage("Message", "*")%>
        </p>  
        <p>
            <input type="submit" value="Post" />
        </p>  
    <%} %>

</asp:Content>
