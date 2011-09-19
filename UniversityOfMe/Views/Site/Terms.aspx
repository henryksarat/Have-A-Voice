<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Terms of Use
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MetaDescriptionHolder" runat="server">
	<%= UniversityOfMe.Helpers.MetaHelper.MetaDescription("Terms of Use") %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MetaKeywordsHolder" runat="server">
	<%= UniversityOfMe.Helpers.MetaHelper.MetaKeywords() %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="normal-page">
        <div class="padding-col">
    	    <div class="heading">Terms of Use</div>
            <div class="sub-heading">Your Information and Content</div>
            Any content or information you post is owned by you. You may edit all of the content and information you post. However, items you post and then delete may still be backed up securely where no one has access.<br /><br />

            <div class="sub-heading">Activity</div>
            You will not use your Have a Voice™ to post unauthorized commercial communications (spam).<br />
            You will not intimidate or harass other members.<br />
            You will not impair the proper working of Have a Voice.<br />
            You will not do anything unlawful or malicious.<br />
            You will not try to gain access to other person’s accounts.<br />
            You will not pretend to be a politician or political candidate.<br /><br />

            <div class="sub-heading">Account Termination</div>
            If you violate any of these terms then we have the right to delete or disable your account. We will notify you through email and the next time you login.<br /><br />

            <div class="sub-heading">Changes</div>
            We reserve the right to change these terms of use. Please check back to the terms of use page periodically to check for any changes. <br /><br />
        </div>
    </div>

</asp:Content>

