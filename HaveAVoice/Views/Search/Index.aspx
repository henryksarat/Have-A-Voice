<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript" src="../../Content/lib/jquery.js"></script>
<script type='text/javascript' src='../../Content/jquery.autocomplete.js'></script>
<script type='text/javascript' src='../../Content/localdata.js'></script>
<link rel="stylesheet" type="text/css" href="../../Content/jquery.autocomplete.css" />
	
<script type="text/javascript">
    $().ready(function() {

        $("#suggest1").focus().autocomplete("http://localhost:50815/Search/getAjaxResult");

        $("#clear").click(function() {
            $(":input").unautocomplete();
        });
    });

</script>
    <h2>Look for an artist</h2>       
        
    <% using (Html.BeginForm()) {%>
		<p>
			<label>Single City (local):</label>
			<input type="text" id="suggest1" />
			<input type="button" value="Get Value" />
		</p>
		
		<input type="submit" value="Submit" />
    <% }%>
    
        <br />



</asp:Content>
