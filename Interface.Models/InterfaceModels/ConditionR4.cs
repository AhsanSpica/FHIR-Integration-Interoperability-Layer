using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.InterfaceModels
{
    public class ConditionR4
    {
        public readonly string resourceType = "Condition";
        public string Id { get; set; }
        public Meta Meta { get; set; }
        public List<Identifier> Identifier { get; set; }
        public CodeableConcept ClinicalStatus { get; set; }
        public string VerificationStatus { get; set; }
        public List<CodeableConcept> Category { get; set; }
        public CodeableConcept Code { get; set; }
        public ResourceReference Subject { get; set; }
        public ResourceReference Encounter { get; set; }
        public DataType Onset { get; set; }
        public string RecordedDate { get; set; }
        public ResourceReference Recorder { get; set; }
        
        // Add other properties as needed
    }
}
