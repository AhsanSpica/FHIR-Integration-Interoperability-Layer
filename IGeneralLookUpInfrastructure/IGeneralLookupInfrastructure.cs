using Interface.Models.GeneralLookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGeneralLookUpInfrastructure
{
    public interface IGeneralLookupInfrastructure
    {
        Task<List<GeneralLookup>> GetByCriteria(string types, string lang = "en-us", long practiceId = 0);
        Task<List<Speciality>> GetAllSpecialty();
        Task<List<Speciality>> SearchSpecialty(string keyword);

        Task<List<Diseases>> GetAllDiseases(Diseases diseases);
    }
}
