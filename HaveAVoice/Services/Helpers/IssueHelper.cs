using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Helpers.Enums;

namespace HaveAVoice.Services.Helpers {
    public class IssueDispositionHelper {
        public static int NumberOfDisposition(User aUser, Disposition aDisposition) {
            int myTotalFromIssues = (from i in aUser.IssueDispositions
                                     where i.Disposition == (int)aDisposition
                                     select i).Count<IssueDisposition>();

            int myTotalFromIssueReplys = (from ir in aUser.IssueReplyDispositions
                                          where ir.Disposition == (int)aDisposition
                                          select ir).Count<IssueReplyDisposition>();

            return myTotalFromIssues + myTotalFromIssueReplys;
        }
    }
}