using IDocumentReferenceRepository;
using Interface.Misc.Interfaces;
using Interface.Models.DocumentReferenceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentReferenceRepository
{
    public class DocumentReferenceRepository : IDocumentReferenceRepository.IDocumentReferenceRepository
    {
        private readonly IDBAccess _dBAccess;


        public DocumentReferenceRepository(IDBAccess dBAccess)
        {
            _dBAccess = dBAccess;
        }
        public async Task<PatientDocumentMultipleDto>  GetPatientDocument(int DocumentId, long AssignedUserId)
        {
            var _param = new Dapper.DynamicParameters();
            _param.Add("@DocumentId", DocumentId);
            _param.Add("@AssignedUserId", AssignedUserId);

            PatientDocumentMultipleDto patientDocumentMultipleDto = new PatientDocumentMultipleDto();
            var tupleResult = await _dBAccess.GetAllMultiple1<PatientDocumentMultipleDto, DocumentUsers, DocumentActionODT>("GetPatientDocument",
             _param, _dBAccess.GetConnectionString(), System.Data.CommandType.StoredProcedure);

            if (tupleResult.Item1.Count() > 0)
            {
                patientDocumentMultipleDto = (PatientDocumentMultipleDto)tupleResult.Item1.First();
                patientDocumentMultipleDto.AssignedUsers = tupleResult.Item2.ToList();
                patientDocumentMultipleDto.documentActions = tupleResult.Item3.ToList();
            }

            return patientDocumentMultipleDto;
        }
    }
}
