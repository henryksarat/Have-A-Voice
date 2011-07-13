using Social.Generic.Models;

namespace UniversityOfMe.Models.SocialModels {
    public class SocialBoardModel : AbstractBoardModel<Board> {
        public static SocialBoardModel Create(Board anExternal) {
            return new SocialBoardModel(anExternal);
        }

        public SocialBoardModel() { }

        public override Board FromModel() {
            Board myBoard = Board.CreateBoard(Id, OwnerUserId, PostedUserId, Message, DateTimeStamp, Deleted);
            myBoard.UpdatedByUserId = UpdatedByUserId;
            myBoard.UpdatedDateTimeStamp = UpdatedDateTimeStamp;
            myBoard.DeletedByUserId = DeletedByUserId;
            return myBoard;
        }

        private SocialBoardModel(Board anExternal) {
            Model = anExternal;

            Id = anExternal.Id;
            OwnerUserId = anExternal.OwnerUserId;
            PostedUserId = anExternal.PostedUserId;
            Message = anExternal.Message;
            DateTimeStamp = anExternal.DateTimeStamp;
            UpdatedByUserId = anExternal.UpdatedByUserId;
            UpdatedDateTimeStamp = anExternal.UpdatedDateTimeStamp;
            Deleted = anExternal.Deleted;
            DeletedByUserId = anExternal.DeletedByUserId;
        }
    }
}