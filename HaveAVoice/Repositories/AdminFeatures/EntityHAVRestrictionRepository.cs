using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.AdminFeatures {
    public class EntityHAVRestrictionRepository : HAVBaseRepository, IHAVRestrictionRepository {
        public void CreateRestriction(User aCreatedByUser, Restriction aRestrictionToCreate) {
            aRestrictionToCreate.CreatedByUserId = aCreatedByUser.Id;
            GetEntities().AddToRestrictions(aRestrictionToCreate);
            GetEntities().SaveChanges();
        }

        public Restriction GetRestriction(int id) {
            return (from r in GetEntities().Restrictions
                    where r.Id == id && r.Deleted == false
                    select r).FirstOrDefault();
        }

        public IEnumerable<Restriction> GetAllRestrictions() {
            return GetEntities().Restrictions.ToList<Restriction>().Where(r=> r.Deleted == false);
        }

        public void DeleteRestriction(User aDeletedByUser, Restriction aRestrictionToDelete) {
            Restriction myOriginalRestriction = GetRestriction(aRestrictionToDelete.Id);
            myOriginalRestriction.Deleted = true;
            myOriginalRestriction.DeletedByUserId = aDeletedByUser.Id;
            myOriginalRestriction.DeletedDateTimeStamp = DateTime.UtcNow;

            GetEntities().ApplyCurrentValues(myOriginalRestriction.EntityKey.EntitySetName, myOriginalRestriction);
            GetEntities().SaveChanges();
        }

        public void EditRestriction(User anEditedByUser, Restriction aRestrictionToEdit) {
            Restriction myOriginalRestriction = GetRestriction(aRestrictionToEdit.Id);
            aRestrictionToEdit.EditedByUserId = anEditedByUser.Id;
            aRestrictionToEdit.EditedDateTimeStamp = DateTime.UtcNow;

            GetEntities().ApplyCurrentValues(myOriginalRestriction.EntityKey.EntitySetName, aRestrictionToEdit);
            GetEntities().SaveChanges();
        }
    }
}