<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UniversityOfMe.Models.View.CreateUserModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript" language="javascript">
	    $(function () {
	        $('#DateOfBirth').datepicker({
	            yearRange: '1900:2011',
	            changeMonth: true,
	            changeYear: true,
	            dateFormat: "mm-dd-yy",
	            yearRange: '1900:2011'
	        });
	    });
	</script>
    <h2>Create</h2>

    <% using (Html.BeginForm()) {%>
        <% Html.RenderPartial("Validation"); %>

        <fieldset>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Email) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Email) %>
                <%: Html.ValidationMessageFor(model => model.Email, "*") %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Password) %>
            </div>
            <div class="editor-field">
                <%: Html.PasswordFor(model => model.Password) %>
                <%: Html.ValidationMessageFor(model => model.Password, "*")%>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.FirstName) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.FirstName) %>
                <%: Html.ValidationMessageFor(model => model.FirstName, "*")%>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.LastName) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.LastName) %>
                <%: Html.ValidationMessageFor(model => model.LastName, "*")%>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Gender) %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownList("Gender", Model.Genders)%>
                <%: Html.ValidationMessageFor(model => model.Gender, "*")%>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.DateOfBirth) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBox("DateOfBirth", Model.getDateOfBirthFormatted())%>
                <%: Html.ValidationMessage("DateOfBirth", "*")%>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.ShortUrl) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.ShortUrl) %>
                <%: Html.ValidationMessageFor(model => model.ShortUrl, "*")%>
            </div>

			<div class="editor-label">
				<%= Html.CheckBox("Agreement") %> I agree with the <a href="/Site/Terms" target="_blank">Terms of Use</a>.
			</div>
			<div class="editor-field">
				<%= Html.ValidationMessage("Agreement", "*") %>
			</div>
            
            <p>
                <input type="submit" value="Create Account" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%: Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

