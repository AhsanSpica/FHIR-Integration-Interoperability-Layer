using Dapper;
using IGeneralLookUpRepository;
using Interface.Misc.Interfaces;
using Interface.Misc.Implementation;
using Interface.Models.GeneralLookups;
using System.Data;

namespace GeneralLookUpRepository
{
    public class GeneralLookupRepository : IGeneralLookupRepository
    {
        private IDBAccess _dBAccess;
        private readonly DBAccessFHIR _dBAccessFHIR;


        public GeneralLookupRepository(IDBAccess dBAccess,
            DBAccessFHIR dBAccessFHIR)
        {
            _dBAccess = dBAccess;
            _dBAccessFHIR = dBAccessFHIR;
        }

        public async Task<List<GeneralLookup>> GetByCriteria(string types, string lang, long practiceId)
        {
            var _params = new DynamicParameters();
            _params.Add("@Types", types);
            _params.Add("@Language", lang);
            _params.Add("@PracticeId", practiceId);
            return await _dBAccessFHIR.GetAll<GeneralLookup>("GetGeneralLookupFHIR",
                                                      _params, _dBAccess.GetConnectionString());
        }
       
        public async Task<List<Speciality>> GetAllSpecialty()
        {
            return await _dBAccess.GetAll<Speciality>("sp_GetAllSpecialty", null, _dBAccess.GetConnectionString());
        }
        public async Task<List<Speciality>> SearchSpecialty(string keyword)
        {
            var _param = new DynamicParameters();
            _param.Add("@keyword", keyword);
            return await _dBAccess.GetAll<Speciality>("sp_SearchSpecialty", _param, _dBAccess.GetConnectionString());
        }

        public async Task<List<Diseases>> GetAllDiseases(Diseases diseases)
        {
            var _param = new Dapper.DynamicParameters();
            _param.Add("@DiseaseName", diseases.DiseaseName);
            _param.Add("@PracticeId", diseases.PracticeId);
            return await _dBAccessFHIR.GetAll<Diseases>("GetAllDiseasesFHIR", _param, _dBAccess.GetConnectionString(), System.Data.CommandType.StoredProcedure);
        }
    }
}