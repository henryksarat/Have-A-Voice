using System.Text;
namespace HaveAVoice.Models.View {
    public class RestrictionModel {
        public Restriction Restriction { get; private set; }

        private RestrictionModel(Builder aBuilder) {
            Restriction = Restriction.CreateRestriction(
                    aBuilder.Id(),
                    aBuilder.Name(),
                    aBuilder.Description(),
                    aBuilder.IssuePostsWithinTimeLimit(),
                    aBuilder.IssuePostsTimeLimit(),
                    aBuilder.IssuePostsWaitTimeBeforePostSeconds(),
                    aBuilder.IssueReportsBeforeLockout(),
                    aBuilder.IssueReplyPostsWithinTimeLimit(),
                    aBuilder.IssueReplyPostsTimeLimit(),
                    aBuilder.IssueReplyPostsWaitTimeBeforePostSeconds(),
                    aBuilder.IssueReplyCommentPostsWithinTimeLimit(),
                    aBuilder.IssueReplyCommentPostsTimeLimit(),
                    aBuilder.IssueReplyCommentPostsWaitTimeBeforePostSeconds(),
                    aBuilder.MergeIssueRequestWithinTimeLimit(),
                    aBuilder.MergeIssueRequestTimeLimit(),
                    aBuilder.MergeIssueRequestWaitTimeBeforePostSeconds(),
                    aBuilder.CreatedByUserId(),
                    false);
        }

        public class Builder {
            private int theId;
            private int theCreatedByUserId;
            private string theName = string.Empty;
            private string theDescription = string.Empty;
            private int theIssuePostsWithinTimeLimit = 0;
            private long theIssuePostsTimeLimit = 0;
            private long theIssuePostsWaitTimeBeforePostSeconds = 0;
            private int theIssueReplyPostsWithinTimeLimit = 0;
            private long theIssueReplyPostsTimeLimit = 0;
            private long theIssueReplyPostsWaitTimeBeforePostSeconds = 0;
            private int theIssueReplyCommentPostsWithinTimeLimit = 0;
            private long theIssueReplyCommentPostsTimeLimit = 0;
            private long theIssueReplyCommentPostsWaitTimeBeforePostSeconds = 0;
            private int theMergeIssueRequestWithinTimeLimit = 0;
            private long theMergeIssueRequestTimeLimit = 0;
            private long theMergeIssueRequestWaitTimeBeforePostSeconds = 0;
            private int theIssueReportsBeforeLockout = 0;

            public Builder(int aId) {
                theId = aId;
            }

            public int Id() {
                return theId;
            }

            public int CreatedByUserId() {
                return theCreatedByUserId;
            }

            public string Name() {
                return theName;
            }

            public string Description() {
                return theDescription;
            }

            public int IssuePostsWithinTimeLimit() {
                return theIssuePostsWithinTimeLimit;
            }

            public long IssuePostsTimeLimit() {
                return theIssuePostsTimeLimit;
            }

            public long IssuePostsWaitTimeBeforePostSeconds() {
                return theIssuePostsWaitTimeBeforePostSeconds;
            }

            public int IssueReplyPostsWithinTimeLimit() {
                return theIssueReplyPostsWithinTimeLimit;
            }

            public long IssueReplyPostsTimeLimit() {
                return theIssueReplyPostsTimeLimit;
            }

            public long IssueReplyPostsWaitTimeBeforePostSeconds() {
                return theIssueReplyPostsWaitTimeBeforePostSeconds;
            }

            public int IssueReplyCommentPostsWithinTimeLimit() {
                return theIssueReplyCommentPostsWithinTimeLimit;
            }

            public long IssueReplyCommentPostsTimeLimit() {
                return theIssueReplyCommentPostsTimeLimit;
            }

            public long IssueReplyCommentPostsWaitTimeBeforePostSeconds() {
                return theIssueReplyCommentPostsWaitTimeBeforePostSeconds;
            }

            public int MergeIssueRequestWithinTimeLimit() {
                return theMergeIssueRequestWithinTimeLimit;
            }

            public long MergeIssueRequestTimeLimit() {
                return theMergeIssueRequestTimeLimit;
            }

            public long MergeIssueRequestWaitTimeBeforePostSeconds() {
                return theMergeIssueRequestWaitTimeBeforePostSeconds;
            }

            public int IssueReportsBeforeLockout() {
                return theIssueReportsBeforeLockout;
            }

            public Builder CreatedByUserId(int anId) {
                theCreatedByUserId = anId;
                return this;
            }

            public Builder Name(string aValue) {
                theName = aValue;
                return this;
            }

            public Builder Description(string aValue) {
                theDescription = aValue;
                return this;
            }

            public Builder IssuePostsWithinTimeLimit(int aValue) {
                theIssuePostsWithinTimeLimit = aValue;
                return this;
            }

            public Builder IssuePostsTimeLimit(long aValue) {
                theIssuePostsTimeLimit = aValue;
                return this;
            }

            public Builder IssuePostsWaitTimeBeforePostSeconds(long aValue) {
                theIssuePostsWaitTimeBeforePostSeconds = aValue;
                return this;
            }

            public Builder IssueReplyPostsWithinTimeLimit(int aValue) {
                theIssueReplyPostsWithinTimeLimit = aValue;
                return this;
            }

            public Builder IssueReplyPostsTimeLimit(long aValue) {
                theIssueReplyPostsTimeLimit = aValue;
                return this;
            }

            public Builder IssueReplyPostsWaitTimeBeforePostSeconds(long aValue) {
                theIssueReplyPostsWaitTimeBeforePostSeconds = aValue;
                return this;
            }

            public Builder IssueReplyCommentPostsWithinTimeLimit(int aValue) {
                theIssueReplyCommentPostsWithinTimeLimit = aValue;
                return this;
            }

            public Builder IssueReplyCommentPostsTimeLimit(long aValue) {
                theIssueReplyCommentPostsTimeLimit = aValue;
                return this;
            }

            public Builder IssueReplyCommentPostsWaitTimeBeforePostSeconds(long aValue) {
                theIssueReplyCommentPostsWaitTimeBeforePostSeconds = aValue;
                return this;
            }

            public Builder MergeIssueRequestWithinTimeLimit(int aValue) {
                theMergeIssueRequestWithinTimeLimit = aValue;
                return this;
            }

            public Builder MergeIssueRequestTimeLimit(long aValue) {
                theMergeIssueRequestTimeLimit = aValue;
                return this;
            }

            public Builder MergeIssueRequestWaitTimeBeforePostSeconds(long aValue) {
                theMergeIssueRequestWaitTimeBeforePostSeconds = aValue;
                return this;
            }

            public Builder IssueReportsBeforeLockout(int aValue) {
                theIssueReportsBeforeLockout = aValue;
                return this;
            }

            public RestrictionModel Build() {
                return new RestrictionModel(this);
            }
        }
        
        public override string ToString() {
            return new StringBuilder().AppendFormat(
                "id={0};name={1};description={2};"
                + "issuePostsWithinTimeLimit={3};issuePostsTimeLimit={4};issuePostsWaitTimeBeforePostSeconds={5};"
                + "issueReplyPostsWithinTimeLimit={6};issueReplyPostsTimeLimit={7};issueReplyPostsWaitTimeBeforePostSeconds={8};"
                + "issueReplyCommentPostsWithinTimeLimit={9};issueReplyCommentPostsTimeLimit={10};issueReplyCommentPostsWaitTimeBeforePostSeconds={11};"
                +"mergeIssueRequestWithinTimeLimit={12};mergeIssueRequestTimeLimit={13};mergeIssueRequestWaitTimeBeforePostSeconds={14};"
                +"issueReportsBeforeLockout={15}",
                Restriction.Id, Restriction.Name, Restriction.Description,
                Restriction.IssuePostsWithinTimeLimit, Restriction.IssuePostsTimeLimit, Restriction.IssuePostsWaitTimeBeforePostSeconds,
                Restriction.IssueReplyPostsWithinTimeLimit, Restriction.IssueReplyPostsTimeLimit, Restriction.IssueReplyPostsWaitTimeBeforePostSeconds,
                Restriction.IssueReplyCommentPostsWithinTimeLimit, Restriction.IssueReplyCommentPostsTimeLimit, Restriction.IssueReplyCommentPostsWaitTimeBeforePostSeconds,
                Restriction.MergeIssueRequestWithinTimeLimit, Restriction.MergeIssueRequestTimeLimit, Restriction.MergeIssueRequestWaitTimeBeforePostSeconds,
                Restriction.IssueReportsBeforeLockout)
                .ToString();   
        }

       
   
        
    }
}