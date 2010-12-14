using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Helpers.Enums;
using System.Text;

namespace HaveAVoice.Models.View {
    public class ComplaintModel {
        public int SourceId { get; private set; }
        public User TowardUser { get; private set; }
        public string SourceDescription { get; private set; }
        public string Complaint { get; private set; }
        public ComplaintType ComplaintType { get; private set; }


        private ComplaintModel(Builder aBuilder) {
            SourceId = aBuilder.SourceId();
            TowardUser = aBuilder.TowardUser();
            SourceDescription = aBuilder.SourceDescription();
            Complaint = aBuilder.Complaint();
            ComplaintType = aBuilder.ComplaintType();
        }

        public class Builder {
            private int theSourceId;
            private User theTowardUser;
            private ComplaintType theComplaintType;
            private string theSourceDescription;
            private string theComplaint;

            public Builder(int aSourceId, ComplaintType aComplaintType) {
                theSourceId = aSourceId;
                theComplaintType = aComplaintType;
                theTowardUser = new User();
                theSourceDescription = string.Empty;
                theComplaint = string.Empty;
            }

            public Builder Complaint(string aComplaint) {
                theComplaint = aComplaint;
                return this;
            }

            public Builder SourceDescription(string aDescription) {
                theSourceDescription = aDescription;
                return this;
            }

            public Builder TowardUser(User aUser) {
                theTowardUser = aUser;
                return this;
            }

            public ComplaintType ComplaintType() {
                return theComplaintType;
            }

            public string Complaint() {
                return theComplaint;
            }

            public string SourceDescription() {
                return theSourceDescription;
            }

            public User TowardUser() {
                return theTowardUser;
            }

            public int SourceId() {
                return theSourceId;
            }

            public ComplaintModel Build() {
                return new ComplaintModel(this);
            }
        }

        public override string ToString() {
            return new StringBuilder().AppendFormat(
                "sourceId={0};towardUserId={1};sourceDescription={2};complaint={3};complaintType={4}",
                SourceId,
                TowardUser.Id,
                SourceDescription,
                Complaint,
                ComplaintType)
                .ToString();
        }
    }
}