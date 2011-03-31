<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% if (!ViewData.ModelState.IsValid) { %>
	<div class="error">
	    <ul class="left m-lft10">
	        <% foreach (KeyValuePair<string, ModelState> keyValuePair in ViewData.ModelState) {
                //Can do an if on "Error" to skip it.
				foreach (ModelError modelError in keyValuePair.Value.Errors) { %>
					<li><%= Html.Encode(modelError.ErrorMessage)%></li>
	            <% }
	        } %>
	    </ul>   
	</div>
<% } %>