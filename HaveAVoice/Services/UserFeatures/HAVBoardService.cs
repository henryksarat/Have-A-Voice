using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Validation;
using HaveAVoice.Repositories;
using HaveAVoice.Helpers;
using HaveAVoice.Models.View;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVBoardService : HAVBaseService, IHAVBoardService {
        private IHAVBoardRepository theRepository;
        private IValidationDictionary theValidationDictionary;

        public HAVBoardService(IValidationDictionary validationDictionary)
            : this(validationDictionary, new EntityHAVBoardRepository(), new HAVBaseRepository()) { }

        public HAVBoardService(IValidationDictionary aValidationDictionary, IHAVBoardRepository aRepository,
                                 IHAVBaseRepository aBaseRepository)
            : base(aBaseRepository) {
            theRepository = aRepository;
            theValidationDictionary = aValidationDictionary;
        }

        public Board GetBoard(UserInformationModel aUser, int aBoardId) {
            if (!AllowedToPerformAction(aUser, HAVPermission.View_Board)) {
                return null;
            }

            theRepository.MarkBoardAsViewed(aUser.Details, aBoardId);
            return theRepository.FindBoardByBoardId(aBoardId);
        }

        public bool PostToBoard(UserInformationModel aPostingUser, int aSourceUserId, string aMessage) {
            if (!ValidateBoardMessage(aMessage)) {
                return false;
            }
            if (!AllowedToPerformAction(aPostingUser, HAVPermission.Post_To_Board)) {
                return false;
            }
            Board myBoard = theRepository.AddToBoard(aPostingUser.Details, aSourceUserId, aMessage);

            theRepository.AddUserToBoardViewedState(aSourceUserId, myBoard, false);
            theRepository.AddUserToBoardViewedState(aPostingUser.Details.Id, myBoard, true);

            return true;
        }

        public bool EditBoardMessage(UserInformationModel anEditBy, Board aNewBoard) {
            if (!ValidateBoardMessage(aNewBoard.Message)) {
                return false;
            }

            if (!AllowedToPerformAction(anEditBy, HAVPermission.Edit_Board_Message, HAVPermission.Edit_Any_Board_Message)) {
                return false;
            }

            bool myOverride = HAVPermissionHelper.AllowedToPerformAction(anEditBy, HAVPermission.Edit_Any_Board_Message);

            Board myOriginalBoard = theRepository.FindBoardByBoardId(aNewBoard.Id);
            if (myOriginalBoard.PostedUserId == anEditBy.Details.Id || myOverride) {
                theRepository.EditBoardMessage(anEditBy.Details, myOriginalBoard, aNewBoard);
                return true;
            }

            AddPermissionDeniedToValidationDictionary();
            return false;
        }

        public bool DeleteBoardMessage(UserInformationModel aDeletingUser, int aBoardId) {
            if (!AllowedToPerformAction(aDeletingUser, HAVPermission.Delete_Board_Message, HAVPermission.Delete_Any_Board_Message)) {
                return false;
            }
            bool myOverride = HAVPermissionHelper.AllowedToPerformAction(aDeletingUser, HAVPermission.Delete_Any_Board_Message);

            Board myOriginalBoard = theRepository.FindBoardByBoardId(aBoardId);

            if (myOriginalBoard.PostedByUser.Id == aDeletingUser.Details.Id
                || myOriginalBoard.OwnerUserId == aDeletingUser.Details.Id
                || myOverride) {
                theRepository.DeleteBoardMessage(aDeletingUser.Details, myOriginalBoard);
                return true;
            }

            AddPermissionDeniedToValidationDictionary();
            return false;
        }

        public BoardReply FindBoardReply(int aReplyId) {
            return theRepository.FindBoardReplyByBoardReplyId(aReplyId);
        }

        public IEnumerable<BoardReply> FindBoardRepliesForBoard(UserInformationModel aUser, int aBoardId) {
            if (!AllowedToPerformAction(aUser, HAVPermission.View_Board)) {
                return new LinkedList<BoardReply>();
            }
            return theRepository.FindBoardRepliesByBoard(aBoardId);
        }

        public bool PostReplyToBoard(UserInformationModel aPostingUser, int aBoardId, string aReply) {
            if (!ValidateBoardReply(aReply)) {
                return false;
            }
            if (!AllowedToPerformAction(aPostingUser, HAVPermission.Post_Reply_To_Board)) {
                return false;
            }
            theRepository.AddReplyToBoard(aPostingUser.Details, aBoardId, aReply);


            return true;
        }

        public bool EditBoardReply(UserInformationModel anEditedBy, BoardReply aBoardReply) {
            if (!ValidateBoardReply(aBoardReply.Message)) {
                return false;
            }
            if (!AllowedToPerformAction(anEditedBy, HAVPermission.Edit_Board_Reply, HAVPermission.Edit_Any_Board_Reply)) {
                return false;
            }
            
            bool myOverrideDelete = HAVPermissionHelper.AllowedToPerformAction(anEditedBy, HAVPermission.Edit_Any_Board_Reply);

            BoardReply myOriginalReply = theRepository.FindBoardReplyByBoardReplyId(aBoardReply.Id);
            if (myOriginalReply.UserId == anEditedBy.Details.Id || myOverrideDelete) {
                theRepository.EditBoardReply(anEditedBy.Details, myOriginalReply, aBoardReply);
                return true;
            }
            return false;
        }

        public bool DeleteBoardReply(UserInformationModel aDeletingUser, int aBoardReplyId) {
            if (!AllowedToPerformAction(aDeletingUser, HAVPermission.Delete_Board_Reply, HAVPermission.Delete_Any_Board_Reply)) {
                return false;
            }
            bool myOverrideDelete = HAVPermissionHelper.AllowedToPerformAction(aDeletingUser, HAVPermission.Delete_Any_Board_Reply);

            BoardReply myOriginalReply = theRepository.FindBoardReplyByBoardReplyId(aBoardReplyId);
            Board myBoard = theRepository.FindBoardByBoardId(myOriginalReply.BoardId);

            if (myOriginalReply.User.Id == aDeletingUser.Details.Id
                || myBoard.OwnerUserId == aDeletingUser.Details.Id
                || myOverrideDelete) {
                theRepository.DeleteBoardReply(aDeletingUser.Details, myOriginalReply);
                return true;
            }
            return false;
        }

        private bool ValidateBoardMessage(string aBoardMessage) {
            if (String.IsNullOrEmpty(aBoardMessage)) {
                theValidationDictionary.AddError("BoardMessage", aBoardMessage, "A message must be entered to be posted to the board.");
            }
            return theValidationDictionary.isValid;
        }

        private bool ValidateBoardReply(string aBoardReply) {
            if (String.IsNullOrEmpty(aBoardReply)) {
                theValidationDictionary.AddError("BoardReply", aBoardReply, "You must enter text to reply to the board message.");
            }
            return theValidationDictionary.isValid;
        }

        private bool AllowedToPerformAction(UserInformationModel aUser, params HAVPermission[] aPermissions) {
            if (!HAVPermissionHelper.AllowedToPerformAction(aUser, aPermissions)) {
                AddPermissionDeniedToValidationDictionary();
            }

            return theValidationDictionary.isValid;
        }

        private void AddPermissionDeniedToValidationDictionary() {
            theValidationDictionary.AddError("PerformAction", string.Empty, ServiceConstants.PERMISSION_DENIED);
        }
    }
}