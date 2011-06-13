using System.Web.Mvc;
using Social.Validation;
using UniversityOfMe.Models;
using UniversityOfMe.Services.Professors;
using UniversityOfMe.Services.Clubs;
using System.Collections;
using UniversityOfMe.Models.View;
using System.Collections.Generic;
using System.Linq;

namespace UniversityOfMe.Helpers.Functionality {
    public class ClubHelper {
        public static bool IsAdmin(User aUser, int aClubId) {
            IClubService myClubService = new ClubService(new ModelStateWrapper(new ModelStateDictionary()));
            return myClubService.IsAdmin(aUser, aClubId);
        }

        public static bool IsPending(User aUser, int aClubId) {
            IClubService myClubService = new ClubService(new ModelStateWrapper(new ModelStateDictionary()));
            return myClubService.IsPendingApproval(aUser.Id, aClubId);
        }

        public static bool IsMember(User aUser, int aClubId) {
            IClubService myClubService = new ClubService(new ModelStateWrapper(new ModelStateDictionary()));
            return myClubService.IsApartOfClub(aUser.Id, aClubId);
        }

        public static IEnumerable<ClubAdminFeed> GetAdminFeed(Club aClub, int aLimit) {
            IEnumerable<ClubMember> myClubMembers = aClub.ClubMembers.OrderByDescending(cm => cm.DateTimeStamp);
            List<ClubAdminFeed> myFeed = new List<ClubAdminFeed>();

            myFeed.AddRange(from cm in myClubMembers.Where(cm => cm.Approved == UOMConstants.PENDING)
                            select new ClubAdminFeed() {
                                ClubMemberId = cm.Id,
                                ClubId = cm.ClubId,
                                MemberUser = cm.ClubMemberUser,
                                DateTimeStamp = cm.DateTimeStamp,
                                HasDetails = true,
                                Status = Status.Pending
                            });
            myFeed.AddRange(from cm in myClubMembers.Where(cm => cm.Approved == UOMConstants.DENIED)
                            select new ClubAdminFeed() {
                                ClubMemberId = cm.Id,
                                ClubId = cm.ClubId,
                                MemberUser = cm.ClubMemberUser,
                                AdminUser = cm.DeniedByUser,
                                DateTimeStamp = cm.DeniedByDateTimeStamp,
                                Status = Status.Denied
                            });
            myFeed.AddRange(from cm in myClubMembers.Where(cm => cm.Approved == UOMConstants.APPROVED && cm.ApprovedByUserId != cm.ClubMemberUserId)
                            select new ClubAdminFeed() {
                                ClubMemberId = cm.Id,
                                ClubId = cm.ClubId,
                                MemberUser = cm.ClubMemberUser,
                                AdminUser = cm.ApprovedByUser,
                                DateTimeStamp = cm.ApprovedDateTimeStamp,
                                Status = Status.Approved
                            });

            return myFeed.OrderByDescending(f => f.DateTimeStamp).Take<ClubAdminFeed>(aLimit);
        }
    }
}