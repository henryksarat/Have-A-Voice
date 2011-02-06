using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Repositories;
using HaveAVoice.Helpers;
using HaveAVoice.Models.View;
using System.Collections;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVUserPrivacySettingsService : HAVBaseService, IHAVUserPrivacySettingsService {
        private IHAVUserPrivacySettingsRepository thePrivacySettingsRepo;

        public HAVUserPrivacySettingsService()
            : this(new HAVBaseRepository(), new EntityHAVUserPrivacySettingsRepository()) { }

        public HAVUserPrivacySettingsService(IHAVBaseRepository baseRepository, IHAVUserPrivacySettingsRepository aPrivacySettingsRepo)
            : base(baseRepository) {
            thePrivacySettingsRepo = aPrivacySettingsRepo;
        }

        public IEnumerable<PrivacySetting> FindPrivacySettingsForUser(User aUser) {
            return thePrivacySettingsRepo.FindPrivacySettingsForUser(aUser);
        }

        public void AddAuthorityPrivacySettingsForUser(User aUser) {
            thePrivacySettingsRepo.AddPrivacySettingsForUser(aUser, HAVPrivacyHelper.GetAuthorityPrivacySettings());
        }

        public void AddDefaultPrivacySettingsForUser(User aUser) {
            thePrivacySettingsRepo.AddPrivacySettingsForUser(aUser, HAVPrivacyHelper.GetDefaultPrivacySettings());
        }

        public void UpdatePrivacySettings(User aUser, UpdatePrivacySettingsModel aPrivacySettings) {
            IEnumerable<string> myPrivacySettings = (from p in FindPrivacySettingsForUser(aUser)
                                                     select p.Name).ToList<string>();
            List<PrivacySetting> mySelectSettings = new List<PrivacySetting>();
            List<PrivacySetting> myNotSelectSettings = new List<PrivacySetting>();

            foreach (Pair<PrivacySetting, bool> myPair in aPrivacySettings.PrivacySettings) {
                if (myPair.Second) {
                    mySelectSettings.Add(myPair.First);
                } else {
                    myNotSelectSettings.Add(myPair.First);
                }
            }

            IEnumerable<PrivacySetting> myToRemove = (from p in myNotSelectSettings
                                                      where myPrivacySettings.Contains(p.Name)
                                                      select p).ToList<PrivacySetting>();


            IEnumerable<PrivacySetting> myToAdd = (from p in mySelectSettings
                                                      where !myPrivacySettings.Contains(p.Name)
                                                      select p).ToList<PrivacySetting>();

            thePrivacySettingsRepo.UpdatePrivacySettingsForUser(aUser, myToRemove, myToAdd);
        }

        public DisplayPrivacySettingsModel GetPrivacySettingsForEdit(User aUser) {
            Dictionary<string, IEnumerable<Pair<PrivacySetting, bool>>> myPrivacySelection = new Dictionary<string, IEnumerable<Pair<PrivacySetting, bool>>>();

            IEnumerable<PrivacySetting> myPrivacySettings = FindPrivacySettingsForUser(aUser);
            IEnumerable<PrivacySetting> myAllPrivacySettings = thePrivacySettingsRepo.GetAllPrivacySettings();

            IEnumerable<PrivacySetting> myExcludedPrivacySettings = (from p in myAllPrivacySettings
                                                                     where !myPrivacySettings.Contains(p)
                                                                     select p).ToList<PrivacySetting>();

            IEnumerable<string> myGroups = (from p in myAllPrivacySettings
                                            select p.PrivacyGroup).Distinct<string>().ToList<string>();

            List<Pair<PrivacySetting, bool>> myPairedPrivacySettings = new List<Pair<PrivacySetting, bool>>();


            foreach (PrivacySetting mySetting in myPrivacySettings) {
                myPairedPrivacySettings.Add(new Pair<PrivacySetting, bool>() { 
                        First = mySetting, 
                        Second = true 
                    });
            }

            foreach (PrivacySetting mySetting in myExcludedPrivacySettings) {
                myPairedPrivacySettings.Add(new Pair<PrivacySetting, bool>() {
                        First = mySetting,
                        Second = false
                    });
            }

            myPairedPrivacySettings = myPairedPrivacySettings.OrderByDescending(p => p.First.ListOrder).ToList();

            foreach (string myGroup in myGroups) {
                IEnumerable<Pair<PrivacySetting, bool>> myGroupPair = (from p in myPairedPrivacySettings
                                                  where p.First.PrivacyGroup == myGroup
                                                  select p).ToList<Pair<PrivacySetting, bool>>();
                myPrivacySelection.Add(myGroup, myGroupPair);
            }


            return new DisplayPrivacySettingsModel(aUser) {
                PrivacySettings = myPrivacySelection 
            };
        }
    }
}