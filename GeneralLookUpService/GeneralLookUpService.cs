using IGeneralLookUpInfrastructure;
using IGeneralLookUpService;
using Interface.Models.GeneralLookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace GeneralLookUpService
{
    public class GeneralLookUpService : IGeneralLookupService
    {
        private readonly IGeneralLookupInfrastructure _generalLookupInfrastructure;
        public GeneralLookUpService(IGeneralLookupInfrastructure generalLookupInfrastructure)
        {
            _generalLookupInfrastructure = generalLookupInfrastructure;
        }

        public async Task<Dictionary<string, List<GeneralLookup>>> GetByCriteria(string types, string lang, long practiceId)
        {
            Dictionary<string, List<GeneralLookup>> Criteria =
                new Dictionary<string, List<GeneralLookup>>();

            var list = await _generalLookupInfrastructure.GetByCriteria(types, lang, practiceId);

            var groupList = list.GroupBy(x => x.Type).ToList();
            //groupList.ForEach(x => Criteria.Add(x.Key, x.ToList()));
            foreach (var group in groupList)
            {
                Criteria.Add(group.Key, group.ToList());
            }
            return Criteria;
        }

        public async Task<List<Speciality>> GetAllSpecialty()
        {
            return await _generalLookupInfrastructure.GetAllSpecialty();
        }
        public async Task<List<Speciality>> SearchSpecialty(string keyword)
        {
            return await _generalLookupInfrastructure.SearchSpecialty(keyword);
        }

        public async Task<List<Diseases>> GetAllDiseases(Diseases diseases)
        {
            return await _generalLookupInfrastructure.GetAllDiseases(diseases);
        }
    }
}