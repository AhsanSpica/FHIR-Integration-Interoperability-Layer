using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.InterfaceModels
{
    public class ObservationLabCore : Observation
    {
        
        public List<CodeableConcept> Category { get; set; }

        // CodeableConcept for observation code (extensible binding to LOINC Codes)
        public CodeableConcept Code { get; set; }

        // Reference to the patient resource
        public ResourceReference Subject { get; set; }

        // Mandatory field with required binding to ObservationStatus
        public Code Status { get; set; }

        // Must Support fields

        // Either DateTime or Period for effective time (one or the other)
        public DateTime EffectiveDateTime { get; set; }
        public Period EffectivePeriod { get; set; }

        // Quantity or CodeableConcept for observation value (one or the other)
        public Quantity ValueQuantity { get; set; }
        public CodeableConcept ValueCodeableConcept { get; set; }

        public CodeableConcept DataAbsentReason { get; set; }
    }
}
