<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<HaveAVoice.Models.View.CreateUserModelBuilder>" %>
<%@ Import Namespace="HaveAVoice.Helpers.UI" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        span#meter
        {
            color: #000;
            padding: 4px;
            font-weight:bold;
        }
        
        span#meter span
        {
            width: 75px;
            padding: 4px;
            margin: 0px;
            text-align: center;
        }
        
        span#meter span.weak
        {
            border: 1px solid #C00;
            background: #F7CBCA;
        }
        
        span#meter span.okay
        {
            border: 1px solid #DEDEDE;
            background: #FFC;
        }
        
        span#meter span.good
        {
            border: 1px solid #349534;
            background: #C9FFCA;
        }
    </style>

    <script type="text/javascript">
        $(function() {
            $("span#meter span").fadeTo("fast", .2);
            $('#DateOfBirth').datepicker(
            {
                changeYear: true,
                changeMonth: true,
                dateFormat: "mm-dd-yy"
            });
            $("#Password").bind("keyup", function(e) {
                var str = 1;

                if ($(this).val().length >= 5) {
                    str++;
                }
                if ($(this).val().match(/[a-z]+/)) {
                    str++;
                }
                if ($(this).val().match(/[0-9]+/)) {
                    str++;
                }
                if ($(this).val().match(/[A-Z]+/)) {
                    str++;
                }

                switch (str) {
                    case 1: case 2:
                        $("span#meter span").fadeTo("fast", .2);
                        $("span.weak").stop().fadeTo("normal", 1);
                        break;
                    case 3: case 4:
                        $("span#meter span").fadeTo("fast", .2);
                        $("span.okay").stop().fadeTo("normal", 1);
                        break;
                    case 5:
                        $("span#meter span").fadeTo("fast", .2);
                        $("span.good").stop().fadeTo("normal", 1);
                        break;
                }
            });
        });    
    </script>
    
    <h2>Create</h2>

    <%= Html.ValidationSummary("Create was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) {%>

        <fieldset>
            <legend>Fields</legend>
            <p>
                 <%= Html.Encode(ViewData["Message"]) %>
            </p>
            <p>
                <label for="Email">Email:</label>
                <%= Html.TextBox("Email", Model.Email()) %>
                <%= Html.ValidationMessage("Email", "*") %>
            </p>
            <p>
                <label for="FirstName">Full Name:</label>
                <%= Html.TextBox("FullName", Model.FullName())%>
                <%= Html.ValidationMessage("FullName", "*") %>
            </p>
            <p>
                <label for="Username">Username:</label>
                <%= Html.TextBox("Username", Model.Username())%>
                <%= Html.ValidationMessage("Username", "*") %>
            </p>
            <p>
                <label for="Password">Password:</label>
                <%= Html.Password("Password") %>
                <span id="meter">
                    <span class="weak">WEAK</span>
                    <span class="okay">OKAY</span>
                    <span class="good">GOOD</span>
                </span>
                <%= Html.ValidationMessage("Password", "*") %>
            </p>
            <p>
                <label for="DateOfBirth">Date Of Birth:</label>
                <%= Html.TextBox("DateOfBirth", Model.getDateOfBirthFormatted())%>
                <%= Html.ValidationMessage("DateOfBirth", "*") %>
            </p>
            <p>
                <label for="State">City/State:</label>
                <%= Html.TextBox("City", Model.City())%>
                <%= Html.ValidationMessage("City", "*")%>
                <%= Html.DropDownList("State", Model.States())%>
                <%= Html.ValidationMessage("State", "*")%>
            </p>
            <p>
                <%= CaptchaHelper.GenerateCaptcha()  %>:  
            </p>
            <p>
                <%= Html.TextArea("AgreementText", UserHelper.UserAgreement()) %>
            </p>
            <p>
                <%= Html.CheckBox("Agreement") %> I Agree
                <%= Html.ValidationMessage("Agreement", "*")%>
            </p>            
            <%--
                <table>
                    <tr>
                        <td colspan="2"><label for="Newsletter">Newsletter:</label></td>
                    </tr>
                    <tr>
                        <td><label for="Newsletter">Yes</label> <%= Html.RadioButton("Newsletter", true, Model.UserInformation.Newsletter) %></td>
                        <td><label for="Newsletter">No</label> <%= Html.RadioButton("Newsletter", false, !Model.UserInformation.Newsletter)%> </td>
                    </tr>
                </table>
            --%>
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>