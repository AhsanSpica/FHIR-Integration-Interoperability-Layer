using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Hl7.Fhir.Model;
using Hl7.Fhir.Support;
using Interface.Models.EncounterModels;
using Interface.Models.ImmunizationModels;
using Interface.Models.InterfaceModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hl7.Fhir.Model.Immunization;

namespace FHIRMappers
{
   

    public class ImmunizationFHIRMappingProfile : Profile
    {
        //VaccineRefusalReason
        //::Site , Route and Refuse Reason handled at the mapper layer
        public ImmunizationFHIRMappingProfile()

        {
            CreateMap<ImmunizationDTO, Immunization>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))

                .ForMember(dest => dest.StatusReason, opt => opt.Ignore())
                .ForMember(dest => dest.Site, opt => opt.Ignore())
                .ForMember(dest => dest.Route, opt => opt.Ignore())
                .ForMember(dest => dest.FundingSource, opt => opt.Ignore())
                .ForMember(dest => dest.ProgramEligibility, opt => opt.Ignore())

                .ForMember(dest => dest.LotNumber, opt => opt.MapFrom(src => src.LotNumber))
                  .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta
                  {
                      LastUpdated = src.UpdatedAt ?? DateTimeOffset.UtcNow // Assuming UpdatedAt is the last updated time
                  }))
               .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => new List<Identifier> { new Identifier(getSystemUrl(src), src.Id.ToString()) }))
                .ForMember(dest => dest.VaccineCode, opt => opt.MapFrom(src => new CodeableConcept
                {
                    Coding = new List<Coding>
                    {
                    new Coding
                    {
                        Code = src.CVXCode.HasValue ? src.CVXCode.Value.ToString() : "-",
                        Display = string.IsNullOrEmpty(src.VaccineName) ? "-" : src.VaccineName ,
                        System = "http://hl7.org/fhir/sid/cvx"
                    }
                    },
                    Text = string.IsNullOrEmpty(src.VaccineName) ? "-" : src.VaccineName,
                }))
                 .ForMember(dest => dest.Recorded, opt => opt.MapFrom(src => new FhirDateTime(src.CreatedAt.HasValue ? src.CreatedAt.Value : new DateTimeOffset())))
                .ForMember(dest => dest.PrimarySource, opt => opt.MapFrom(src => src.isPrimary))
                .ForMember(dest => dest.Occurrence, opt => opt.MapFrom(src => getAdministrateredDate(src)))

                 .ForMember(dest => dest.Status, opt => opt.MapFrom(src => getStatus(src)))
                //status is ignore since the FHIR Serializer is not acepting the fhir code value of 'not-done'
                //https://hl7.org/fhir/R4/valueset-immunization-status.html

                //.ForMember(dest => dest.Status, opt => opt.Ignore())
               // .ForMember(dest => dest.Encounter, opt => opt.MapFrom(src => src.EncounterReference));
                .ForMember(dest => dest.Patient, opt => opt.MapFrom(src => src.PatientReference))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.LocationReference));

        }
        private FhirDateTime getAdministrateredDate(ImmunizationDTO src)
        {
            var result = src.AdministeredDate.HasValue ? src.AdministeredDate : new DateTimeOffset();
            result = result.HasValue ? result : new DateTime();
            return new FhirDateTime(result.Value);

        }
        private ImmunizationStatusCodes getStatus(ImmunizationDTO src)
        {

             if (src.RefuseReason > 0)
            {
               return ImmunizationStatusCodes.NotDone;

            }
            else if (src.AdministeredDate.HasValue)
            {
                return ImmunizationStatusCodes.Completed;
            }

            else
            {
                return ImmunizationStatusCodes.EnteredInError;
            }
            
        }
        private string getSystemUrl(ImmunizationDTO encounterInfoDto)
        {
            // var baseURL = _options.Value.EMRBaseURL;
            var baseURL = "https://qa.wmi360.com/EHR/api/main/api/v1";

            return $"{baseURL}/encounters/getallimmunization?PatientID={encounterInfoDto.PatientId}";
        }
    }
}
