using Interface.Models.DocumentReferenceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDocumentReferenceRepository
{
    public interface  IDocumentReferenceRepository
    {
        Task<PatientDocumentMultipleDto> GetPatientDocument(int DocumentId, long AssignedUserId);

    }
}
