using AutoMapper;
using Hl7.Fhir.Model;
using Hl7.Fhir.Support;
using Interface.Models.EncounterModels;
using Interface.Models.InterfaceModels;

namespace FHIRMappers
{
    public class EncounterMappingFHIRProfile : Profile
    {
        public EncounterMappingFHIRProfile()
        {
            //}
            //public EncounterMapping(IProviderService providerService, LookUpScoped lookUpScoped)
            //{
            //    _providerService = providerService;
            //    _lookUpScoped = lookUpScoped;

            CreateMap<EncounterInfoDto, Encounter>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => new List<Identifier> { new Identifier(getSystemUrl(src), src.Id.ToString()) }))
           .ForMember(dest => dest.Appointment, opt => opt.MapFrom(src => getAppointmentReference(src)))
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MapStatus(src)))
                 //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.))
               //  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))

                // .ForMember(dest => dest.Class, opt => opt.Ignore())

                 .ForMember(dest => dest.Class, opt => opt.MapFrom(src => new Coding
                 {
                     Code = "IMP", // Assuming "AMB" for ambulatory
                     System = "http://terminology.hl7.org/CodeSystem/v3-ActCode",
                     Display = "InPatient"
                 }))
                // .ForMember(dest => dest.Type, opt => opt.MapFrom(src => ))
                 .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.PatientReference ))

                 .ForMember(dest => dest.Period, opt => opt.MapFrom(src => new Period
                 {
                     Start = src.EncounterDateTime.Value.ToFhirDateTime()
                 }))
                 .ForMember(dest => dest.ReasonCode, opt => opt.MapFrom(src => new List<CodeableConcept>
                 {
                new CodeableConcept
                {
                    Text = src.ReasonName,
                    Coding = new List<Coding>
                    {
                        new Coding
                        {
                            Code =  src.Reason.HasValue? src.Reason.ToString() : "0", // Assuming Reason is the FHIR code for reason
                            System = "http://terminology.hl7.org/CodeSystem/condition-code", // Define your code system URL
                            Display = string.IsNullOrWhiteSpace(src.ReasonString) ? src.ReasonString : "0",
                        }
                    }
                }
                 }))
                 .ForMember(dest => dest.Location, opt => opt.MapFrom(src => getLocationComponent(src)))
                 .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta
                 {
                     LastUpdated = src.UpdatedAt ?? DateTimeOffset.UtcNow // Assuming UpdatedAt is the last updated time
                 }));
        }
       
        private Encounter.EncounterStatus MapStatus(EncounterInfoDto src)
        {
                if (src.IsDeleted.Value)
                {
                    return Encounter.EncounterStatus.Cancelled;
                }

                if (src.EncounterDateTime.HasValue)
                {
                    if (src.EncounterDateTime.Value > DateTimeOffset.UtcNow)
                    {
                        return Encounter.EncounterStatus.Planned;
                    }
                    else if (src.EncounterDateTime.Value <= DateTimeOffset.UtcNow && src.EncounterDateTime.Value.AddMinutes(30) > DateTimeOffset.UtcNow)
                    {
                        return Encounter.EncounterStatus.InProgress;
                    }
                    else if (src.EncounterDateTime.Value <= DateTimeOffset.UtcNow)
                    {
                        return Encounter.EncounterStatus.Finished;
                    }
                }
                return Encounter.EncounterStatus.Unknown;

        }

        private List<ResourceReference> getAppointmentReference(EncounterInfoDto src)
        {
            var returner = new List<ResourceReference>
            {

                src.AppointmentReference
            };
            return returner;
        }
        private List<Encounter.LocationComponent> getLocationComponent(EncounterInfoDto src)
        {

            //
            var returner = new List<Encounter.LocationComponent>
            {

                new Encounter.LocationComponent {  Location =  src.LocationReference  }
            };
            return returner;
        }

        private string getSystemUrl(EncounterInfoDto encounterInfoDto)
        {
            // var baseURL = _options.Value.EMRBaseURL;
            var baseURL = "https://qa.wmi360.com/EHR/api/main/api/v1";

            return $"{baseURL}/encounters/getencounterbyid/{encounterInfoDto.Id}";
        }
    }
    public class EncounterMappingProfile : Profile
    {
        public EncounterMappingProfile()
        {
            CreateMap<EncounterInfoDto, EncounterR4>()
                .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => new List<Identifier>
                {
                new Identifier { System = getSystemUrl(src), Value = src.Id.ToString() }
                }))
                .ForMember(dest => dest.Appointment, opt => opt.MapFrom(src => getAppointmentReference(src)))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               // .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MapStatus(src)))
               .ForMember(dest => dest.Class, opt => opt.MapFrom(src => new Coding
               {
                   Code = "IMP", // Assuming "AMB" for ambulatory
                   System = "http://terminology.hl7.org/CodeSystem/v3-ActCode",
                   Display = "inpatient encounter"
               }))
               // .ForMember(dest => dest.Type, opt => opt.MapFrom(src => ))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => new ResourceReference { Reference = $"Patient/{src.PatientId}" }))
                .ForMember(dest => dest.Participant, opt => opt.Ignore()) // Ignoring Participant mapping
                .ForMember(dest => dest.Period, opt => opt.MapFrom(src => new Period
                {
                    Start = src.EncounterDateTime.ToFhirDateTime()
                }))
                .ForMember(dest => dest.ReasonCode, opt => opt.MapFrom(src => new List<CodeableConcept>
                {
                new CodeableConcept
                {
                    Text = src.ReasonString,
                    Coding = new List<Coding>
                    {
                        new Coding
                        {
                            Code = src.Reason.HasValue ? src.Reason.Value.ToString() : null,
                            System = "http://terminology.hl7.org/CodeSystem/condition-code",
                            Display = src.ReasonString
                        }
                    }
                }
                }))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new List<Encounter.LocationComponent>
                {
                new Encounter.LocationComponent
                {
                    Location = new ResourceReference { Reference = $"Location/{src.LocationId}" }
                }
                }))
                .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta
                {
                    LastUpdated = src.UpdatedAt.HasValue ? src.UpdatedAt.Value : DateTimeOffset.UtcNow
                }));
        }
        private Encounter.EncounterStatus MapStatus(EncounterInfoDto src)
        {
            if (src.IsDeleted.Value)
            {
                return Encounter.EncounterStatus.Cancelled;
            }

            if (src.EncounterDateTime.HasValue)
            {
                if (src.EncounterDateTime.Value > DateTimeOffset.UtcNow)
                {
                    return Encounter.EncounterStatus.Planned;
                }
                else if (src.EncounterDateTime.Value <= DateTimeOffset.UtcNow && src.EncounterDateTime.Value.AddMinutes(30) > DateTimeOffset.UtcNow)
                {
                    return Encounter.EncounterStatus.InProgress;
                }
                else if (src.EncounterDateTime.Value <= DateTimeOffset.UtcNow)
                {
                    return Encounter.EncounterStatus.Finished;
                }
            }
            return Encounter.EncounterStatus.Unknown;
        }

        private List<ResourceReference> getAppointmentReference(EncounterInfoDto src)
        {
            return new List<ResourceReference>
        {
            new ResourceReference { Reference = $"Appointment/{src.AppointmentId}" }
        };
        }

        private string getSystemUrl(EncounterInfoDto encounterInfoDto)
        {
            var baseURL = "https://qa.wmi360.com/EHR/api/main/api/v1";
            return $"{baseURL}/encounters/getencounterbyid/{encounterInfoDto.Id}";
        }
      
    }


}
