<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    $(document).ready(function () {
        $(".close").click(function () {
            $("#message").hide();
        });
    });
</script> 


<%= ViewData["Message"]%>
<%= TempData["Message"]%>