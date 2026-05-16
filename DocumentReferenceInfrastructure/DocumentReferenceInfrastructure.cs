using IDocumentReferenceInfrastructure;
using IDocumentReferenceRepository;
using Interface.Models.DocumentReferenceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentReferenceInfrastructure
{
    public class DocumentReferenceInfrastructure : IDocumentReferenceInfrastructure.IDocumentReferenceInfrastructure
    {
        private readonly IDocumentReferenceRepository.IDocumentReferenceRepository _documentsRepository;

        public DocumentReferenceInfrastructure(IDocumentReferenceRepository.IDocumentReferenceRepository documentsRepository)
        {
            _documentsRepository = documentsRepository;
        }
        public async Task<PatientDocumentMultipleDto> GetPatientDocument(int DocumentId, long AssignedUserId)
        {
            return await _documentsRepository.GetPatientDocument(DocumentId, AssignedUserId);
        }
    }
}
