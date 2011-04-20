<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LoggedInWrapperModel<UniversityView>>" %>
<%@ Import Namespace="UniversityOfMe.Models" %>
<%@ Import Namespace="UniversityOfMe.Helpers" %>
<%@ Import Namespace="UniversityOfMe.Models.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Details</h2>

    <% Html.RenderPartial("Message"); %>
    <% Html.RenderPartial("Validation"); %>

    <table>
        <tr>
            <td>
                <img src="<%= PhotoHelper.ProfilePicture(Model.User) %>" />
            </td>
            <td>
                <a href="/Message/Inbox"><%= Model.GetUnreadMessageCount() %> new messages</a>
            </td>
            <td>
                <a href="/PhotoAlbum/List">My Photos</a>
            </td>
            <td>
                <a href="/User/Edit">Account Settings</a>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                Newest Members in the university<br />
                <% foreach (User myUser in Model.Get().NewestUsers) { %>
                    <%= NameHelper.FullName(myUser) %><br />
                <% } %>
            </td>
            <td>
               <table>
        <tr>
            <td>
                Latest Professor Ratings<br />
                <% foreach (Professor myProfessor in Model.Get().Professors.Take<Professor>(5)) { %>
                    * <a href="/<%= myProfessor.UniversityId %>/Professor/Details/<%= myProfessor.FirstName + "_" +  myProfessor.LastName %>"><%= myProfessor.FirstName + " " +  myProfessor.LastName %></a>
                    <% int myTotalReviews = myProfessor.ProfessorReviews.Count; %>
                    <%= myProfessor.ProfessorReviews.Sum<ProfessorReview>(pr => pr.Rating) / myTotalReviews%> stars (<%= myTotalReviews%> ratings)
                    <br />
                <% } %>

                <a href="/<%= Model.Get().University.Id %>/Professor/List">View all</a>
            </td>
            <td>
                Latest Class Ratings<br />
                <table>
                    <% foreach (Class myClass in Model.Get().Classes.Take<Class>(5)) { %>
                        <tr>
                            <td>
                                * <a href="<%= URLHelper.BuildClassUrl(myClass) %>"><%= myClass.ClassCode %></a>
                            </td>
                            <td>
                                <% int myTotalReviews = 100; %>
                                <%= 1000 / myTotalReviews%> stars (<%= myTotalReviews%> ratings)
                            </td>
                        </tr>
                        <tr>
                            <td><%= TextShortener.Shorten(myClass.ClassTitle, 20) %></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td><%= myClass.AcademicTerm.DisplayName %> <%= myClass.Year %></td>
                            <td></td>
                        </tr>
                    
                        <br />
                    <% } %>
                </table>
                <a href="/<%= Model.Get().University.Id %>/Class/List">View all</a>
            </td>

        </tr>
        <tr>
            <td>
                Latest Events on Campus<br />
                <% foreach (Event myEvent in Model.Get().Events.Take<Event>(5)) { %>
                    * <a href="/<%= myEvent.UniversityId %>/Event/Details/<%= myEvent.Id %>"><%= myEvent.Title %></a>
                    <%= myEvent.StartDate%>
                    <br />
                <% } %>

                <a href="/<%= Model.Get().University.Id %>/Event/List">View all</a>
            </td>
            <td>
                Buy/Sell Textbooks<br />
                <% foreach (TextBook myTextBook in Model.Get().TextBooks.Take<TextBook>(5)) { %>
                    * <a href="/<%= myTextBook.UniversityId %>/TextBook/Details/<%= myTextBook.Id %>"><%= myTextBook.BookTitle %> (<%= myTextBook.ClassCode %>)</a>
                    <%= myTextBook.BuySell.Equals("Buy") ? "Buy" : "Sell" %>
                    <br />
                <% } %>

                <a href="/<%= Model.Get().University.Id %>/TextBook/List">View all</a>
            </td>
        </tr>

        <tr>
            <td>
                Most Active Organizations<br />
                <% foreach (Club myClub in Model.Get().Organizations.Take<Club>(5)) { %>
                    * <a href="/<%= myClub.UniversityId %>/Club/Details/<%= myClub.Id %>"><%= myClub.Name %></a>
                    <%= myClub.ClubTypeDetails.DisplayName %>
                    ( <%= myClub.ClubMembers.Count %> members)
                    <br />
                <% } %>

                <a href="/<%= Model.Get().University.Id %>/Club/List">View all</a>
            </td>
            <td>
                General Postings<br />
                <% foreach (GeneralPosting myGeneralPosting in Model.Get().GeneralPostings.Take<GeneralPosting>(5)) { %>
                    * <a href="/<%= myGeneralPosting.UniversityId %>/GeneralPosting/Details/<%= myGeneralPosting.Id %>"><%= TextShortener.Shorten(myGeneralPosting.Title, 20) %></a><br />
                    Posts <%= myGeneralPosting.GeneralPostingReplies.Count %><br />
                    Last post <%= myGeneralPosting.GeneralPostingReplies.OrderByDescending(gp => gp.DateTimeStamp).First().DateTimeStamp %><br />
                    <br />
                <% } %>

                <a href="/<%= Model.Get().University.Id %>/GeneralPosting/List">View all</a>
            </td>
        </tr>
    </table>
            </td>
        </tr>
    </table>
</asp:Content>

