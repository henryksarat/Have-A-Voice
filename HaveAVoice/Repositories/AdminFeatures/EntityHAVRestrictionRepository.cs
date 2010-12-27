using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.AdminFeatures {
    public class EntityHAVRestrictionRepository : IHAVRestrictionRepository {
        private HaveAVoiceEntities theEntities = new HaveAVoiceEntities();

        public void CreateRestriction(User aCreatedByUser, Restriction aRestrictionToCreate) {
            aRestrictionToCreate.CreatedByUserId = aCreatedByUser.Id;
            theEntities.AddToRestrictions(aRestrictionToCreate);
            theEntities.SaveChanges();
        }

        public Restriction GetRestriction(int id) {
            return (from r in theEntities.Restrictions
                    where r.Id == id && r.Deleted == false
                    select r).FirstOrDefault();
        }

        public IEnumerable<Restriction> GetAllRestrictions() {
            return theEntities.Restrictions.ToList<Restriction>().Where(r=> r.Deleted == false);
        }

        public void DeleteRestriction(User aDeletedByUser, Restriction aRestrictionToDelete) {
            Restriction myOriginalRestriction = GetRestriction(aRestrictionToDelete.Id);
            myOriginalRestriction.Deleted = true;
            myOriginalRestriction.DeletedByUserId = aDeletedByUser.Id;
            myOriginalRestriction.DeletedDateTimeStamp = DateTime.UtcNow;

            theEntities.ApplyCurrentValues(myOriginalRestriction.EntityKey.EntitySetName, myOriginalRestriction);
            theEntities.SaveChanges();
        }

        public void EditRestriction(User anEditedByUser, Restriction aRestrictionToEdit) {
            Restriction myOriginalRestriction = GetRestriction(aRestrictionToEdit.Id);
            aRestrictionToEdit.EditedByUserId = anEditedByUser.Id;
            aRestrictionToEdit.EditedDateTimeStamp = DateTime.UtcNow;

            theEntities.ApplyCurrentValues(myOriginalRestriction.EntityKey.EntitySetName, aRestrictionToEdit);
            theEntities.SaveChanges();
        }
    }
}