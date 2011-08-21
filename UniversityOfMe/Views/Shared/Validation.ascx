<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<% if (!ViewData.ModelState.IsValid) { %>
	<div class="error">
	    <ul class="left ml10">
	        <% foreach (KeyValuePair<string, ModelState> keyValuePair in ViewData.ModelState) {
				foreach (ModelError modelError in keyValuePair.Value.Errors) { %>
                    <% if(!string.IsNullOrEmpty(modelError.ErrorMessage)) {  %>
					    <li><%= Html.Encode(modelError.ErrorMessage) %></li>
                    <% } %>
	            <% }
	        } %>
	    </ul>   
	</div>
<% } %>