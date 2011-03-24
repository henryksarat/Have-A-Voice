using System;
using Social.Generic.Models;

namespace Social.User.Models {
    public abstract class AbstractPrivacySettingModel<T> : AbstractSocialModel<T>, IEquatable<AbstractPrivacySettingModel<T>> {
        public string Name { get; set; }
        public int CreatedByUserId { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string PrivacyGroup { get; set; }
        public int ListOrder { get; set; }

        public abstract T FromModel();

        public override int GetHashCode() {
            const int myPrime = 397;
            int myResult = CreatedByUserId;
            myResult = (myResult * myPrime) ^ (Name != null ? Name.GetHashCode() : 0);
            myResult = (myResult * myPrime) ^ (DisplayName != null ? DisplayName.GetHashCode() : 0);
            myResult = (myResult * myPrime) ^ (Description != null ? Description.GetHashCode() : 0);
            myResult = (myResult * myPrime) ^ (PrivacyGroup != null ? PrivacyGroup.GetHashCode() : 0);
            myResult = (myResult * myPrime) ^ ListOrder;
            return myResult;
        }

        public static bool operator == (AbstractPrivacySettingModel<T> aLeft,
                                        AbstractPrivacySettingModel<T> aRight) {
            if (Object.ReferenceEquals(aLeft, null)) return false;
            else return aLeft.Equals(aRight);
        }

        public static bool operator != (AbstractPrivacySettingModel<T> aLeft,
                                        AbstractPrivacySettingModel<T> aRight) {
            return !(aLeft == aRight);
        }

        public override bool Equals(object obj) {
            AbstractPrivacySettingModel<T> myAbstract = obj as AbstractPrivacySettingModel<T>;
            if (myAbstract != null) {
                return Equals(myAbstract);
            } else {
                return false;
            }
        }

        public bool Equals(AbstractPrivacySettingModel<T> other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return (
                Name.Equals(other.Name)
                && CreatedByUserId == other.CreatedByUserId
                && DisplayName.Equals(other.DisplayName)
                && Description.Equals(other.Description)
                && PrivacyGroup.Equals(other.PrivacyGroup)
                && ListOrder == other.ListOrder
            );
        }
    }
}
