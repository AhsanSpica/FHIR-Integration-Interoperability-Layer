using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.InterfaceModels
{
    public class DocumentReferenceR4
    {
        
             public readonly string ResourceType = "DocumentReference";

             public List<Identifier> Identifier { get; set; }
        public Meta Meta { get; set; }
            public DocumentReferenceStatus? Status { get; set; }
            public CodeableConcept Type { get; set; }
            public ResourceReference Subject { get; set; }
            public FhirDateTime Date { get; set; }
            public List<ResourceReference> Author { get; set; }
            public ResourceReference Custodian { get; set; }
            public List<Attachment> Content { get; set; }
            public DocumentReference.ContextComponent Context { get; set; }
        public List<Extension> Extension { get; set; }
       
        }
     
}
