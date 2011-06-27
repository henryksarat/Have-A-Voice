using System;
using System.Collections.Generic;
using Social.Admin.Helpers;
using Social.Board.Repositories;
using Social.Generic.Constants;
using Social.Generic.Helpers;
using Social.Generic.Models;
using Social.Validation;
using Social.Admin.Exceptions;

namespace Social.Board.Services {
    public class BoardService<T, U, V> : IBoardService<T, U, V> {
        private IBoardRepository<T, U, V> theRepository;
        private IValidationDictionary theValidationDictionary;

        public BoardService(IValidationDictionary aValidationDictionary, IBoardRepository<T, U, V> aRepository) {
            theRepository = aRepository;
            theValidationDictionary = aValidationDictionary;
        }

        public U GetBoard(UserInformationModel<T> aUser, int aBoardId) {
            if (!AllowedToPerformAction(aUser, SocialPermission.View_Board)) {
                throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
            }

            theRepository.MarkBoardAsViewed(aUser.Details, aBoardId);
            return theRepository.FindBoardByBoardId(aBoardId);
        }

        public bool DeleteBoardMessage(UserInformationModel<T> aDeletingUser, int aBoardId) {
            if (!AllowedToPerformAction(aDeletingUser, SocialPermission.Delete_Board_Message, SocialPermission.Delete_Any_Board_Message)) {
                throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
            }
            bool myOverride = PermissionHelper<T>.AllowedToPerformAction(aDeletingUser, SocialPermission.Delete_Any_Board_Message);

            AbstractBoardModel<U> myOriginalBoard = theRepository.FindAbstractBoardByBoardId(aBoardId);

            if (myOriginalBoard.PostedUserId == aDeletingUser.UserId
                || myOriginalBoard.OwnerUserId == aDeletingUser.UserId
                || myOverride) {
                theRepository.DeleteBoardMessage(aDeletingUser.Details, myOriginalBoard.Model);
                return true;
            }

            throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
        }

        public bool EditBoardMessage(UserInformationModel<T> anEditBy, AbstractBoardModel<U> aNewBoard) {
            if (!ValidateBoardMessage(aNewBoard.Message)) {
                return false;
            }

            if (!AllowedToPerformAction(anEditBy, SocialPermission.Edit_Board_Message, SocialPermission.Edit_Any_Board_Message)) {
                throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
            }

            bool myOverride = PermissionHelper<T>.AllowedToPerformAction(anEditBy, SocialPermission.Edit_Any_Board_Message);

            AbstractBoardModel<U> myOriginalBoard = theRepository.FindAbstractBoardByBoardId(aNewBoard.Id);
            if (myOriginalBoard.PostedUserId == anEditBy.UserId || myOverride) {
                theRepository.EditBoardMessage(anEditBy.Details, myOriginalBoard.FromModel(), aNewBoard.FromModel());
                return true;
            }

            AddPermissionDeniedToValidationDictionary();
            return false;
        }

        public bool PostToBoard(UserInformationModel<T> aPostingUser, int aSourceUserId, string aMessage) {
            if (!ValidateBoardMessage(aMessage)) {
                return false;
            }
            if (!AllowedToPerformAction(aPostingUser, SocialPermission.Post_To_Board)) {
                throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
            }
            U myBoard = theRepository.AddToBoard(aPostingUser.Details, aSourceUserId, aMessage);

            theRepository.AddUserToBoardViewedState(aSourceUserId, myBoard, false);
            theRepository.AddUserToBoardViewedState(aPostingUser.UserId, myBoard, true);

            return true;
        }

        public bool DeleteBoardReply(UserInformationModel<T> aDeletingUser, int aBoardReplyId) {
            if (!AllowedToPerformAction(aDeletingUser, SocialPermission.Delete_Board_Reply, SocialPermission.Delete_Any_Board_Reply)) {
                throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
            }
            bool myOverrideDelete = PermissionHelper<T>.AllowedToPerformAction(aDeletingUser, SocialPermission.Delete_Any_Board_Reply);

            AbstractBoardReplyModel<V> myOriginalReply = theRepository.FindAbstractBoardReplyByBoardReplyId(aBoardReplyId);
            AbstractBoardModel<U> myBoard = theRepository.FindAbstractBoardByBoardId(myOriginalReply.BoardId);

            if (myOriginalReply.UserId == aDeletingUser.UserId
                || myBoard.OwnerUserId == aDeletingUser.UserId
                || myOverrideDelete) {
                theRepository.DeleteBoardReply(aDeletingUser.Details, myOriginalReply.Model);
                return true;
            }
            throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
        }

        public bool EditBoardReply(UserInformationModel<T> anEditedBy, AbstractBoardReplyModel<V> aBoardReply) {
            if (!ValidateBoardReply(aBoardReply.Message)) {
                return false;
            }
            if (!AllowedToPerformAction(anEditedBy, SocialPermission.Edit_Board_Reply, SocialPermission.Edit_Any_Board_Reply)) {
                return false;
            }

            bool myOverrideDelete = PermissionHelper<T>.AllowedToPerformAction(anEditedBy, SocialPermission.Edit_Any_Board_Reply);

            AbstractBoardReplyModel<V> myOriginalReply = theRepository.FindAbstractBoardReplyByBoardReplyId(aBoardReply.Id);
            if (myOriginalReply.UserId == anEditedBy.UserId || myOverrideDelete) {
                theRepository.EditBoardReply(anEditedBy.Details, myOriginalReply.FromModel(), aBoardReply.FromModel());
                return true;
            }
            return false;
        }

        public V FindBoardReply(int aReplyId) {
            return theRepository.FindBoardReplyByBoardReplyId(aReplyId);
        }

        public IEnumerable<V> FindBoardRepliesForBoard(UserInformationModel<T> aUser, int aBoardId) {
            if (!AllowedToPerformAction(aUser, SocialPermission.View_Board)) {
                return new LinkedList<V>();
            }
            return theRepository.FindBoardRepliesByBoard(aBoardId);
        }

        public bool PostReplyToBoard(UserInformationModel<T> aPostingUser, int aBoardId, string aReply) {
            if (!ValidateBoardReply(aReply)) {
                return false;
            }
            if (!AllowedToPerformAction(aPostingUser, SocialPermission.Post_Reply_To_Board)) {
                throw new PermissionDenied(ErrorKeys.PERMISSION_DENIED);
            }
            theRepository.AddReplyToBoard(aPostingUser.Details, aBoardId, aReply);


            return true;
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

        private bool AllowedToPerformAction(UserInformationModel<T> aUser, params SocialPermission[] aPermissions) {
            if (!PermissionHelper<T>.AllowedToPerformAction(aUser, aPermissions)) {
                AddPermissionDeniedToValidationDictionary();
            }

            return theValidationDictionary.isValid;
        }

        private void AddPermissionDeniedToValidationDictionary() {
            theValidationDictionary.AddError("PerformAction", string.Empty, Constants.PERMISSION_DENIED);
        }
    }
}