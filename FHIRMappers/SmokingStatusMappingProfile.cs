using AutoMapper;
using Hl7.Fhir.Model;
using Interface.Models.EncounterModels;
using Interface.Models.InterfaceModels;
using Interface.Models.Medication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIRMappers
{
    public class SmokingStatusFHIRMappingProfile : Profile
    {
        public SmokingStatusFHIRMappingProfile()
        {
            CreateMap<SmokingStatusDTO, Observation>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => getCategory()))
                .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => new List<Identifier> { new Identifier(getSystemUrl(src), src.Id.ToString()),
                 new Identifier ("http://hl7.org/fhir/us/core/StructureDefinition/us-core-smokingstatus",src.Id.ToString() ) }))

                  .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta
                  {
                      LastUpdated = src.UpdatedAt ?? DateTimeOffset.UtcNow,
                      Profile = new List<string> { "http://hl7.org/fhir/StructureDefinition/us-core-smokingstatus" }
                  }))
                .ForMember(dest => dest.Encounter, opt => opt.MapFrom(src => src.EncounterReference))

                //.ForMember(dest => dest.Extension, opt => opt.MapFrom(src => new List<Extension>
                //        {
                //            new Extension
                //            {

                //                Url = "http://hl7.org/fhir/StructureDefinition/us-core-smokingstatus",
                //                Value = new FhirString(src.Id.ToString())
                //            }
                //        }))

                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.PatientReference))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => getStatus(src)))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => new CodeableConcept
                {
                    Coding = new List<Coding>
                    {
                    new Coding
                    {
                        System = "http://loinc.org",
                        Code = MapTobaccoStatusCode(src.StatusName),
                        Display = MapTobaccoStatus(src.StatusName)
                    }
                    },
                    Text = MapTobaccoStatus(src.StatusName)
                }))

                .ForMember(dest => dest.Effective, opt => opt.MapFrom(src => src.StartDate.HasValue ? new FhirDateTime(src.StartDate.Value) : null))

                .ForMember(dest => dest.Issued, opt => opt.MapFrom(src => src.StartDate.HasValue ? src.StartDate.Value : new DateTimeOffset()))

                //:: null issue this field is handled in the business Mapper layer
                
                //.ForMember(dest => dest.Value, opt => opt.MapFrom(src => new CodeableConcept
                //{
                //    Coding = new List<Coding>
                //    {
                //    new Coding
                //    {
                //        System = "http://snomed.info/sct",
                //        Code =  src.FHIRStatusCode ,
                //        Display = src.UseName
                //    }
                //    },
                //    Text = src.UseName
                //}))

                .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta
                {
                    LastUpdated = src.UpdatedAt ?? src.CreatedAt,
                    Source = src.CreatedBy ?? src.UpdatedBy
                }));
                
               // .ReverseMap();
               
        }
        private string getSystemUrl(SmokingStatusDTO src)
        {
            // var baseURL = _options.Value.EMRBaseURL;
            var baseURL = "https://qa.wmi360.com/EHR/api/main/api/v1";

            if (src.EncounterId > 0)
            {
                return $"{baseURL}/encounters/getencounterwrapperbyid/{src.EncounterId}?IncludeSdoh=true&IncludePhq9=true";
            }
            else
            {
                return $"{baseURL}/encounters/getencounterwrapperbyid/IncludeSdoh=true&IncludePhq9=true";

            }
        }
        private List<CodeableConcept> getCategory()
        {
            return new List<CodeableConcept> { new CodeableConcept {
                Coding = new List<Coding> {
                    new Coding {
                        System = "http://terminology.hl7.org/CodeSystem/observation-category", Code = "social-history", Display = "Social History" } } } };
        }
        private ObservationStatus getStatus(SmokingStatusDTO src)
        {
            var result = new ObservationStatus();

            if (src.ObservationStatus.ToLower().Equals("final"))
            { result = ObservationStatus.Final; }

            else if (src.ObservationStatus.ToLower().Equals("registered"))
            { result = ObservationStatus.Registered; }

            else if (src.ObservationStatus.ToLower().Equals("amended"))
            { result = ObservationStatus.Amended; }

            return result;

        }
        private static string MapTobaccoStatus(string? tobaccoType)
        {
            return tobaccoType switch
            {
                "Smoking Status" => "Tobacco smoking status",
                "Smokeless Status" => "Smokeless tobacco status",
                "Vaping" => "Vaping status",

                _ => "unknown"
            };
        }
        private static string MapTobaccoStatusCode(string? tobaccoType)
        {
            return tobaccoType switch
            {
                "Smoking Status" => "72166-2",
                "Smokeless Status" => "88031-0",
                "Vaping" => "Vaping status",

                _ => "unknown"
            };
        }
    }
    //public class SmokingStatusMappingProfile : Profile
    //{
    //    public SmokingStatusMappingProfile()
    //    {
    //        CreateMap<SmokingStatusDTO, SmokingStatusCore>()
    //            .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => new List<Identifier> { new Identifier(getSystemUrl(src), src.Id.ToString()) }))
    //              .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta
    //              {
    //                  LastUpdated = src.UpdatedAt ?? DateTimeOffset.UtcNow,
    //                  Profile = new List<string> { "http://hl7.org/fhir/StructureDefinition/us-core-smokingstatus" }
    //          }))
    //            .ForMember(dest => dest.Encounter, opt => opt.MapFrom(src => new ResourceReference($"Encounter/{src.EncounterId}")))
    //            .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => new ResourceReference($"Patient/{src.PatientId}")))
    //            .ForMember(dest => dest.Status, opt => opt.Ignore())
    //            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => new CodeableConcept
    //            {
    //                Coding = new List<Coding>
    //                {
    //                new Coding
    //                {
    //                    System = "http://loinc.org",
    //                    Code = MapTobaccoStatusCode(src.StatusName),
    //                    Display = MapTobaccoStatus(src.StatusName)
    //                }
    //                },
    //                Text = MapTobaccoStatus(src.StatusName)
    //            }))
    //            .ForMember(dest => dest.Effective, opt => opt.MapFrom(src => src.StartDate.HasValue ? new FhirDateTime(src.StartDate.Value): null))
    //            .ForMember(dest => dest.Issued, opt => opt.MapFrom(src => src.StartDate.HasValue ? new FhirDateTime(src.StartDate.Value) : null))
    //            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => new CodeableConcept
    //            {
    //                Coding = new List<Coding>
    //                {
    //                new Coding
    //                {
    //                    System = "http://snomed.info/sct",
    //                    Code =  src.FHIRStatusCode ,
    //                    Display = src.UseName
    //                }
    //                },
    //                Text = src.UseName
    //            }))
    //            .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta
    //            {
    //                LastUpdated = src.UpdatedAt ?? src.CreatedAt,
    //                Source = src.CreatedBy ?? src.UpdatedBy
    //            }))
    //           ;
    //    }
    //    private string getSystemUrl(SmokingStatusDTO src)
    //    {
    //        // var baseURL = _options.Value.EMRBaseURL;
    //        var baseURL = "https://qa.wmi360.com/EHR/api/main/api/v1";

    //        if (src.EncounterId > 0)
    //        {
    //            return $"{baseURL}/encounters/getencounterwrapperbyid/{src.EncounterId}?IncludeMedications=true&IncludeAllergies=true&IncludeVitals=true&IncludeExamSubmissions=true&IncludeAssessments=true&IncludePlans=false&IncludeGoals=true&IncludeOrders=true&IncludeSocialHistory=true&IncludeSdoh=true&IncludePhq9=true&IncludeFallRisk=true&IncludeOtherDetails=true&IncludeAddendumDetails=true";
    //        }
    //        else
    //        {
    //            return $"{baseURL}/encounters/getencounterwrapperbyid/IncludeMedications=true&IncludeAllergies=true&IncludeVitals=true&IncludeExamSubmissions=true&IncludeAssessments=true&IncludePlans=false&IncludeGoals=true&IncludeOrders=true&IncludeSocialHistory=true&IncludeSdoh=true&IncludePhq9=true&IncludeFallRisk=true&IncludeOtherDetails=true&IncludeAddendumDetails=true";

    //        }
    //    }
      

    //    private static string MapTobaccoStatus(string? tobaccoType)
    //    {
    //        return tobaccoType switch
    //        {
    //            "Smoking Status" => "Tobacco smoking status NHIS",
    //            "Smokeless Status" => "Smokeless tobacco status NHIS",
    //            "Vaping" => "Vaping status NHIS",
                
    //            _ => "unknown"
    //        };
    //    }
    //    private static string MapTobaccoStatusCode(string? tobaccoType)
    //    {
    //        return tobaccoType switch
    //        {
    //            "Smoking Status" => "72166-2",
    //            "Smokeless Status" => "88031-0",
    //            "Vaping" => "Vaping status NHIS",

    //            _ => "unknown"
    //        };
    //    }
    //}
}

