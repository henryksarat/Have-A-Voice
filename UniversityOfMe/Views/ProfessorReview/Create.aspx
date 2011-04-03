<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UniversityOfMe.Models.View.CreateProfessorReviewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create</h2>

    <% using (Html.BeginForm()) {%>
        <% Html.RenderPartial("Message"); %>
        <% Html.RenderPartial("Validation"); %>
        <%= Html.Hidden("ProfessorId", Model.ProfessorId) %>

        <fieldset>
            <div class="editor-label">
                <%: Html.Label("Please provide when you had this professor:") %>
            </div>
            <div class="editor-field">
                <%: Html.DropDownListFor(model => model.AcademicTermId, Model.AcademicTerms)%>
                <%: Html.ValidationMessageFor(model => model.AcademicTermId, "*")%>

                <%: Html.DropDownListFor(model => model.Year, Model.Years)%>
                <%: Html.ValidationMessageFor(model => model.Year, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Class you had this professor with:") %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Class)%>
                <%: Html.ValidationMessageFor(model => model.Class, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.LabelFor(model => model.Rating) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Rating)%>
                <%: Html.ValidationMessageFor(model => model.Rating, "*")%>
            </div>

            <div class="editor-label">
                <%: Html.LabelFor(model => model.Review) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Review)%>
                <%: Html.ValidationMessageFor(model => model.Review, "*")%>
            </div>
                        
			<div class="editor-label">
				<%= Html.CheckBoxFor(model => model.Anonymous) %> Post rating and review as anonymous
			</div>
            
            <p>
                <input type="submit" value="Review" />
            </p>
        </fieldset>

    <% } %>

</asp:Content>
