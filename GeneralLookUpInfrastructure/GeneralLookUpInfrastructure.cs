using IGeneralLookUpInfrastructure;
using IGeneralLookUpRepository;
using Interface.Models.GeneralLookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace GeneralLookUpInfrastructure
{
    public class GeneralLookupInfrastructure : IGeneralLookupInfrastructure
    {
        private readonly IGeneralLookupRepository _generalLookupRepository;
        public GeneralLookupInfrastructure(IGeneralLookupRepository generalLookupRepository)
        {
            _generalLookupRepository = generalLookupRepository;
        }
        public async Task<List<GeneralLookup>> GetByCriteria(string types, string lang, long practiceId = 0)
        {
            return await _generalLookupRepository.GetByCriteria(types, lang, practiceId);
        }

       
        public async Task<List<Speciality>> GetAllSpecialty()
        {
            return await _generalLookupRepository.GetAllSpecialty();
        }
        public async Task<List<Speciality>> SearchSpecialty(string keyword)
        {
            return await _generalLookupRepository.SearchSpecialty(keyword);
        }

        public async Task<List<Diseases>> GetAllDiseases(Diseases diseases)
        {
            return await _generalLookupRepository.GetAllDiseases(diseases);
        }
         
    }
}

