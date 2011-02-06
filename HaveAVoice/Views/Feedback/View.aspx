<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Models.Feedback>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	View Feedback
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="col-24">
	    <% Html.RenderPartial("Message"); %>
	    <div class="clear">&nbsp;</div>

	    <h4>View Feedback</h4>
	    <div class="clear">&nbsp;</div>

		<div class="col-6 center fnt-14 c-white bold">
			Date Sent
			<div class="clear">&nbsp;</div>
		</div>
		<div class="col-6 center fnt-14 c-white bold">
			Submitted By
			<div class="clear">&nbsp;</div>
		</div>
		<div class="col-12 center fnt-14 c-white bold">
			What They Said
			<div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>

		<% int j = 0; %>
	    <% foreach (var item in Model) { %>
	    	<div class="board-<% if (j % 2 == 0) { %>row<% } else { %>alt<% } %> p-v10 col-24 m-btm5">
				<div class="col-6">
					<%: String.Format("{0:g}", item.DateTimeStamp) %>
					<div class="clear">&nbsp;</div>
				</div>
    			<div class="col-6">
    				<%: HaveAVoice.Helpers.NameHelper.FullName(item.User) %>
    				<div class="clear">&nbsp;</div>
    			</div>
    			<div class="col-12">
    				<%: item.Text %>
    				<div class="clear">&nbsp;</div>
    			</div>
    			<div class="clear">&nbsp;</div>
    		</div>
	    	<% j++; %>
	    <% } %>
    	<div class="clear"></div>
	</div>
</asp:Content>

