using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.InterfaceModels
{
    public class MedicationStatementR4
   
    {
        public string ResourceType { get; set; } = "MedicationStatement";
        public string Id { get; set; }
        public Meta Meta { get; set; }
        public List<Identifier> Identifier { get; set; }
        public MedicationStatement.MedicationStatusCodes Status { get; set; }
        public ResourceReference Medication { get; set; }
        public ResourceReference Subject { get; set; }
        public ResourceReference Context { get; set; }
        public Period EffectivePeriod { get; set; }
        public DateTime DateAsserted { get; set; }
        public ResourceReference InformationSource { get; set; }
        public List<Dosage> Dosage { get; set; }
       // public MedicationStatement Taken { get; set; }
    }
    public class MedicationRequestR4

    {
        public string ResourceType { get; set; } = "MedicationRequest";
        public string Id { get; set; }
        public Meta Meta { get; set; }
        public List<Identifier> Identifier { get; set; }
        public MedicationRequest.MedicationrequestStatus Status { get; set; }
        public MedicationRequest.MedicationRequestIntent Intent { get; set; }
        public ResourceReference ReportedReference { get; set; }
        public ResourceReference Medication { get; set; }
        public ResourceReference Subject { get; set; }
        public ResourceReference Encounter { get; set; }
        public MedicationRequest.DispenseRequestComponent DispenseRequest {  get; set; }
        public DateTime AuthoredOn { get; set; }
        public ResourceReference Requester { get; set; }
        public List<Dosage> DosageInstruction { get; set; }
    }
    }
