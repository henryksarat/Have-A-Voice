<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dictionary<string, bool>>" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	EditPrivacy
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit Privacy</h2>
    
    <% Html.RenderPartial("Message"); %>

    <% using (Html.BeginForm()) { %>
        <fieldset>
            <legend>Fields</legend>
            <p>
                <table>
                    <tr>
                        <td><label for="">Display Profile To Politicians:</label></td>
                        <td><label for="Newsletter">Yes</label> <%= Html.RadioButton(HAVPrivacySetting.Display_Profile_Politician.ToString(), true, Model[HAVPrivacySetting.Display_Profile_Politician.ToString()])%></td>
                        <td><label for="Newsletter">No</label> <%= Html.RadioButton(HAVPrivacySetting.Display_Profile_Politician.ToString(), false, !Model[HAVPrivacySetting.Display_Profile_Politician.ToString()])%> </td>
                    </tr>     
                </table>
            </p>
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>
    <%} %>

</asp:Content>
