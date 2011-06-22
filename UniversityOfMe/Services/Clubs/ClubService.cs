﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.Admin.Exceptions;
using Social.Admin.Helpers;
using Social.Generic.Constants;
using Social.Generic.Exceptions;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Photo.Exceptions;
using Social.Photo.Helpers;
using Social.User.Repositories;
using Social.User.Services;
using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Helpers.Constants;
using UniversityOfMe.Models;
using UniversityOfMe.Models.View;
using UniversityOfMe.Repositories.Clubs;
using UniversityOfMe.Repositories.UserRepos;
using UniversityOfMe.Services.Professors;

namespace UniversityOfMe.Services.Clubs {
    public class ClubService : IClubService {
        private const string CLUB_DOESNT_EXIST = "That club doesn't exist.";
        private const string MEMBER_DOESNT_EXIST = "That user doesn't exist.";
        private const string NOT_ADMINISTRATOR = "You are not an administrator of this club.";

        private IValidationDictionary theValidationDictionary;
        private IClubRepository theClubRepository;
        private IUserRetrievalService<User> theUserRetrievalService;

        public ClubService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityClubRepository(), new EntityUserRetrievalRepository()) { }

        public ClubService(IValidationDictionary aValidationDictionary, IClubRepository aProfessorRepo, IUserRetrievalRepository<User> aUserRetrievalRepo) {
            theValidationDictionary = aValidationDictionary;
            theClubRepository = aProfessorRepo;
            theUserRetrievalService = new UserRetrievalService<User>(aUserRetrievalRepo);
        }

        public bool ActivateClub(UserInformationModel<User> aUser, int aClubId) {
            if (!ValidateAdmin(aUser.Details, aClubId)) {
                return false;
            }

            theClubRepository.ActivateClub(aUser.Details, aClubId);

            return true;
        }

        public bool ApproveClubMember(UserInformationModel<User> aUser, int aClubMemberId, string aTitle, bool anAdministrator) {
            if (!ValidTitle(aTitle)) {
                return false;
            }

            theClubRepository.ApproveClubMember(aUser.Details, aClubMemberId, aTitle, anAdministrator);

            return true;
        }

        public void CancelRequestToJoin(UserInformationModel<User> aUser, int aClubId) {
            theClubRepository.DeleteRequestToJoinClub(aUser.Details, aClubId);
        }

        public bool CreateClub(UserInformationModel<User> aUser, ClubViewModel aCreateClubModel) {
            if (!ValidClub(aCreateClubModel, true)) {
                return false;
            }

            Club myClub = theClubRepository.CreateClub(aUser.Details, aCreateClubModel.UniversityId, aCreateClubModel.ClubType, aCreateClubModel.Name, aCreateClubModel.Description);

            try {
                theClubRepository.AddMemberToClub(aUser.Details, aUser.Details.Id, myClub.Id, aCreateClubModel.Title, true);
            } catch (Exception myException) {
                theClubRepository.DeleteClub(myClub.Id);
                throw new Exception("Error adding the creating member of the club as a club member.", myException);
            }

            if (aCreateClubModel.ClubImage != null) {
                UpdateClubPhoto(aCreateClubModel.Name, aCreateClubModel.ClubImage, myClub);
            }

            return true;
        }

        public IDictionary<string, string> CreateAllClubTypesDictionaryEntry() {
            IEnumerable<ClubType> myClubTypes = theClubRepository.GetClubTypes();
            IDictionary<string, string> myDictionary = DictionaryHelper.DictionaryWithSelect();
            foreach (ClubType myUniversity in myClubTypes) {
                myDictionary.Add(myUniversity.DisplayName, myUniversity.Id);
            }
            return myDictionary;
        }

        public bool DeactivateClub(UserInformationModel<User> aUser, int aClubId) {
            if (!ValidateAdmin(aUser.Details, aClubId)) {
                return false;
            }

            theClubRepository.DeactivateClub(aUser.Details, aClubId);

            return true;
        }

        public void DenyClubMember(UserInformationModel<User> aUser, int aClubMemberId) {
            theClubRepository.DenyClubMember(aUser.Details, aClubMemberId);
        }

        public Club GetClub(UserInformationModel<User> aUser, int aClubId) {
            theClubRepository.MarkClubBoardAsViewed(aUser.Details, aClubId);
            return theClubRepository.GetClub(aUser.Details, aClubId);
        }

        public bool EditClub(UserInformationModel<User> aUserEditing, ClubViewModel aClubViewModel) {
            if (!ValidClub(aClubViewModel, false)) {
                return false;
            }

            Club myClub = theClubRepository.GetClub(aUserEditing.Details, aClubViewModel.ClubId);

            myClub.Name = aClubViewModel.Name;
            myClub.Description = aClubViewModel.Description;
            myClub.ClubType = aClubViewModel.ClubType;
            myClub.LastEditedByUserId = aUserEditing.UserId;
            myClub.LastEditedByDateTimeStamp = DateTime.UtcNow;

            theClubRepository.UpdateClub(myClub);

            if (aClubViewModel.ClubImage != null) {
                string myOldClubImage = myClub.Picture;

                UpdateClubPhoto(aClubViewModel.Name, aClubViewModel.ClubImage, myClub);
                if (!string.IsNullOrEmpty(myOldClubImage)) {
                    SocialPhotoHelper.PhysicallyDeletePhoto(HttpContext.Current.Server.MapPath(PhotoHelper.ClubPhoto(myOldClubImage)));
                }
            }

            return true;
        }

        public Club GetClubForEdit(UserInformationModel<User> aUser, int aClubId) {
            if (!IsAdmin(aUser.Details, aClubId)) {
                if (!PermissionHelper<User>.AllowedToPerformAction(theValidationDictionary, aUser, SocialPermission.Edit_Any_Club)) {
                    throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
                }
            }
            return theClubRepository.GetClub(aUser.Details, aClubId);
        }

        public IEnumerable<ClubBoard> GetClubBoardPostings(int aClubId) {
            return theClubRepository.GetClubBoardPostings(aClubId);
        }

        public ClubMember GetClubMember(UserInformationModel<User> aUser, int aClubMemberId) {
            ClubMember myClubMember = theClubRepository.GetClubMember(aClubMemberId);
            if (myClubMember != null) {
                bool myIsAdmin = IsAdmin(aUser.Details, myClubMember.ClubId);
                if (!myIsAdmin) {
                    myClubMember = null;
                }
            }
            return myClubMember;
        }

        public IEnumerable<ClubMember> GetActiveClubMembers(int aClubId) {
            return theClubRepository.GetClubMembers(aClubId).Where(cm => cm.Approved == UOMConstants.APPROVED);
        }

        public IEnumerable<Club> GetClubs(UserInformationModel<User> aUser, string aUniversityId) {
            return theClubRepository.GetClubs(aUser.Details, aUniversityId);
        }

        public bool IsAdmin(User aUser, int aClubId) {
            ClubMember myClubMember = theClubRepository.GetClubMember(aUser.Id, aClubId);
            return myClubMember != null && myClubMember.Administrator;
        }

        public bool IsApartOfClub(int aUserId, int aClubId) {
            ClubMember myClubMember = theClubRepository.GetClubMember(aUserId, aClubId);
            return myClubMember != null && !myClubMember.Deleted && myClubMember.Approved == UOMConstants.APPROVED;
        }

        public bool IsPendingApproval(int aUserId, int aClubId) {
            ClubMember myClubMember = theClubRepository.GetClubMember(aUserId, aClubId);
            return myClubMember != null && !myClubMember.Deleted && myClubMember.Approved == UOMConstants.PENDING;
        }

        public bool PostToClubBoard(UserInformationModel<User> aPostingUser, int aClubId, string aMessage) {
            if(!ValidatePostToBoard(aPostingUser.Details, aClubId, aMessage)) {
                return false;
            }

            theClubRepository.PostToClubBoard(aPostingUser.Details, aClubId, aMessage);

            return true;
        }

        public bool RemoveClubMember(UserInformationModel<User> aClubAdmin, int aCurrentUserId, int aClubId) {
            if (!ValidateRemovingClubMember(aClubAdmin.Details, aCurrentUserId, aClubId)) {
                return false;
            }

            theClubRepository.DeleteUserFromClub(aClubAdmin.Details, aCurrentUserId, aClubId);

            return true;
        }

        public void RequestToJoinClub(UserInformationModel<User> aRequestingMember, int aClubId) {
            theClubRepository.MemberRequestToJoinClub(aRequestingMember.Details, aClubId, ClubConstants.DEFAULT_NEW_MEMBER_TITLE);
        }

        private bool ValidateAdmin(User aUser, int aClubId) {
            if (!IsAdmin(aUser, aClubId)) {
                theValidationDictionary.AddError("ClubMemberAdmin", string.Empty, "You are not an admin of the club.");
                return false;
            }

            return true;
        }

        private bool ValidateRemovingClubMember(User aUserDoingRemoving, int aCurrentUserId, int aClubId) {
            ValidateClubExists(aUserDoingRemoving, aClubId);
            ClubMember myClubMember = theClubRepository.GetClubMember(aCurrentUserId, aClubId);
            if (myClubMember == null) {
                theValidationDictionary.AddError("ClubMember", string.Empty, "That club member doesn't exist.");
            }
            ClubMember myClubMemberDoingRemoving = theClubRepository.GetClubMember(aUserDoingRemoving.Id, aClubId);
            if (aUserDoingRemoving.Id != aCurrentUserId) {
                if (!myClubMemberDoingRemoving.Administrator) {
                    theValidationDictionary.AddError("ClubMemberAdmin", string.Empty, NOT_ADMINISTRATOR);
                }
            } 

            return theValidationDictionary.isValid;
        }

        private bool ValidatePostToBoard(User aPostingUser, int aClubId, string aMessage) {
            ValidateClubExists(aPostingUser, aClubId);
            
            ClubMember myPostingUserClubMember = theClubRepository.GetClubMember(aPostingUser.Id, aClubId);

            if (myPostingUserClubMember == null) {
                theValidationDictionary.AddError("ClubMember", string.Empty, "You are not part of the club so you can't post on the board.");
            }

            if (string.IsNullOrEmpty(aMessage)) {
                theValidationDictionary.AddError("BoardMessage", aMessage, "To post to the board you need to provide a message.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidMemberRequestToClub(User aUserAddingNewMemeber, int aClubId, int aClubMemberUserId) {
            ValidateClubExists(aUserAddingNewMemeber, aClubId);
            ValidateUserExists(aClubMemberUserId);

            ClubMember myAdminClubMember = theClubRepository.GetClubMember(aUserAddingNewMemeber.Id, aClubId);

            if (myAdminClubMember == null || !myAdminClubMember.Administrator) {
                theValidationDictionary.AddError("ClubMemberAdmin", aClubMemberUserId.ToString(), NOT_ADMINISTRATOR);
            }

            return theValidationDictionary.isValid;
        }

        private void ValidateUserExists(int aClubMemberUserId ) {
	        User myPotentialClubMember = theUserRetrievalService.GetUser(aClubMemberUserId);
            if (myPotentialClubMember == null) {
                theValidationDictionary.AddError("NewClubMember", aClubMemberUserId.ToString(), MEMBER_DOESNT_EXIST);
            }
        }

        private void ValidateClubExists(User aUser, int aClubId) {
            Club myClub = theClubRepository.GetClub(aUser, aClubId);
            if (myClub == null) {
                theValidationDictionary.AddError("Club", aClubId.ToString(), CLUB_DOESNT_EXIST);
            }
        }

        private void UpdateClubPhoto(string aName, HttpPostedFileBase aClubImage, Club myClub) {
            string myImageName = string.Empty;

            try {
                myImageName = SocialPhotoHelper.TakeImageAndResizeAndUpload(ClubConstants.CLUB_PHOTO_PATH, aName.Replace(" ", ""), aClubImage, ClubConstants.CLUB_MAX_SIZE);
            } catch (Exception myException) {
                throw new PhotoException("Error while resizing and uploading the club photo. ", myException);
            }
            try {
                myClub.Picture = myImageName;
                theClubRepository.UpdateClub(myClub);
            } catch (Exception myException) {
                SocialPhotoHelper.PhysicallyDeletePhoto(HttpContext.Current.Server.MapPath(ClubConstants.CLUB_PHOTO_PATH + myImageName));
                throw new CustomException("Error while updating the club to the new club photo.", myException);
            }
        }
        
        private bool ValidClub(ClubViewModel aCreateClubModel, bool aIsCreating) {
            if (aIsCreating) {
                ValidTitle(aCreateClubModel.Title);
            }

            if (string.IsNullOrEmpty(aCreateClubModel.Name)) {
                theValidationDictionary.AddError("Name", aCreateClubModel.Name, "A club name is required.");
            }

            if (string.IsNullOrEmpty(aCreateClubModel.Description)) {
                theValidationDictionary.AddError("Description", aCreateClubModel.Description, "Some description is required.");
            }

            if (string.IsNullOrEmpty(aCreateClubModel.UniversityId)) {
                theValidationDictionary.AddError("UniversityId", aCreateClubModel.UniversityId, "The club must be associated with a university.");
            }

            if (!DropDownItemValidation.IsValid(aCreateClubModel.ClubType)) {
                theValidationDictionary.AddError("ClubType", aCreateClubModel.ClubType, "Please select a club type.");
            }

            if (aCreateClubModel.ClubImage != null && !PhotoValidation.IsValidImageFile(aCreateClubModel.ClubImage.FileName)) {
                theValidationDictionary.AddError("ClubImage", aCreateClubModel.ClubImage.FileName, "Please specify a proper image file that ends in .gif, .jpg, or .jpeg.");
            }

            return theValidationDictionary.isValid;
        }

        private bool ValidTitle(string aTitle) {
            if (string.IsNullOrEmpty(aTitle)) {
                theValidationDictionary.AddError("Title", aTitle, "You must give yourself a title for the club. You can use the default title if you'd like: " + ClubConstants.DEFAULT_CLUB_LEADER_TITLE);
            }

            return theValidationDictionary.isValid;
        }
    }
}
