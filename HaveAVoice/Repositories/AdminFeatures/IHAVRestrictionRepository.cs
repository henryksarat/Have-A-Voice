﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Repositories.AdminFeatures {
    public interface IHAVRestrictionRepository {
        IEnumerable<Restriction> GetAllRestrictions();
        void CreateRestriction(User aCreatedByUser, Restriction aRestrictionToCreate);
        Restriction GetRestriction(int restrictionId);
        void DeleteRestriction(User aDeletedByUser, Restriction aRestrictionToDelete);
        void EditRestriction(User anEditedByUser, Restriction aRestrictionToEdit);
    }
}