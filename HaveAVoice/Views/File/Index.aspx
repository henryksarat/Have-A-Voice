<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<HaveAVoice.Controllers.FileDescription>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%">
        <tr>
            <th>
                Name
            </th>
            <th>
                File Url
            </th>
            <th>
                Uploaded Date
            </th>
            <th>
                Size
            </th>
        </tr>

    <% foreach (var item in Model) {
           string fileType = System.IO.Path.GetExtension(item.Name);
           string completeURL = "http://localhost:50815/Uploads/" + Html.Encode(item.WebPath);
           %>
    
        <tr>
            <td>
                <%if (fileType.ToUpper().Equals(".JPG"))
                  {%>
                    <img src=<%=completeURL%> width="100" height="100"/>
                  </a>
                  <%}
                 %>
            </td>
            <td style="vertical-align:middle">
               <a href=<%=completeURL%>>
               <%= Html.Encode(item.WebPath) %>
               </a>
            </td>
           <td style="vertical-align:middle">
               <%= Html.Encode(item.DateCreated.ToString()) %>
            </td>
            <td style="vertical-align:middle">
                <%= Html.Encode(item.Size) + " KB" %>
            </td>
        </tr>
    
    <% } %>
    
    <% using (Html.BeginForm("Upload", "File", FormMethod.Post, new { enctype = "multipart/form-data" }))
       {%>
        <p><input type="file" id="file1" name="fileUpload" size="23"/> </p>
        <p><input type="submit" value="Upload file" /></p>
    <% } %>

    </table>

</asp:Content>

