<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<GroupMember>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create Group
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% Html.RenderPartial("Message"); %>
<% Html.RenderPartial("Validation"); %>

<% foreach (GroupMember myGroupMember in Model) { %>    
    <a href="/Profile/Show/<%= myGroupMember.MemberUser.ShortUrl %>"><%= NameHelper.FullName(myGroupMember.MemberUser) %></a><br />
<% } %>
</asp:Content>