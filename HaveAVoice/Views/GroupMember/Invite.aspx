<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<GroupInviteModel>" %>
<%@ Import Namespace="HaveAVoice.Models.View" %>
<%@ Import Namespace="HaveAVoice.Models" %>
<%@ Import Namespace="HaveAVoice.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.Constants" %>
<%@ Import Namespace="HaveAVoice.Services.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.UserInformation" %>
<%@ Import Namespace="Social.Generic.Models" %>
<%@ Import Namespace="Social.Generic.Helpers" %>
<%@ Import Namespace="HaveAVoice.Helpers.Groups" %>
<%@ Import Namespace="Social.ViewHelpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Invite Members
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript" language="javascript" src="/Content/jquery.autocomplete.js"></script>

<script type="text/javascript">
    $(function () {
        $("a.people").click(function () {
            $(this).addClass("selected");
            $("a.issue").removeClass("selected");
            $("#SearchType").val("User");
            $("#SearchQuery").unautocomplete();
            $("#SearchQuery").focus().autocomplete("/Search/getUserAjaxResult");

            return false;
        });

        if ($("a.people").hasClass("selected")) {
            $("#SearchQuery").unautocomplete();
            $("#SearchQuery").focus().autocomplete("/Search/getUserAjaxResult");
        } else {
            $("#SearchQuery").unautocomplete();
            $("#SearchQuery").focus().autocomplete("/Search/getIssueAjaxResult");
        }
    });
</script>

<div class="col-24 m-btm30">
	<% UserInformationModel<User> myUserInfo = HAVUserInformationFactory.GetUserInformation(); %>

    <div class="spacer-30">&nbsp;</div>

	<% Html.RenderPartial("Message"); %>

	<div class="clear">&nbsp;</div>

    <div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
        <a class="issue-create" href="/Group/List">GROUPS</a>
    	<div class="clear">&nbsp;</div>
    </div>

    <div class="push-1 col-4 center p-t5 p-b5 t-tab btint-6">
    	<%= Html.ActionLink("CREATE GROUP", "Create", "Group", null, new { @class = "issue-create" }) %>
    	<div class="clear">&nbsp;</div>
    </div>
    <div class="push-1 col-14 center p-t5 p-b5 t-tab b-wht">
    	<span class="fnt-16 tint-6 bold"><%= Html.Encode("Invite Friends to " + Model.Group.Name) %></span>
    	<div class="clear">&nbsp;</div>
    </div>
    <div class="clear">&nbsp;</div>
    	
    <div class="b-wht m-btm10">
    	<div class="spacer-10">&nbsp;</div>
		<div class="clear">&nbsp;</div>
	</div>

    <div class="clear">&nbsp;</div>
	<div class="col-21 m-btm10">
        <div class="action-bar bold p-a10 m-btm20 color-4">
            Invite some of your friends to the group <%= Model.Group.Name %>
        </div>

        <% using (Html.BeginForm("SendInvite", "GroupMember", FormMethod.Post)) { %>
            <% if(Model.Users.Count<User>() != 0) { %>
                <div class="col-2 m-btm25 m-lft20">
			        <input type="submit" class="button" value="Send To Selected Friends" />
	            </div>
            <% } %>
            <div class="clear">&nbsp;</div>
            <%= Html.Hidden("GroupId", Model.Group.Id) %>
            <% int cnt = 1; %>
            <% string klass = "friend"; %>
	        <% foreach (var item in Model.Users) { %>
	            <% if (cnt % 2 == 0) {
                        klass = "friend";
                    } else {
                        klass = "friend alt";
                    }%>
	    	
                    <% string myLeftMargin = cnt % 5 == 1 ? "m-lft20" : string.Empty;  %>
			        <div class="col-4 p-t10 center <%= myLeftMargin %> <%= klass %>">
                        <% bool myIsSelected = Model.SelectedUsers.Contains(item.Id); %>
                        <%=CheckBoxHelper.StandardCheckbox("SelectedUsers", item.Id.ToString(), myIsSelected) %> Invite
				        <div class="p-a5">
					        <div class="profile">
						        <a href="<%= LinkHelper.Profile(item) %>">
                                    <img src ="<%= PhotoHelper.ProfilePicture(item) %>" alt="<%= NameHelper.FullName(item) %>" class="profile" />
                                </a>
					        </div>
					        <div class="p-a5">
						        <a href="<%= LinkHelper.Profile(item) %>" class="name"><%= NameHelper.FullName(item)%></a>
					        </div>
				        </div>
				        <div class="clear">&nbsp;</div>
			        </div>
			
			        <% if (cnt % 5 == 0) { %>
				        <div class="clear">&nbsp;</div>
			        <% } %>
			
	                <% cnt++; %>
	        <% } %>
        <% } %>
    </div>
</div>
</asp:Content>