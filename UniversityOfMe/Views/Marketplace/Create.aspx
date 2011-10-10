<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<ItemViewModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create a Listing in the <%= Model.Get().UniversityId %> Marketplace
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
        <% Html.RenderPartial("Message"); %>
        <div class="create-feature-form">
		    <div class="create"> 
			    <div class="banner black full small"> 
				<span class="cart">Create a Listing in the <%= Model.Get().UniversityId %> Marketplace</span> 
			</div> 

            <% Html.RenderPartial("Validation"); %>

                <div class="padding-col">
                    <% using (Html.BeginForm("Create", "Marketplace", FormMethod.Post, FormMethod.Post, new { enctype = "multipart/form-data", @class = "create btint-6" })) {%>
                        <div class="field-holder">
			                <label for="BookTitle">Title of Listing</label> 
			                <%= Html.TextBox("Title", string.Empty, new { @class = "quarter" })%>
                            <%= Html.ValidationMessage("Title", "*", new { @class = "req" })%>
                        </div>
                        <div class="field-holder">
			                <label for="Item Type">Item Type Condition</label> 
                            <%= Html.DropDownListFor(model => model.Get().ItemType, Model.Get().ItemTypes)%>
                            <%= Html.ValidationMessageFor(model => model.Get().ItemType, "*", new { @class = "req" })%>
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
			                <%= Html.TextBox("Price", string.Empty, new { @class = "quarter" })%>
                            <%= Html.ValidationMessage("Price", "*", new { @class = "req" })%>
                        </div>

                        <div class="field-holder">
			                <label for="Details">Description</label> 
                            <%= Html.TextArea("Description", string.Empty, 6, 0, new { @class = "textarea" })%>
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
	</div> 
</asp:Content>











