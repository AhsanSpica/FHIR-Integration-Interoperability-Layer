using AutoMapper;
using Hl7.Fhir.Model;
using Interface.Models.EncounterModels;
using Interface.Models.ImmunizationModels;
using Interface.Models.InterfaceModels;
using Interface.Models.Procedure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace FHIRMappers
{
  
    public class ProcedureFHIRMappingProfile : Profile
    {
        public ProcedureFHIRMappingProfile()
        {
            CreateMap<CombinedProcedureDTO, Procedure>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                 .ForMember(dest => dest.Category, opt => opt.MapFrom(src => MapCategory(src)))
                  .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta
                  {
                      LastUpdated = src.UpdatedAt ?? DateTimeOffset.UtcNow
                  }))
                .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => new List<Identifier> { new Identifier(getSystemUrl(src), src.Id.ToString()) }))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MapStatus(src)))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => new CodeableConcept
                {
                    Coding = new List<Coding>
                {
                    new Coding
                    {
                        System = MapProcedureSystem(src.ProcedureCodeType),
                        Code = src.ProcedureCode,
                        Display = src.CodeDetail
                    }
                },
                    Text = src.CodeDetail
                }))

                 .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.PatientReference))
                 .ForMember(dest => dest.Encounter, opt => opt.MapFrom(src => src.EncounterReference))

                //.ForMember(dest => dest.Subject, opt => opt.MapFrom(src => new ResourceReference($"Patient/{src.PatientId}")))
                //.ForMember(dest => dest.Encounter, opt => opt.MapFrom(src => new ResourceReference($"Encounter/{src.EncounterId}")))
                .ForMember(dest => dest.Performed, opt => opt.MapFrom(src => MapPeriod(src.FromDate, src.ToDate)))
                .ForMember(dest => dest.Recorder, opt => opt.MapFrom(src => src.RecorderId != null ? new ResourceReference($"Practitioner/{src.RecorderId}") : null))
                .ForMember(dest => dest.Report, opt => opt.MapFrom(src => src.ReportId != null ? new List<ResourceReference> { new ResourceReference($"DocumentReference/{src.ReportId}") } : null))

                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new ResourceReference($"Location/{src.POSCode}")));
        }
        private static CodeableConcept MapCategory(CombinedProcedureDTO src)
        {

            var categoryCode = src.ProcedureCodeType switch
            {
                "CPT" => "387713003", // Adjust category codes as needed
                "HCPCS" => "billing-code",
                "SNOMEDCT" => "clinical-procedure",
                _ => "unknown-category"
            };

            var categoryDisplay = src.ProcedureCodeType switch
            {
                "CPT" => "Surgical Procedure",
                "HCPCS" => "Billing Code",
                "SNOMEDCT" => "Clinical Procedure",
                _ => "Unknown Category"
            };

            return new CodeableConcept
            {
                Coding = new List<Coding>
            {
                new Coding
                {
                    System = "http://hl7.org/fhir/procedure-category",
                    Code = categoryCode,
                    Display = categoryDisplay
                }
            },
                Text = categoryDisplay
            };
        }
        private string getSystemUrl(CombinedProcedureDTO encounterInfoDto)
        {
            var proc = new Procedure();
            // var baseURL = _options.Value.EMRBaseURL;
            var baseURL = "https://qa.wmi360.com/EHR/api/main/api/v1";

            return $"{baseURL}encounters/getencounterbilledprocedurebyencounterid?EncounterId={encounterInfoDto.EncounterId}";
        }
        private static string MapProcedureSystem(string procedureCodeType)
        {
            return procedureCodeType switch
            {
                "CPT" => "http://www.ama-assn.org/go/cpt",
                "HCPCS" => "http://www.cms.gov/Medicare/Coding/HCPCSReleaseCodeSets",
                "SNOMEDCT" => "http://snomed.info/sct",
                _ => "http://example.com/unknown-system"
            };
        }
        private static Period MapPeriod(DateTimeOffset? fromDate, DateTimeOffset? toDate)
        {
            return new Period
            {
                Start = fromDate.HasValue ? fromDate.Value.ToString("yyyy-MM-ddTHH:mm:sszzz") : null,
                End = toDate.HasValue ? toDate.Value.ToString("yyyy-MM-ddTHH:mm:sszzz") : null
            };
        }
        private static EventStatus MapStatus(CombinedProcedureDTO src)
        {
            var isCompleted = false;
            if (src.FromDate.HasValue)
            {
                if (src.ToDate.HasValue)
                {
                    isCompleted = true;
                }
                else { isCompleted = false; }
            }
            var boolValue = StatusMapping.TryGetValue(isCompleted, out var procedureStatus);
            return procedureStatus;
        }

        private static readonly Dictionary<bool, EventStatus> StatusMapping = new Dictionary<bool, EventStatus>
        {

            { false, EventStatus.InProgress},
            { true, EventStatus.Completed}
        };

    }
}
