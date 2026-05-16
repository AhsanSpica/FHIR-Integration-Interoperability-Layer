using AutoMapper;
using GlobalHelpers;
using Hl7.Fhir.Model;
using Interface.Models.EncounterModels;
using Interface.Models.InterfaceModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIRMappers
{
    public class ProblemConditionFHIRMappingProfile : Profile
    {
        public ProblemConditionFHIRMappingProfile()
        {
            CreateMap<PatientProblem, Condition>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => new List<Identifier> { new Identifier(getSystemUrl(src), src.Id.ToString()) }))
                  .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta
                  {
                      LastUpdated = src.UpdatedAt ?? DateTimeOffset.UtcNow
                  }))
                .ForMember(dest => dest.ClinicalStatus, opt => opt.MapFrom(src => MapClinicalStatus(src)))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => MapCategory(src)))
                .ForMember(dest => dest.Onset, opt => opt.MapFrom(src => src.OnsetDate.HasValue ? new FhirDateTime(src.OnsetDate.Value.Date) : null))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.PatientReference))
                .ForMember(dest => dest.Encounter, opt => opt.MapFrom(src => src.EncounterId.HasValue ? src.EncounterReference : null))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => MapConditionCode(src)))
                .ForMember(dest => dest.Recorder, opt => opt.MapFrom(src => src.PractitionerReference))
                .ForMember(dest => dest.RecordedDate, opt => opt.MapFrom(src => src.CreatedAt.HasValue ? new FhirDateTime(src.CreatedAt.Value).ToString() : null))
                .ReverseMap(); // If bidirectional mapping is needed
        }
        private CodeableConcept MapConditionCode(PatientProblem patientProblem)
        {
            return new CodeableConcept
            {
                Text = patientProblem.ICDCodeDescription,
                Coding = new List<Coding>
                {
                    new Coding {
                        System = "http://snomed.info/sct",
                        Code = patientProblem.ICDCode,
                        Display = patientProblem.ICDCodeDescription
                    }
                }
            };
        }
        private string getSystemUrl(PatientProblem patientProblem)
        {
            // var baseURL = _options.Value.EMRBaseURL;
            var baseURL = "https://qa.wmi360.com/EHR/api/main/api/v1";

            if (patientProblem.claimId > 0)
            {
                return $"{baseURL}/encounters/getclaimdiagnosisbyid?ClaimId={patientProblem.claimId}";
            }
            else
            {
                return $"{baseURL}/encounters/getpatientproblembyid?PatientId={patientProblem.PatientId}";
            }
        }
        private CodeableConcept MapClinicalStatus(PatientProblem patientProblem)
        {
            var boolValue = ClinicalStatusMapping.TryGetValue(patientProblem.ProblemStatusName?.ToLower(), out var problemStatusName);
            return new CodeableConcept
            {
                Text = problemStatusName,
                Coding = new List<Coding>
                {
                    new Coding {
                        System = "http://terminology.hl7.org/CodeSystem/condition-clinical",
                        Code = problemStatusName
                       // Display = problemStatusName
                    }
                }
            };
        }

        private List<CodeableConcept> MapCategory(PatientProblem patientProblem)
        {
            var boolValue = CategoryMapping.TryGetValue(patientProblem.claimId > 0, out var problemCategory);

            var coding = new CodeableConcept
            {

                Coding = new List<Coding>
                {
                    new Coding {
                        System = "http://terminology.hl7.org/CodeSystem/condition-category",
                        Code = problemCategory,
                        Display = $"{char.ToUpper(problemCategory[0])}{problemCategory.Substring(1).Replace("-", " ")}"
        }
                }
            };
            var list = new List<CodeableConcept>
            {
                coding
            };

            return list;
        }

        private static readonly Dictionary<string, string> ClinicalStatusMapping = new Dictionary<string, string>
        {

            { "active", Condition.ConditionClinicalStatusCodes.Active.ToString().ToLower() },
            { "resolved", Condition.ConditionClinicalStatusCodes.Resolved.ToString().ToLower() }
        };
        private static readonly Dictionary<bool, string> CategoryMapping = new Dictionary<bool, string>
        {

            { false, "problem-list-item" },
            { true, "encounter-diagnosis"}
        };

    }
   
}
