﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<AuthorityViewableZipCode>>" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="System.Collections" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-24">
        <div class="col-24 spacer-30">&nbsp;</div>
    
    	<div class="push-1 col-5 center p-t5 p-b5 t-tab b-wht">
    		<span class="fnt-16 tint-6 bold">AUTHORITY ZIP CODES</span>
    	</div>
    	<div class="clear">&nbsp;</div>
    	
    	<div class="b-wht">
    		<div class="col-1">&nbsp;</div>
    		<div class="col-22">
    			<div class="spacer-30">&nbsp;</div>
					<% Html.RenderPartial("Message"); %>
					<% Html.RenderPartial("Validation"); %>
					<div class="clear">&nbsp;</div>
					
					<div class="push-3 col-16 fnt-14 teal m-btm10">
						<% foreach (AuthorityViewableZipCode myZipCode in Model) { %>
                            User Id: <%= myZipCode.AuthorityUser.Id %><br />
                            Email: <%= myZipCode.AuthorityUser.Email %><br />
                            ZipCode: <%= myZipCode.ZipCode %><br /><br />
                        <% } %>
						<div class="clear">&nbsp;</div>
					</div>
				    <div class="clear">&nbsp;</div>
			</div>
			<div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>
	</div>
</asp:Content>
