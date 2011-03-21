using HaveAVoice.Models;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.UserFeatures;
using Social.Validation;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVComplaintService : HAVBaseService, IHAVComplaintService {
        private IHAVComplaintRepository theRepository;
        private IValidationDictionary theValidationDictionary;

        public HAVComplaintService(IValidationDictionary validationDictionary)
            : this(validationDictionary, new EntityHAVComplaintRepository(), new HAVBaseRepository()) { }

        public HAVComplaintService(IValidationDictionary aValidationDictionary, IHAVComplaintRepository aRepository,
                                   IHAVBaseRepository aBaseRepository) : base(aBaseRepository) {
            theRepository = aRepository;
            theValidationDictionary = aValidationDictionary;
        }

        public bool IssueComplaint(User aFiledBy, string aComplaint, int aIssueId) {
            if (!ValidComplaint(aComplaint, aIssueId)) {
                return false;
            }
            theRepository.AddIssueComplaint(aFiledBy, aComplaint, aIssueId);
            return true;
        }

        public bool IssueReplyComplaint(User aFiledBy, string aComplaint, int aIssueReplyId) {
            if (!ValidComplaint(aComplaint, aIssueReplyId)) {
                return false;
            }
            theRepository.AddIssueReplyComplaint(aFiledBy, aComplaint, aIssueReplyId);
            return true;
        }

        public bool IssueReplyCommentComplaint(User aFiledBy, string aComplaint, int aIssueReplyCommentId) {
            if (!ValidComplaint(aComplaint, aIssueReplyCommentId)) {
                return false;
            }
            theRepository.AddIssueReplyCommentComplaint(aFiledBy, aComplaint, aIssueReplyCommentId);
            return true;
        }

        public bool ProfileComplaint(User aFiledBy, string aComplaint, int aTowardUserId) {
            if (!ValidComplaint(aComplaint, aTowardUserId)) {
                return false;
            }
            theRepository.AddProfileComplaint(aFiledBy, aComplaint, aTowardUserId);
            return true;
        }

        public bool PhotoComplaint(User aFiledBy, string aComplaint, int aPhotoId) {
            if (!ValidComplaint(aComplaint, aPhotoId)) {
                return false;
            }
            theRepository.AddPhotoComplaint(aFiledBy, aComplaint, aPhotoId);
            return true;
        }

        private bool ValidComplaint(string aComplaint, int aSourceId) {
            if (aSourceId <= 0) {
                theValidationDictionary.AddError("ComplaintSourceId", string.Empty, "Can't tell what you are reporting about. Please go back and try again.");
            }
            if (aComplaint.Trim().Length == 0) {
                theValidationDictionary.AddError("Complaint", aComplaint.Trim(), "Complaint is required.");
            }

            return theValidationDictionary.isValid;
        }
    }
}
