using AutoMapper;
using Hl7.Fhir.Model;
using Hl7.Fhir.Support;
using Interface.Models.EncounterModels;
using Interface.Models.InterfaceModels;
using Interface.Models.Medication;
using SemanticVersioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIRMappers
{
    public class MedicationStatementFHIRMappingProfile : Profile
    {
        public MedicationStatementFHIRMappingProfile()
        {
            CreateMap<ORMChartPrescriptionView, MedicationStatement>()
              //  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
              .ForMember(dest => dest.Id, opt => Guid.NewGuid())
                 .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => new List<Identifier> { new Identifier(getSystemUrl(src), src.Id.ToString()) }))
                .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta
                {
                    LastUpdated = src.UpdatedAt ?? DateTimeOffset.UtcNow
                }))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Archive == "Y" ? MedicationStatement.MedicationStatusCodes.NotTaken : MedicationStatement.MedicationStatusCodes.Active))
                .ForMember(dest => dest.Medication, opt => opt.MapFrom(src => src.DrugInfo != null ? new ResourceReference { Reference = src.DrugInfo } : null))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.PatientId.HasValue ? src.PatientResourceReference : null))
                .ForMember(dest => dest.Context, opt => opt.MapFrom(src => src.EncounterId != null ? src.EncounterResourceReference : new ResourceReference { Reference = $"No EncounterId In Record" }))
                .ForMember(dest => dest.Effective, opt => opt.MapFrom(src => MapPeriod( src.StartDate,src.EndDate ) ) )
                .ForMember(dest => dest.DateAsserted, opt => opt.MapFrom(src => src.IssuedDate.ToFhirDate()))
                .ForMember(dest => dest.InformationSource, opt => opt.MapFrom(src => src.ProviderId != null ? src.PractitionerResourceReference : new ResourceReference { Reference = $"No ProviderId In Record" }))
                .ForMember(dest => dest.Dosage, opt => opt.MapFrom(src => new List<Dosage>
                {
                new Dosage
                {
                    Text = dosageText(src),
                    AdditionalInstruction = new List<CodeableConcept> { new CodeableConcept { Text=  string.IsNullOrEmpty(src.DosageFrequency) ? "N/A" : src.DosageFrequency   } },
                     DoseAndRate = mapDose(src)
                }
                }));

            // .ForMember(dest => dest.take, opt => opt.MapFrom(src => src.archive == "Y" ? MedicationStatement.MedicationStatementTaken.NotTaken : MedicationStatement.MedicationStatementTaken.Taken))
            //  .ForAllOtherMembers(opt => opt.Ignore()); 
        }
        private string dosageText(ORMChartPrescriptionView src)
        {
            var temp = src.SigText.Trim() != null ? src.SigText.Trim() : "no SigText or DosageFreq";
            temp = temp.Trim() != "" ? temp.Trim() : src.DosageFrequency.Trim();
            temp = temp.Trim() != "" ? temp.Trim() : "no SigText or DosageFreq";
            return temp != "" ? temp : "no SigText or DosageFreq";
        }
        private List<Dosage.DoseAndRateComponent> mapDose(ORMChartPrescriptionView src)
        {

            return new List<Dosage.DoseAndRateComponent>
           {
               new Dosage.DoseAndRateComponent
               {
                    Dose = new Quantity
                    {
                         Value = ParseRange(src.Quantity),
                    }
               }
           };
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
        private static Period MapPeriod(DateTimeOffset? fromDate, DateTimeOffset? toDate)
        {
            return new Period
            {
                Start = fromDate.HasValue ? fromDate.ToFhirDateTime().ToString() : null,
                End = toDate.HasValue ? toDate.ToFhirDateTime().ToString() : null
            };
        }
    }
    public class MedicationStatementMappingProfile : Profile
    {
        public MedicationStatementMappingProfile()
        {
            CreateMap<ORMChartPrescriptionView, MedicationStatementR4>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                 .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => new List<Identifier> { new Identifier(getSystemUrl(src), src.Id.ToString()) }))
                .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta
                {
                    LastUpdated = src.UpdatedAt ?? DateTimeOffset.UtcNow
                }))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Archive == "Y" ? MedicationStatement.MedicationStatusCodes.NotTaken : MedicationStatement.MedicationStatusCodes.Active))
                .ForMember(dest => dest.Medication, opt => opt.MapFrom(src =>  new ResourceReference { Reference = string.IsNullOrEmpty(src.DrugInfo.Trim())? src.DrugInfo.Trim() :"N/A" } ))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.PatientId.HasValue ? new ResourceReference { Reference = $"Patient/{src.PatientId}" }: null))
                .ForMember(dest => dest.Context, opt => opt.MapFrom(src => src.EncounterId != null ? new ResourceReference { Reference = $"Encounter/{src.EncounterId}" } : new ResourceReference { Reference = $"No EncounterId In Record" }))
                .ForMember(dest => dest.EffectivePeriod, opt => opt.MapFrom(src => MapPeriod(src.StartDate, src.EndDate)))
                .ForMember(dest => dest.DateAsserted, opt => opt.MapFrom(src => src.IssuedDate))
                .ForMember(dest => dest.InformationSource, opt => opt.MapFrom(src => src.ProviderId != null ? new ResourceReference { Reference = $"Practitioner/{src.ProviderId}" } : new ResourceReference { Reference = $"No ProviderId In Record" }))
                .ForMember(dest => dest.Dosage, opt => opt.MapFrom(src => new List<Dosage>
                {
                new Dosage
                {
                    Text = src.SigText!=null ? src.SigText : "-",
                    AdditionalInstruction = new List<CodeableConcept> { new CodeableConcept { Text=  src.DosageFrequency } },
                     DoseAndRate = mapDose(src)
                }
                }));

            // .ForMember(dest => dest.take, opt => opt.MapFrom(src => src.archive == "Y" ? MedicationStatement.MedicationStatementTaken.NotTaken : MedicationStatement.MedicationStatementTaken.Taken))
            //  .ForAllOtherMembers(opt => opt.Ignore()); 
        }
        private static Period MapPeriod(DateTimeOffset? fromDate, DateTimeOffset? toDate)
        {
            return new Period
            {
                Start = fromDate.HasValue ? fromDate.Value.ToString("yyyy-MM-ddTHH:mm:sszzz") : null,
                End = toDate.HasValue ? toDate.Value.ToString("yyyy-MM-ddTHH:mm:sszzz") : null
            };
        }
        private List<Dosage.DoseAndRateComponent> mapDose(ORMChartPrescriptionView src)
        {

            return new List<Dosage.DoseAndRateComponent>
           {
               new Dosage.DoseAndRateComponent
               {
                    Dose = new Quantity
                    {
                         Value = ParseRange(src.Quantity),
                    }
               }
           };
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
    } 
}
