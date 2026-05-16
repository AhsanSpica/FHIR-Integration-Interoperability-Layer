using AutoMapper;
using Hl7.Fhir.Model;
using Interface.Models.EncounterModels;
using Interface.Models.InterfaceModels;
using static Hl7.Fhir.Model.AllergyIntolerance;

namespace FHIRMappers
{
    public class AllergyIntoleranceFHIRProfile : Profile
    {
        public AllergyIntoleranceFHIRProfile()
        {
            CreateMap<ORMChartAllergyView, AllergyIntolerance>()
                .ForMember(dest => dest.Id , opt => opt.Ignore())
                .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => new List<Identifier> { new Identifier(getSystemUrl(src), src.Id.ToString()) }))
                .ForMember(dest => dest.Patient, opt => opt.MapFrom(src => src.PatientResourceReference))
                .ForMember(dest => dest.Encounter, opt => opt.MapFrom(src => src.EncounterResourceReference))
                .ForMember(dest => dest.ClinicalStatus, opt => opt.MapFrom(src => new CodeableConcept
                {
                    Coding = new List<Coding>
                    {
                    new Coding
                    {
                        System = "http://terminology.hl7.org/CodeSystem/allergyintolerance-clinical",
                        Code = GetClinicalStatusCode(src.Status),
                        Display = GetClinicalStatusDisplay(src.Status)
                    }
                    }
                }))
                .ForMember(dest => dest.VerificationStatus, opt => opt.MapFrom(src => new CodeableConcept
                {
                    Coding = new List<Coding>
                    {
                    new Coding
                    {
                        System = "http://terminology.hl7.org/CodeSystem/allergyintolerance-verification",
                        Code = GetVerificationStatus(src)
                    }
                    }
                }))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => GetCategory(src)))
                .ForMember(dest => dest.Criticality, opt => opt.MapFrom(src => MapCriticality(src.CriticalityName).ToString()))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Description)? new CodeableConcept
                {
                    Text = src.Description,
                    Coding = !string.IsNullOrEmpty(src.RxNorm) ? new List<Coding>
                    {
                    new Coding
                    {
                        System = "http://snomed.info/sct",
                        Code = src.RxNorm
                    }
                    } : null
                }:null))
                .ForMember(dest => dest.Onset, opt => opt.MapFrom(src => ParseFhirDateTime(src.OnsetDate)))
                .ForMember(dest => dest.RecordedDate, opt => opt.MapFrom(src => ParseFhirDateTime(src.AllergyDate)))
                .ForMember(dest => dest.Recorder, opt => opt.MapFrom(src => src.PractitionerResourceReference))

                .ForMember(dest => dest.Reaction, opt => opt.MapFrom(src => getReaction(src)))
               

                .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta
                {
                    LastUpdated = src.UpdatedAt ?? DateTimeOffset.UtcNow,
                    Profile = new List<string> { "http://hl7.org/fhir/us/core/StructureDefinition/us-core-allergyintolerance" }
                }));
        }
        private List<ReactionComponent> getReaction(ORMChartAllergyView src)
        {
            return new List<AllergyIntolerance.ReactionComponent>
                {
                new AllergyIntolerance.ReactionComponent
                {
                    Description = string.IsNullOrEmpty( src.Notes) ? src.Notes : "--",
                    Severity = MapSeverity(src.Severity),
                    Manifestation =
                    new List<CodeableConcept>
                    {
                        new CodeableConcept
                        {
                          Coding =  new List<Coding>
                            {
                                new Coding
                    {
                        System = src.ManifestationSystem,
                        Code = src.ManifestationCode,
                        Display = src.ManifestationDisplay
                    }
                            }
                        }
                    }
                }
                };
        }
        private string getSystemUrl(ORMChartAllergyView src)
        {
            var baseURL = "https://qa.wmi360.com/EHR/api/main/api/v1";
            return $"{baseURL}/dashboard/getpatientallergies?PatientId={src.PatientId}";
        }

        private static string GetClinicalStatusCode(string status)
        {
            return status switch
            {
                "A" => AllergyIntolerance.AllergyIntoleranceClinicalStatusCodes.Active.ToString().ToLower(),
                "I" => AllergyIntolerance.AllergyIntoleranceClinicalStatusCodes.Inactive.ToString().ToLower(),
                _ => "Unknown"
            };
        }
        private static string GetClinicalStatusDisplay(string status)
        {
            return status switch
            {
                "A" => AllergyIntolerance.AllergyIntoleranceClinicalStatusCodes.Active.ToString(),
                "I" => AllergyIntolerance.AllergyIntoleranceClinicalStatusCodes.Inactive.ToString(),
                _ => "Unknown"
            };
        }

        private static AllergyIntolerance.AllergyIntoleranceCriticality? MapCriticality(string criticalityName)
        {
            return criticalityName switch
            {
                "Low Risk" => AllergyIntolerance.AllergyIntoleranceCriticality.Low,
                "High Risk" => AllergyIntolerance.AllergyIntoleranceCriticality.High,
                "Unable to Assess" => AllergyIntolerance.AllergyIntoleranceCriticality.UnableToAssess,
                _ => null
            };
        }

        private static AllergyIntolerance.AllergyIntoleranceSeverity? MapSeverity(string severity)
        {
            return severity switch
            {
                "MILD" => AllergyIntolerance.AllergyIntoleranceSeverity.Mild,
                "MODERATE" => AllergyIntolerance.AllergyIntoleranceSeverity.Moderate,
                "SEVERE" => AllergyIntolerance.AllergyIntoleranceSeverity.Severe,
                _ => null
            };
        }

        private List<AllergyIntoleranceCategory>? GetCategory(ORMChartAllergyView src)
        {
            AllergyIntoleranceCategory? result = src.CategoryCode switch
            {
                "food" => AllergyIntoleranceCategory.Food,
                "environment" => AllergyIntoleranceCategory.Environment,
                "medication" => AllergyIntoleranceCategory.Medication,
                _=> AllergyIntoleranceCategory.Environment
            };

            return new List<AllergyIntoleranceCategory> { result.Value };
        }

        private string GetVerificationStatus(ORMChartAllergyView src)
        {
            if (src.IsDeleted)
            {
                return AllergyIntolerance.AllergyIntoleranceVerificationStatusCodes.Refuted.ToString().ToLower();
            }
            else if (!src.IsActive)
            {
                return AllergyIntolerance.AllergyIntoleranceVerificationStatusCodes.Unconfirmed.ToString().ToLower();
            }
            return AllergyIntolerance.AllergyIntoleranceVerificationStatusCodes.Confirmed.ToString().ToLower();
        }

        private FhirDateTime ParseFhirDateTime(string dateString)
        {
            if (DateTime.TryParse(dateString, out DateTime dateTime))
            {
                return new FhirDateTime(dateTime.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            }
            return null;
        }
    }
  
   
}
