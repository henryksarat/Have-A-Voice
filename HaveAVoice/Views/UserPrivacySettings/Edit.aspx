<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.UserPrivacySetting>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	EditPrivacy
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit Privacy</h2>

    <% using (Html.BeginForm(new { id = Model.Id })) { %>
        <fieldset>
            <legend>Fields</legend>
            <p style="color: Red">
                <%= Html.Encode(ViewData["Message"]) %>
            </p>
            <p>
                <table>
                    <tr>
                        <td><label for="">Display Profile To Non Logged In:</label></td>
                        <td><label for="Newsletter">Yes</label> <%= Html.RadioButton("DisplayProfileToNonLoggedIn", true, Model.DisplayProfileToNonLoggedIn)%></td>
                        <td><label for="Newsletter">No</label> <%= Html.RadioButton("DisplayProfileToNonLoggedIn", false, !Model.DisplayProfileToNonLoggedIn)%> </td>
                    </tr>   
                    <tr>
                        <td><label for="">Display Profile To Friends:</label></td>
                        <td><label for="Newsletter">Yes</label> <%= Html.RadioButton("DisplayProfileToFriends", true, Model.DisplayProfileToFriends)%></td>
                        <td><label for="Newsletter">No</label> <%= Html.RadioButton("DisplayProfileToFriends", false, !Model.DisplayProfileToFriends)%> </td>
                    </tr>   
                    <tr>
                        <td><label for="">Display Profile To Logged In Users:</label></td>
                        <td><label for="Newsletter">Yes</label> <%= Html.RadioButton("DisplayProfileToLoggedInUsers", true, Model.DisplayProfileToLoggedInUsers)%></td>
                        <td><label for="Newsletter">No</label> <%= Html.RadioButton("DisplayProfileToLoggedInUsers", false, !Model.DisplayProfileToLoggedInUsers)%> </td>
                    </tr>   
                </table>
            </p>
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>
    <%} %>

</asp:Content>
