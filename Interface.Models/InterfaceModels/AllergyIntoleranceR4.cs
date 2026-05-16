using Hl7.Fhir.Model;
using Interface.Models.EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.InterfaceModels
{
    public class AllergyIntoleranceR4
    {

        public string Id { get; set; }
        public readonly string ResourceType = "AllergyIntolerence";
        public List<Identifier> Identifier { get; set; }
        public ResourceReference Patient { get; set; }
        public ResourceReference Encounter { get; set; }
        public CodeableConcept ClinicalStatus { get; set; }
        public CodeableConcept VerificationStatus { get; set; }
        public List<CodeableConcept> Category { get; set; }
       // public AllergyIntolerance.AllergyIntoleranceCriticality? Criticality { get; set; }
        public string? Criticality { get; set; }
        public CodeableConcept Code { get; set; }
        public FhirDateTime OnsetDateTime { get; set; }
        public FhirDateTime RecordedDate { get; set; }
        public ResourceReference Recorder { get; set; }
        public List<AllergyIntolerance.ReactionComponent> Reaction { get; set; }
        public Meta Meta { get; set; }

    
    }
}
