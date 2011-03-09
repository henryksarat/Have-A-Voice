<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Search
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript" language="javascript" src="/Content/jquery.autocomplete.js"></script>
	<script type="text/javascript" language="javascript" src="/Content/localdata.js"></script>

	<link rel="stylesheet" type="text/css" href="/Content/jquery.autocomplete.css" />
	
	<script type="text/javascript">
	    $(function() {
	        if ($("#SearchType").val() === "User") {
	            $("#SearchQuery").focus().autocomplete("/Search/getUserAjaxResult");
	        } else {
	            $("#SearchQuery").focus().autocomplete("/Search/getIssueAjaxResult");
	        }
	
	        $("#SearchType").change(function (e) {
	            if ($("#SearchType").val() === "User") {
	                $("#SearchQuery").focus().autocomplete("/Search/getUserAjaxResult");
	            } else {
	                $("#SearchQuery").focus().autocomplete("/Search/getIssueAjaxResult");
	            }
	        });
	        $("#clear").click(function () {
	            $(":input").unautocomplete();
	        });
	    });
	</script>

    <div class="col-24">
        <div class="spacer-30">&nbsp;</div>
    
    	<% Html.RenderPartial("Message"); %>
    	<div class="clear">&nbsp;</div>
    
    	<div class="push-1 col-4 center p-t5 p-b5 t-tab b-wht">
    		<span class="fnt-16 tint-6 bold">SEARCH</span>
    		<div class="clear">&nbsp;</div>
    	</div>
    	<div class="clear">&nbsp;</div>
    	
    	<div class="b-wht m-btm10">
    		<div class="spacer-10">&nbsp;</div>
			<div class="clear">&nbsp;</div>
		</div>
		
		<div class="col-24 m-btm10">
			<!-- null, null, FormMethod.Post, new { @class = "create" } -->
		    <% using (Html.BeginForm("DoSearch", "Search")) { %>
                <div class="push-6 col-2 m-rgt right">
                	<label for="SearchType">Search:</label>
                	<div class="clear">&nbsp;</div>
                </div>
                <div class="push-6 col-2">
                	<%= Html.DropDownList("SearchType", new SelectList(new List<string>() { "User", "Issue" }, "User"))  %>
                	<div class="clear">&nbsp;</div>
                </div>
                <div class="push-6 col-6">
                	<%= Html.TextBox("SearchQuery") %>
                	<div class="clear">&nbsp;</div>
                </div>
                <div class="push-6 col-2 center">
                	<input type="submit" value="Search" class="button" />
                	<div class="clear">&nbsp;</div>
                </div>
		    <% } %>
		    <div class="clear">&nbsp;</div>
		</div>
		<div class="clear">&nbsp;</div>
	</div>
</asp:Content>