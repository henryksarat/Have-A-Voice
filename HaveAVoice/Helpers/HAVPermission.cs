using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Helpers {
    public enum HAVPermission {
        Test,
        //Board
        View_Board,
        Post_To_Board,
        Edit_Any_Board_Message,
        Edit_Board_Message,
        Delete_Any_Board_Message,
        Delete_Board_Message,
        Post_Reply_To_Board,
        Edit_Any_Board_Reply,
        Edit_Board_Reply,
        Delete_Any_Board_Reply,
        Delete_Board_Reply,
        //Events
        Delete_Any_Event,
        Delete_Any_Issue,
        Delete_Any_Issue_Reply,
        Delete_Any_Issue_Reply_Comment,
        Official_Account,
        View_Issue,
        View_Issue_Reply,
        Post_Issue, 
        Post_Issue_Reply,
        Edit_Issue,
        Edit_Any_Issue,
        Edit_Issue_Reply,
        Edit_Any_Issue_Reply,
        Edit_Issue_Reply_Comment,
        Edit_Any_Issue_Reply_Comment,
        Post_Issue_Reply_Comment,
        Delete_Issue,
        Delete_Issue_Reply,
        Delete_Issue_Reply_Comment,
        Send_Private_Message,
        Admin_Login,
        View_Roles,
        Create_Role,
        Edit_Role,
        Delete_Role,
        Switch_Users_Role,
        View_Permissions,
        Create_Permission,
        Edit_Permission,
        Delete_Permission,
        View_Restrictions,
        Create_Restriction,
        Edit_Restriction,
        Delete_Restriction,
        View_Feedback,
        View_Admin
    }
}