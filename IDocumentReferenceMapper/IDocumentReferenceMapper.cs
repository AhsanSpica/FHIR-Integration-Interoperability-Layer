using Hl7.Fhir.Model;
 using Interface.Models.InterfaceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDocumentReferenceMapper
{
    public interface IDocumentReferenceMapper
    {
        Task<CustomBundle> GetPatientDocument(int DocumentId, long AssignedUserId);

    }
}
