using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using HaveAVoice.Repositories.UserFeatures;
using HaveAVoice.Repositories;
using HaveAVoice.Helpers;
using HaveAVoice.Models.View;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVUserPrivacySettingsService : HAVBaseService, IHAVUserPrivacySettingsService {
        private IHAVUserPrivacySettingsRepository thePrivacySettingsRepo;

        private HAVPrivacySetting[] DEFAULT_PRIVACY_SETTINGS = new HAVPrivacySetting[] { HAVPrivacySetting.Display_Profile_To_Friend };

        public HAVUserPrivacySettingsService()
            : this(new HAVBaseRepository(), new EntityHAVUserPrivacySettingsRepository()) { }

        public HAVUserPrivacySettingsService(IHAVBaseRepository baseRepository, IHAVUserPrivacySettingsRepository aPrivacySettingsRepo)
            : base(baseRepository) {
            thePrivacySettingsRepo = aPrivacySettingsRepo;
        }

        public IEnumerable<PrivacySetting> FindPrivacySettingsForUser(User aUser) {
            return thePrivacySettingsRepo.FindPrivacySettingsForUser(aUser);
        }

        public void AddDefaultPrivacySettingsForUser(User aUser) {
            thePrivacySettingsRepo.AddPrivacySettingsForUser(aUser, DEFAULT_PRIVACY_SETTINGS);
        }

        public void UpdatePrivacySettings(User aUser, EditPrivacySettingsModel aPrivacySettings) {
            IEnumerable<string> myPrivacySettings = (from p in FindPrivacySettingsForUser(aUser)
                                                     select p.Name).ToList<string>();
            List<PrivacySetting> mySelectSettings = new List<PrivacySetting>();
            List<PrivacySetting> myNotSelectSettings = new List<PrivacySetting>();

            foreach (Pair<PrivacySetting, bool> myPair in aPrivacySettings.PrivacySettings.Values) {
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

        public EditPrivacySettingsModel GetPrivacySettingsForEdit(User aUser) {
            Dictionary<string, Pair<PrivacySetting, bool>> myPrivacySelection = new Dictionary<string, Pair<PrivacySetting, bool>>();

            IEnumerable<PrivacySetting> myPrivacySettings = FindPrivacySettingsForUser(aUser);
            IEnumerable<PrivacySetting> myAllPrivacySettings = thePrivacySettingsRepo.GetAllPrivacySettings();

            IEnumerable<PrivacySetting> myExcludedPrivacySettings = (from p in myAllPrivacySettings
                                                                     where !myPrivacySettings.Contains(p)
                                                                     select p).ToList<PrivacySetting>();


            foreach (PrivacySetting mySetting in myPrivacySettings) {
                myPrivacySelection.Add(mySetting.Name, 
                    new Pair<PrivacySetting, bool>() { 
                        First = mySetting, 
                        Second = true 
                    });
            }

            foreach (PrivacySetting mySetting in myExcludedPrivacySettings) {
                myPrivacySelection.Add(mySetting.Name,
                    new Pair<PrivacySetting, bool>() {
                        First = mySetting,
                        Second = false
                    });
            }

            return new EditPrivacySettingsModel() { 
                PrivacySettings = myPrivacySelection 
            };
        }
    }
}