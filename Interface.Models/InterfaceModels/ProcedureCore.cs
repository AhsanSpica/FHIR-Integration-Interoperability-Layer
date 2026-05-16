using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.InterfaceModels
{
    public class ProcedureCore  
    {
        public string Id { get; set; }
        public string ResourceType { get; set; } = "Procedure";
        public List<Identifier> Identifier { get; set; }
        public Meta Meta { get; set; }
        public CodeableConcept Category { get; set; }
        public EventStatus  Status { get; set; }
        public CodeableConcept Code { get; set; }
        public ResourceReference Subject { get; set; }
        public ResourceReference Encounter { get; set; }
        public ResourceReference Recorder { get; set; }
        public List<ResourceReference> Report { get; set; }
        public ResourceReference Location { get; set; }
        public Period PerformedPeriod { get; set; } 
    }
   
}
