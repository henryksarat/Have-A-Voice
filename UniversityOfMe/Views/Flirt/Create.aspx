<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<FlirtModel>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Send Anonymous Flirt
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("LeftNavigation", Model.LeftNavigation); %>

	<div class="eight last"> 
		<div class="create-feature-form create"> 
			<div class="banner black full small"> 
                <% string mySendTo = Model.Get().TaggedUser != null ? "TO " + NameHelper.FullName(Model.Get().TaggedUser) : string.Empty; %>
                
				<span class="professor">SEND ANONYMOUS FLIRT <%= mySendTo %></span> 
			</div> 
            <% Html.RenderPartial("Message"); %>
            <% Html.RenderPartial("Validation"); %>
            <div class="padding-col">
                <% using (Html.BeginForm("Create", "Flirt", FormMethod.Post)) {%>
                    <%= Html.Hidden("TaggedUserId", Model.Get().TaggedUserId) %>
                    <div class="mb25">
                        <% if (Model.Get().TaggedUser == null) { %>
                            Post an anonymous flirt for a person within your school.
                        <% } else { %>
                            Send an anonymous flirt to <%= NameHelper.FullName(Model.Get().TaggedUser) %>. 
                            This will be posted with the specified pet name but the user you are flirting 
                            with will also receive an email. This is ANONYMOUS and they will not know who you are
                            and the school won't know who they are.
                        <% } %>
                    </div>
                    <div class="field-holder">
			            <label for="Petname">Petname</label> 
                        <%= Html.DropDownList("Adjective", Model.Get().Adjectives)%>
                        <%= Html.DropDownList("DeliciousTreat", Model.Get().DeliciousTreats)%>
                        <%= Html.DropDownList("Animal", Model.Get().Animals)%>
                    </div>

                    <div class="field-holder">
			            <label for="Gender">Gender</label> 
                        Boy <%= Html.RadioButton("Gender", "Boy", Model.Get().Gender == "Boy")%>
                        Girl <%= Html.RadioButton("Gender", "Girl", Model.Get().Gender == "Girl")%>
                        <%= Html.ValidationMessage("Gender", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder" style="width:600px">
			            <label for="HairColor">Hair Color</label> 

                        Blonde <%= Html.RadioButton("HairColor", "Blonde", Model.Get().HairColor == "Blonde")%>
                        Brown <%= Html.RadioButton("HairColor", "Brown", Model.Get().HairColor == "Brown")%>
                        Black <%= Html.RadioButton("HairColor", "Black", Model.Get().HairColor == "Black")%>
                        Red <%= Html.RadioButton("HairColor", "Red", Model.Get().HairColor == "Red")%>
                        Bald <%= Html.RadioButton("HairColor", "Bald", Model.Get().HairColor == "Bald")%>
                        No idea <%= Html.RadioButton("HairColor", "Dunno", Model.Get().HairColor == "Dunno")%>
                        <%= Html.ValidationMessage("HairColor", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
			            <label for="Where">Where you saw them</label> 
                        <%= Html.TextBox("Where") %>
                        <%= Html.ValidationMessage("Gender", "*", new { @class = "req" })%>
                    </div>

                    <div class="field-holder">
                       <label for="Review">Flirt Message</label>							
				        <%= Html.TextArea("Message", new { @class = "textarea" })%>
                        <%= Html.ValidationMessage("Message", "*", new { @class = "req" })%>
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
