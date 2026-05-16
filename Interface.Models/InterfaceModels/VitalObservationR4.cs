using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.InterfaceModels
{
    public class VitalObservationR4
    {
        public string Id { get; set; }
        public FhirDateTime Effective { get; set; }
        public Meta Meta { get; set; }
        public List<Identifier> Identifier { get; set; }
        public ResourceReference Subject { get; set; }
        public ResourceReference Encounter { get; set; }
        public Quantity Value { get; set; }
        public CodeableConcept Code { get; set; }
        public FhirDateTime Issued { get; set; }
        public CodeableConcept Method { get; set; }
        public string ResourceType { get; set; } = "Observation"; 
    }
}
