<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<% if (ViewData.ModelState.Count > 0) { %>
	<div class="error">
	    <ul class="left m-lft10">
	        <% foreach (KeyValuePair<string, ModelState> keyValuePair in ViewData.ModelState) {
				foreach (ModelError modelError in keyValuePair.Value.Errors) { %>
					<li><%= Html.Encode(modelError.ErrorMessage) %></li>
	            <% }
	        } %>
	    </ul>   
	</div>
<% } %>