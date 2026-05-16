using AutoMapper;
using Hl7.Fhir.Model;
using Hl7.Fhir.Support;
using Interface.Models.EncounterModels;
using Interface.Models.InterfaceModels;
using Interface.Models.Medication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hl7.Fhir.Model.MedicationRequest;

namespace FHIRMappers
{
    public class MedicationRequestMappingProfile : Profile
    {
        public MedicationRequestMappingProfile()
        {
            CreateMap<ORMChartPrescriptionView, MedicationRequestR4>()
               // .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => new List<Identifier> { new Identifier(getSystemUrl(src), src.Id.ToString()) }))
                .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta
                {
                    LastUpdated = src.UpdatedAt ?? DateTimeOffset.UtcNow
                }))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Archive == "Y" ? MedicationRequest.MedicationrequestStatus.Completed : MedicationRequest.MedicationrequestStatus.Active))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.PatientId.HasValue ? new ResourceReference { Reference = $"Patient/{src.PatientId}" } :null ))
                .ForMember(dest => dest.Encounter, opt => opt.MapFrom(src => src.EncounterId !=null ? new ResourceReference { Reference = $"Encounter/{src.EncounterId}" } : null))
                .ForMember(dest => dest.AuthoredOn, opt => opt.MapFrom(src => src.IssuedDate))
                .ForMember(dest => dest.Medication, opt => opt.MapFrom(src => src.DrugInfo != null ? new ResourceReference { Reference = src.DrugInfo } : null ))
                .ForMember(dest => dest.Requester, opt => opt.MapFrom(src => src.ProviderId != null ? new ResourceReference { Reference = $"Practitioner/{src.ProviderId}" } : null))
                .ForMember(dest => dest.DosageInstruction, opt => opt.MapFrom(src => new List<Dosage>
                {
                    new Dosage
                    {
                        Text = src.SigText !=null ? src.SigText : "N/A"
                    }
                }))
                .ForMember(dest => dest.DispenseRequest, opt => opt.MapFrom(src => new MedicationRequest.DispenseRequestComponent
                {
                    Quantity = new Quantity
                    {
                        Value = ParseRange(src.Quantity),

                    },
                    NumberOfRepeatsAllowed = src.NumOfRefillsAllowed != null ? int.Parse(src.NumOfRefillsAllowed) : (int?)null
                }));

            // .ForMember(dest => dest.ResourceType, opt => opt.Ignore()); 
            // .ForAllOtherMembers(opt => opt.Ignore()); // Ignore unmapped properties
        }
        private string getSystemUrl(ORMChartPrescriptionView src)
        {
            // var baseURL = _options.Value.EMRBaseURL;
            var baseURL = "https://qa.wmi360.com/EHR/api/main/api/v1";

            if (src.EncounterId !=null)
            {
                return $"{baseURL}/encounters/getchartprescriptionview/{src.EncounterId}";
            }
            else
            {
                return $"{baseURL}/encounters/getchartprescriptionview/";
            }
        }
        private decimal? ParseRange(string range)
        {
            if (string.IsNullOrEmpty(range))
            {
                return null;
            }

            if (range.Contains('-'))
            {
                var parts = range.Split('-');
                if (parts.Length == 2 &&
                    decimal.TryParse(parts[0], out var minValue) &&
                    decimal.TryParse(parts[1], out var maxValue))
                {
                    // For simplicity, let's return the average. You can modify this to return min, max, etc.
                    return minValue ;
                }
            }
            else if (decimal.TryParse(range, out var singleValue))
            {
                return singleValue;
            }

            return null;
        }
    }
    public class MedicationRequestFHIRMappingProfile : Profile
    {
        public MedicationRequestFHIRMappingProfile()
        {
            CreateMap<ORMChartPrescriptionView, MedicationRequest>()
               // .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
               .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => new List<Identifier> { new Identifier(getSystemUrl(src), src.Id.ToString()) }))
                .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta
                {
                    LastUpdated = src.UpdatedAt ?? DateTimeOffset.UtcNow
                }))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Archive == "Y" ? MedicationRequest.MedicationrequestStatus.Completed : MedicationRequest.MedicationrequestStatus.Active))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.PatientId.HasValue ? src.PatientResourceReference : null))
                .ForMember(dest => dest.Encounter, opt => opt.MapFrom(src => src.EncounterId != null ? src.EncounterResourceReference : null))
                .ForMember(dest => dest.AuthoredOn, opt => opt.MapFrom(src => src.IssuedDate.ToFhirDate()))
                .ForMember(dest => dest.Intent, opt => opt.MapFrom(src => MedicationRequestIntent.OriginalOrder))
                .ForMember(dest => dest.Medication, opt => opt.MapFrom(src => src.DrugInfo != null ? new ResourceReference { Reference = src.DrugInfo } : null))
                .ForMember(dest => dest.Requester, opt => opt.MapFrom(src => src.ProviderId != null ? src.PractitionerResourceReference : null))
                .ForMember(dest => dest.DosageInstruction, opt => opt.MapFrom(src => new List<Dosage>
                {
                    new Dosage
                    {
                        Text = dosageText(src)
                    }
                }))
                .ForMember(dest => dest.DispenseRequest, opt => opt.MapFrom(src => new MedicationRequest.DispenseRequestComponent
                {
                    Quantity = new Quantity
                    {
                        Value = ParseRange(src.Quantity),

                    },
                    NumberOfRepeatsAllowed = src.NumOfRefillsAllowed != null ? int.Parse(src.NumOfRefillsAllowed) : (int?)null
                }));

            // .ForMember(dest => dest.ResourceType, opt => opt.Ignore()); 
            // .ForAllOtherMembers(opt => opt.Ignore()); // Ignore unmapped properties
        }
        private MedicationRequest.MedicationRequestIntent MapIntent(object src )
        {
             return MedicationRequestIntent.OriginalOrder; 
        }
        private string dosageText(ORMChartPrescriptionView src)
        {
            var temp = src.SigText.Trim() != null ? src.SigText.Trim() : "";
            temp = temp.Trim() != "" ? temp.Trim() : src.DosageFrequency.Trim();
            temp = temp.Trim() != "" ? temp.Trim() : "-";
             return temp != "" ? temp : "no SigText or DosageFreq";
        }
        private string getSystemUrl(ORMChartPrescriptionView src)
        {
            // var baseURL = _options.Value.EMRBaseURL;
            var baseURL = "https://qa.wmi360.com/EHR/api/main/api/v1";

            if (src.EncounterId != null)
            {
                return $"{baseURL}/encounters/getchartprescriptionview/{src.EncounterId}";
            }
            else
            {
                return $"{baseURL}/encounters/getchartprescriptionview/";
            }
        }
        private decimal? ParseRange(string range)
        {
            if (string.IsNullOrEmpty(range))
            {
                return null;
            }

            if (range.Contains('-'))
            {
                var parts = range.Split('-');
                if (parts.Length == 2 &&
                    decimal.TryParse(parts[0], out var minValue) &&
                    decimal.TryParse(parts[1], out var maxValue))
                {
                    // For simplicity, let's return the average. You can modify this to return min, max, etc.
                    return minValue;
                }
            }
            else if (decimal.TryParse(range, out var singleValue))
            {
                return singleValue;
            }

            return null;
        }
    }
}
