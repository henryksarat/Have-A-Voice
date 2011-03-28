using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Social.Generic;
using Social.Generic.Helpers;
using Social.User.Models;
using Social.User.Repositories;

namespace Social.User.Services {
    public class UserPrivacySettingsService<T, U> : IUserPrivacySettingsService<T, U> {
        private IUserPrivacySettingsRepository<T, U> thePrivacySettingsRepo;

        public UserPrivacySettingsService(IUserPrivacySettingsRepository<T, U> aPrivacySettingsRepo) {
            thePrivacySettingsRepo = aPrivacySettingsRepo;
        }

        public IEnumerable<U> FindPrivacySettingsForUser(T aUser) {
            return GetAbstractPrivacySettingsForUser(aUser).Select(p => p.FromModel());
        }

        public void AddPrivacySettingsForUser(T aUser, SocialPrivacySetting[] aPrivacySettings) {
            thePrivacySettingsRepo.AddPrivacySettingsForUser(aUser, aPrivacySettings);
        }

        public Dictionary<string, IEnumerable<Pair<U, bool>>> GetPrivacySettingsGrouped(T aUser) {
            Dictionary<string, IEnumerable<Pair<U, bool>>> myPrivacySelection = new Dictionary<string, IEnumerable<Pair<U, bool>>>();

            IEnumerable<AbstractPrivacySettingModel<U>> myPrivacySettings = GetAbstractPrivacySettingsForUser(aUser);
            IEnumerable<AbstractPrivacySettingModel<U>> myAllPrivacySettings = thePrivacySettingsRepo.GetAllPrivacySettings();
            IEnumerable<AbstractPrivacySettingModel<U>> myExcludedPrivacySettings =  myAllPrivacySettings.Except(myPrivacySettings);

            List<Pair<AbstractPrivacySettingModel<U>, bool>> myPairedPrivacySettings = new List<Pair<AbstractPrivacySettingModel<U>, bool>>();

            foreach (AbstractPrivacySettingModel<U> mySetting in myPrivacySettings) {
                myPairedPrivacySettings.Add(new Pair<AbstractPrivacySettingModel<U>, bool>() {
                    First = mySetting,
                    Second = true
                });
            }

            foreach (AbstractPrivacySettingModel<U> mySetting in myExcludedPrivacySettings) {
                myPairedPrivacySettings.Add(new Pair<AbstractPrivacySettingModel<U>, bool>() {
                    First = mySetting,
                    Second = false
                });
            }

            myPairedPrivacySettings = myPairedPrivacySettings.OrderByDescending(p => p.First.ListOrder).ToList();

            IEnumerable<string> myGroups = (from p in myAllPrivacySettings
                                            select p.PrivacyGroup).Distinct<string>().ToList<string>();

            foreach (string myGroup in myGroups) {
                IEnumerable<Pair<U, bool>> myGroupPair = (from p in myPairedPrivacySettings
                                                          where p.First.PrivacyGroup == myGroup
                                                          select new Pair<U, bool>() {
                                                              First = p.First.FromModel(),
                                                              Second = p.Second
                                                          }).ToList();
                myPrivacySelection.Add(myGroup, myGroupPair);
            }


            return myPrivacySelection;
        }

        public void UpdatePrivacySettings(T aUser, UpdatePrivacySettingsModel<U> aPrivacySettings) {
            IEnumerable<string> myPrivacySettings = (from p in GetAbstractPrivacySettingsForUser(aUser)
                                                     select p.Name).ToList<string>();
            List<AbstractPrivacySettingModel<U>> mySelectSettings = new List<AbstractPrivacySettingModel<U>>();
            List<AbstractPrivacySettingModel<U>> myNotSelectSettings = new List<AbstractPrivacySettingModel<U>>();

            foreach (Pair<AbstractPrivacySettingModel<U>, bool> myPair in aPrivacySettings.PrivacySettings) {
                if (myPair.Second) {
                    mySelectSettings.Add(myPair.First);
                } else {
                    myNotSelectSettings.Add(myPair.First);
                }
            }

            IEnumerable<U> myToRemove = (from p in myNotSelectSettings
                                         where myPrivacySettings.Contains(p.Name)
                                         select p).Select(p2 => p2.FromModel()).ToList();


            IEnumerable<U> myToAdd = (from p in mySelectSettings
                                      where !myPrivacySettings.Contains(p.Name)
                                      select p).Select(p2 => p2.FromModel()).ToList();

            thePrivacySettingsRepo.UpdatePrivacySettingsForUser(aUser, myToRemove, myToAdd);
        }

        public IEnumerable<AbstractPrivacySettingModel<U>> GetAbstractPrivacySettingsForUser(T aUser) {
            return thePrivacySettingsRepo.FindPrivacySettingsForUser(aUser);
        }
    }
}