<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div class="normal">
    <% if (ViewData.ModelState.Count > 0) { %>
        <span>Create was unsuccessful</span>
    <% } %>
    <ul>
            <%
                foreach (KeyValuePair<string, ModelState> keyValuePair in ViewData.ModelState) {
                    foreach (ModelError modelError in keyValuePair.Value.Errors) {
                        %>
                        <li><%= Html.Encode(modelError.ErrorMessage) %></li>
                        <%
                    }
                }
            %>
    </ul>   
</div>