<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<ItemViewModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Model.Get().ItemId %> Listing | Edit | <%= Model.Get().Title %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript" language="javascript">
	    $(function () {
	        $('#ExpireListing').datepicker({
	            changeMonth: true,
	            changeYear: true,
	            dateFormat: "mm-dd-yy",
	            yearRange: '2011:2012'
	        });
	    });
	</script>

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
		<div class="create-feature-form">
			<div class="banner black full small"> 
				<span class="book">EDIT <%= Model.Get().ItemType %> Listing - <%= Model.Get().Title %></span> 
			</div> 

            <% Html.RenderPartial("Message"); %>
            <% Html.RenderPartial("Validation"); %>
            <div class="padding-col">
                <% using (Html.BeginForm("Edit", "Marketplace", FormMethod.Post, FormMethod.Post, new { enctype = "multipart/form-data", @class = "create btint-6" })) {%>
                    <%= Html.Hidden("ItemId", Model.Get().ItemId) %>
                    
                    <div class="field-holder">
			            <label for="BookTitle">Title of Listing</label> 
			            <%= Html.TextBox("Title", Model.Get().Title, new { @class = "quarter" })%>
                        <%= Html.ValidationMessage("Title", "*", new { @class = "req" })%>
                    </div>
                    <div class="field-holder">
			            <label for="Item Type">Item Type Condition</label> 
                        <%= Html.DropDownListFor(model => model.Get().ItemType, Model.Get().ItemTypes)%>
                        <%= Html.ValidationMessageFor(model => model.Get().ItemType, "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
                        <label for="Image">Current Image</label> 
                        <img src="<%= Model.Get().ImageUrl %>" />
                    </div>

                    <div class="field-holder">
			            <label for="BookImage">Image</label> 
			            <input type="file" id="Image" name="Image" size="23" />
                        <%= Html.ValidationMessage("Image", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder-extra">
			            <label for="ExpireListing">Expire Listing</label> 
                        <%= Html.TextBox("ExpireListing", Model.Get().ExpireListing)%>
                        <%= Html.ValidationMessage("ExpireListing", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
			            <label for="Price">Price</label> 
			            <%= Html.TextBox("Price", Model.Get().Price, new { @class = "quarter" })%>
                        <%= Html.ValidationMessage("Price", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
			            <label for="Details">Description</label> 
                        <%= Html.TextArea("Description", Model.Get().Description, 6, 0, new { @class = "textarea" })%>
                        <%= Html.ValidationMessage("Description", "*", new { @class = "req" })%>
                    </div>

			        <div class="field-holder">
                        <div class="right">
				            <input type="submit" name="submit" class="btn site button-padding" value="Submit" /> 
                        </div>
			        </div> 
                <% } %>
            </div>
		</div> 
	</div> 
</asp:Content>
