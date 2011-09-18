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
            return aUser != null && myClubService.IsAdmin(aUser, aClubId);
        }

        public static bool IsPending(User aUser, int aClubId) {
            IClubService myClubService = new ClubService(new ModelStateWrapper(new ModelStateDictionary()));
            return aUser != null && myClubService.IsPendingApproval(aUser.Id, aClubId);
        }

        public static bool IsMember(User aUser, int aClubId) {
            IClubService myClubService = new ClubService(new ModelStateWrapper(new ModelStateDictionary()));
            return aUser != null && myClubService.IsApartOfClub(aUser.Id, aClubId);
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
                                Status = Status.Pending,
                                FeedType = FeedType.Member
                            });
            myFeed.AddRange(from cm in myClubMembers.Where(cm => cm.Approved == UOMConstants.DENIED)
                            select new ClubAdminFeed() {
                                ClubMemberId = cm.Id,
                                ClubId = cm.ClubId,
                                MemberUser = cm.ClubMemberUser,
                                AdminUser = cm.DeniedByUser,
                                DateTimeStamp = cm.DeniedByDateTimeStamp,
                                Status = Status.Denied,
                                FeedType = FeedType.Member
                            });
            myFeed.AddRange(from cm in myClubMembers.Where(cm => cm.Approved == UOMConstants.APPROVED && cm.ApprovedByUserId != cm.ClubMemberUserId)
                            select new ClubAdminFeed() {
                                ClubMemberId = cm.Id,
                                ClubId = cm.ClubId,
                                MemberUser = cm.ClubMemberUser,
                                AdminUser = cm.ApprovedByUser,
                                DateTimeStamp = cm.ApprovedDateTimeStamp,
                                Status = Status.Approved,
                                FeedType = FeedType.Member
                            });
            if (aClub.LastEditedByUser != null) {
                myFeed.Add(new ClubAdminFeed() {
                    AdminUser = aClub.LastEditedByUser,
                    DateTimeStamp = aClub.LastEditedByDateTimeStamp,
                    FeedType = FeedType.Edited
                });
            }

            if (aClub.DeactivatedByUser != null) {
                myFeed.Add(new ClubAdminFeed() {
                    AdminUser = aClub.DeactivatedByUser,
                    DateTimeStamp = aClub.DeativatedDateTimeStamp,
                    FeedType = FeedType.Deactivated
                });
            }

            return myFeed.OrderByDescending(f => f.DateTimeStamp).Take<ClubAdminFeed>(aLimit);
        }
    }
}