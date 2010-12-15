using HaveAVoice.Models.Validation;
using HaveAVoice.Models.Repositories;
using HaveAVoice.Models.Repositories.UserFeatures;

namespace HaveAVoice.Services.UserFeatures {
    public class HAVSearchService : HAVBaseService, IHAVSearchService{
        private IValidationDictionary theValidationDictionary;
        private IHAVSearchRepository theRepository;

        public HAVSearchService(IValidationDictionary aValidationDictionary)
            : this(aValidationDictionary, new EntityHAVSearchRepository(), new HAVBaseRepository()) { }

        public HAVSearchService(IValidationDictionary aValidationDictionary, IHAVSearchRepository aRepository, 
                                             IHAVBaseRepository baseRepository) : base(baseRepository) {
            theValidationDictionary = aValidationDictionary;
            theRepository = aRepository;
        }

        public string SearchResult(string aSearchString) {
            return theRepository.SearchResult(aSearchString);
        }
    }
}
