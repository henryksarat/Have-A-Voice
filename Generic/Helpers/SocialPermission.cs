using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.Generic.Helpers {
    public enum SocialPermission {
        //Board
        Delete_Board_Message,
        Delete_Any_Board_Message,
        Edit_Any_Board_Message,
        Edit_Board_Message,
        Post_To_Board,
        View_Board,
        //Board Reply
        Delete_Any_Board_Reply,
        Delete_Board_Reply,
        Edit_Any_Board_Reply,
        Edit_Board_Reply,
        Post_Reply_To_Board,
        //Issue
        Delete_Any_Issue,
        Delete_Issue,
        Edit_Any_Issue,
        Edit_Issue,
        Post_Issue,
        View_Issue,
        //Issue Reply
        Delete_Any_Issue_Reply,
        Delete_Issue_Reply,
        Edit_Any_Issue_Reply,
        Edit_Issue_Reply,
        Post_Issue_Reply,
        View_Issue_Reply,
        //Issue Reply Comments
        Delete_Any_Issue_Reply_Comment,
        Delete_Issue_Reply_Comment,
        Edit_Any_Issue_Reply_Comment,
        Edit_Issue_Reply_Comment,
        Post_Issue_Reply_Comment,
        //Events
        Edit_Any_Event,
        Delete_Any_Event,
        //Messagining
        Send_Private_Message,
        //Admin
        Admin_Login,
        View_Admin,
        //Roles
        Create_Role,
        Delete_Role,
        Edit_Role,
        View_Roles,
        Switch_Users_Role,
        //Permissions
        Create_Permission,
        Delete_Permission,
        Edit_Permission,
        View_Permissions,
        //Authority Verification
        Create_Authority_Verification_Token,
        Authority_Feed,
        //IssueReply
        Post_Anonymous_Issue_Reply,
        //Usertypes
        Confirmed_User,
        Confirmed_Politician,
        Confirmed_Political_Candidate,
        //Site
        View_Feedback,
        View_ErrorLog,

        //UOfMeOnly
        //Club
        Edit_Any_Club,
        //textbook
        Edit_Any_Textbook,
        Delete_Any_Textbook,
        //Classes
        Delete_Any_Class_Board,
        Delete_Any_Class_Board_Reply
    }
}
