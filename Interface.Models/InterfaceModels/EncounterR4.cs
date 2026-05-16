using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.InterfaceModels
{
    public class EncounterR4
    {
        public List<Identifier> Identifier { get; set; }
        public readonly string resourceType = "Encounter";
        public List<ResourceReference> Appointment {  get; set; }
        public string? Id { get; set; }
        public Coding? Class { get; set; }
        public List<CodeableConcept>? Type { get; set; }
        public ResourceReference? Subject { get; set; }
        public List<Encounter.ParticipantComponent>? Participant { get; set; }
        public Period? Period { get; set; }
        public List<CodeableConcept>? ReasonCode { get; set; }
        public List<Encounter.LocationComponent>? Location { get; set; }
        public Meta? Meta { get; set; }
    }
}
