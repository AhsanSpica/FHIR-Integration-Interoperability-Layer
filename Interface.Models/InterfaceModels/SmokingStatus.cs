using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Model;

namespace Interface.Models.InterfaceModels
{
     

    public class SmokingStatusCore
    {
        public string Id { get; set; }
        public List<Identifier> Identifier { get; set; }
        public Meta Meta { get; set; }
        public readonly string ResourceType = "Observtion";
         public ResourceReference Encounter { get; set; }
        public ResourceReference Subject { get; set; }
        public ObservationStatus? Status { get; set; }
        public CodeableConcept Code { get; set; }
        public DataType Effective { get; set; }
        public Instant Issued { get; set; }
        public DataType Value { get; set; }
    }
  
  
}
