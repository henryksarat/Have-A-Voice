<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	FileUpload
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<form method="post" enctype="multipart/form-data" action="File/Upload">
  <div>
    <span>
     Name:
   </span>
   <span>
     <input type="text" id="Name" name="Name" />
   </span>
  </div>
  <div>
    <span>
      Alternate Text:
    </span>
    <span>
     <input type="text" id="AlternateText" name="AlternateText" />
    </span>
  </div>
  <div>
    <span>
      Image
    </span>
    <span>
      <input type="file" id="OriginalLocation" name="OriginalLocation" />
    </span>
  </div>
  <div>
    <input type="submit" value="Upload" />
  </div>
</form>

</asp:Content>
