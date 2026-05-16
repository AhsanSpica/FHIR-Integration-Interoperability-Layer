using Hl7.FhirPath.Sprache;
using IDocumentReferenceInfrastructure;
using IDocumentReferenceService;
using Interface.Models.DocumentReferenceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentReferenceService
{
    public class DocumentReferenceService:IDocumentReferenceService.IDocumentReferenceService
    {
        private readonly IDocumentReferenceInfrastructure.IDocumentReferenceInfrastructure _documentsInfrastructure;
         public DocumentReferenceService(IDocumentReferenceInfrastructure.IDocumentReferenceInfrastructure documentsInfrastructure   )
        {
            _documentsInfrastructure = documentsInfrastructure;
         }
        public async Task<PatientDocumentMultipleDto> GetPatientDocument(int DocumentId, long AssignedUserId)
        {
            return await _documentsInfrastructure.GetPatientDocument(DocumentId, AssignedUserId);
        }
    }
}
