<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript" src="../../Content/lib/jquery.js"></script>
<script type='text/javascript' src='../../Content/jquery.autocomplete.js'></script>
<script type='text/javascript' src='../../Content/localdata.js'></script>
<link rel="stylesheet" type="text/css" href="../../Content/jquery.autocomplete.css" />
	
<script type="text/javascript">
    $().ready(function () {

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

    <h2>Look for an artist</h2>       
        
    <% using (Html.BeginForm()) {%>
		<p>
            <%= Html.DropDownList("SearchType", new SelectList(new List<string>() { "User", "Issue"}, "User"))  %>
			<label>Single City (local):</label>
            <%= Html.TextBox("SearchQuery")%>
		</p>
		
		<input type="submit" value="Search" />
    <% }%>
    
        <br />



</asp:Content>
