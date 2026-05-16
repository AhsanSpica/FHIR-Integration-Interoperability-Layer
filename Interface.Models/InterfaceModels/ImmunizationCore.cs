using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.InterfaceModels
{
    public class ImmunizationCore 
    {
        public string Id { get; set; }
        public List<Identifier> Identifier { get; set; }
        public Meta Meta { get; set; }
        public readonly string ResourceType = "Immunization";
        public CodeableConcept VaccineCode { get; set; }
        public DataType Occurrence { get; set; }
        public string Recorded {  get; set; }
        public ResourceReference Location {  get; set; }

        // Reference to the patient resource
        public ResourceReference Patient { get; set; }

        // Mandatory field with required binding to Immunization Status Codes
        public Code Status { get; set; }

        // DateTime for the immunization occurrence (can be single or a range)
        //  public Extension Occurrence { get; set; }
       // public DateTime occurrenceDateTime { get; set; }
        // Boolean indicating if this is the primary source of information
        public bool PrimarySource { get; set; }

        // Must Support fields

        // CodeableConcept for reason why immunization wasn't given (optional)
        public CodeableConcept StatusReason { get; set; }
    }
}
