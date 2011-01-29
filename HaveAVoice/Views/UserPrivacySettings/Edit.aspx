<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EditPrivacySettingsModel>" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>

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
                    <% foreach (HAVPrivacySetting mySetting in Enum.GetValues(typeof(HAVPrivacySetting))) { %>
                        <tr>
                            <td><label for=""><%= mySetting.ToString()%></label></td>
                            <td><label for="Newsletter">Yes</label> <%= Html.RadioButton(mySetting.ToString(), true, Model.PrivacySettings[mySetting.ToString()].Second)%></td>
                            <td><label for="Newsletter">No</label> <%= Html.RadioButton(mySetting.ToString(), false, !Model.PrivacySettings[mySetting.ToString()].Second)%> </td>
                        </tr>
                    <% } %>     
                </table>
            </p>
            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>
    <%} %>

</asp:Content>
