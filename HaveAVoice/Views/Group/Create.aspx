<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EditGroupModel>" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Helpers.Constants" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Groups
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% Html.RenderPartial("Message"); %>
<% Html.RenderPartial("Validation"); %>

<% using (Html.BeginForm("Create", "Group", FormMethod.Post)) {%>
    <div>
        <label for="Name">Name:</label> 
        <%= Html.TextBox("Name")%>
        <%= Html.ValidationMessage("Name", "*")%>
    </div>
    <div>
	    <label for="Name">Your title:</label> 
	    <%= Html.TextBox("CreatorTitle", GroupConstants.DEFAULT_GROUP_LEADER_TITLE)%>
        <%= Html.ValidationMessage("CreatorTitle", "*")%>
    </div> 
    <div>
	    <label for="Description">Description:</label> 
        <%= Html.TextArea("Description", string.Empty, 6, 0 , null) %>
        <%= Html.ValidationMessage("Description", "*")%>
    </div> 
    <div>
        <label for="AutoAccept">Auto Accept:</label> 
        <%= Html.RadioButton("AutoAccept", true)%> Yes
        <%= Html.RadioButton("AutoAccept", false, true)%> No
    </div> 
    <div>
	    <label for="ZipCodeTags">ZipCodeTags:</label> 
        <%= Html.TextArea("ZipCodeTags", string.Empty, 6, 0, null)%>
        <%= Html.ValidationMessage("ZipCodeTags", "*")%>
    </div> 
    <div>
	    <label for="KeywordTags">KeywordTags:</label> 
        <%= Html.TextArea("KeywordTags", string.Empty, 6, 0, null)%>
        <%= Html.ValidationMessage("KeywordTags", "*")%>
    </div> 
    <div>
	    <label for="CityTag">CityTag:</label> 
	    <%= Html.TextBox("CityTag")%>
        <%= Html.ValidationMessage("CityTag", "*")%>
    </div> 
    <div>
        <label for="StateTag">StateTag:</label> 
        <%= Html.DropDownList("StateTag", Model.States)%>
    </div>
	<div>
		<input type="submit" name="submit" value="Submit" /> 
		<input type="button" name="cancel" value="Cancel" /> 
	</div> 
<% } %>
</asp:Content>