using Interface.Models.GeneralLookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace IGeneralLookUpService
{
    public interface IGeneralLookupService
    {
        Task<Dictionary<string, List<GeneralLookup>>> GetByCriteria(string types, string lang, long practiceId);
        Task<List<Speciality>> GetAllSpecialty();
        Task<List<Speciality>> SearchSpecialty(string keyword);

        Task<List<Diseases>> GetAllDiseases(Diseases diseases);
    }
}
