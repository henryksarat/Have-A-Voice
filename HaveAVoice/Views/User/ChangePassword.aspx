<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.StringWrapper>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ChangePassword
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Change Password</h2>
    <p>
        <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>
    </p>

    <% using (Html.BeginForm("ChangePassword", "User", new { forgetPasswordHash = Model.Value })) {%>

    <p>
        <%= Html.Encode(ViewData["Message"])%>
    </p>
        <fieldset>
            <legend>Fields</legend>
                <p>
                    Please enter your email address.
                </p>

                <p>
                    <label for="Email">New Password:</label>
                    <%= Html.TextBox("Email")%>
                    <%= Html.ValidationMessage("Email", "*")%>
                </p>
                <p>
                    <label for="Password">New Password:</label>
                    <%= Html.TextBox("Password")%>
                    <%= Html.ValidationMessage("Password", "*")%>
                </p>
                <p>
                    <label for="RetypedPassword">Retype Password:</label>
                    <%= Html.TextBox("RetypedPassword")%>
                    <%= Html.ValidationMessage("RetypedPassword", "*")%>
                </p>
            
            <p>
                <button name="button" value="submit">Submit</button>
            </p>
        </fieldset>
    <% } %>

</asp:Content>
