using Hl7.Fhir.Model;
using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.EncounterModels
{
    public class EncounterPatientVitalDto : BaseModel
    {
        public Guid SessionId { get; set; }
        public DateTimeOffset? SessionDate { get; set; }
        public long Id { get; set; }
        public long PatientId { get; set; }
        public long? EncounterId { get; set; }
        public long VitalTypeId { get; set; }
        public long VitalSubTypeId { get; set; }
        public int? Source { get; set; }
        public int Position { get; set; }
        public decimal? Value { get; set; }
        public string? ReadBy { get; set; }
        public DateTimeOffset ReadAt { get; set; }
        public bool Action { get; set; }
        public string? PatientName { get; set; }
        public string? VitalName { get; set; }
        public string? VitalDesc { get; set; }
        public string? VstName { get; set; }
        public string? VstDesc { get; set; }
        public string? LOINC { get; set; }

        public string? Unit { get; set; }
        public string? ShortName { get; set; }
        public decimal? MinRange { get; set; }
        public decimal? CriticalMinRange { get; set; }
        public decimal? MaxRanage { get; set; }
        public decimal? CriticalMaxRange { get; set; }
        public decimal? DefaultValue { get; set; }
        public string? SourceText { get; set; }
        public string? PositionText { get; set; }

        //new 

        public string? ObservationStatus { get; set; }
        //ETL 
        public string? PatientMrn { get; set; }
        public ResourceReference PatientReference { get; set; }
        public ResourceReference EncounterReference { get; set; }
        public ResourceReference PractitionerReference { get; set; }
    }
    

 public class PatientVitalsSessionViewModel
    {
        public Guid? SessionId { get; set; }
        public long EncounterId { get; set; }
        public DateTimeOffset SessionDate { get; set; }
        public List<PatientVitalViewModel>? PatientVitalViewModels { get; set; }
    }

    public class PatientVitalViewModel
    {
        public string? VitalName { get; set; }
        public string? VitalValue { get; set; }
        public List<EncounterPatientVitalDto>? ListOfPatientVitals { get; set; }
    }
    public class PatientVitalsSession
    {
        public Guid? SessionId { get; set; }
        public long EncounterId { get; set; }
        public DateTimeOffset SessionDate { get; set; }
        public List<EncounterPatientVitalDto>? ListOfPatientVitals { get; set; }
    }

   
}
