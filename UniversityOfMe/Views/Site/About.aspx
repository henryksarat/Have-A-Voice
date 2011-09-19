<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	About Us
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaDescriptionHolder" runat="server">
	<%= UniversityOfMe.Helpers.MetaHelper.MetaDescription("About who we are and what we do") %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MetaKeywordsHolder" runat="server">
	<%= UniversityOfMe.Helpers.MetaHelper.MetaKeywords() %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    
    <div class="normal-page">
        <div class="padding-col">
            <% Html.RenderPartial("Message"); %>
            <% Html.RenderPartial("Validation"); %>

    	    <div class="heading">About Us</div>
            We are currently in beta at the University of Chicago and gathering requriements. We will fill this page out once we exit beta.
        </div>
    </div>
</asp:Content>
