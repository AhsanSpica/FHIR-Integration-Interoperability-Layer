using AutoMapper;
using Hl7.Fhir.Model;
using Interface.Models.EncounterModels;
using Interface.Models.InterfaceModels;
using Interface.Models.Patients;

namespace FHIRMappers
{
    public class CareTeamMapperProfile : Profile
    {

        public CareTeamMapperProfile()
        { 
            CreateMap<PatientCareTeam, CareTeamR4>()
                 .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta
                 {
                     LastUpdated = src.UpdatedAt ?? DateTimeOffset.UtcNow // Assuming UpdatedAt is the last updated time
                 }))
                 .ForMember(dest => dest.Id, opt => opt.Ignore())
          .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => new ResourceReference($"Patient/{src.PatientId}")))
          .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => new List<Identifier> { new Identifier(getSystemUrl(src), src.Id.ToString()) }))
          .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
          .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MapStatus(src.Status)))
          .ForMember(dest => dest.Participant, opt => opt.MapFrom(src => src.PatientCareTeamMembers));

            CreateMap<PatientCareTeamMember, CareTeam.ParticipantComponent>()
               // .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.RelationWithPatient))
                .ForMember(dest => dest.Member, opt => opt.MapFrom(src => new ResourceReference($"Practitioner/{src.ProviderReferenceId}")))
                .ForMember(dest => dest.Extension, opt => opt.MapFrom(src => MapSpecialtyExtension(src)));
                

            //CreateMap<int?, CareTeam.CareTeamStatus?>()
            //    .ConvertUsing(MapStatus);
        }
        private string getSystemUrl(PatientCareTeam patientCareTeam)
        {
            // var baseURL = _options.Value.EMRBaseURL;
            var baseURL = "https://qa.wmi360.com/EHR/api/main/api/v1";
 
                return $"{baseURL}/patientcareteam/getpatientteambyid?careTeamId={patientCareTeam.Id}&patientId=22{patientCareTeam.PatientId}";
         }
        private CareTeam.CareTeamStatus? MapStatus(int? status)
        {
            // Map your status values here
            // Example mapping: 0 -> CareTeam.CareTeamStatus.Active
            switch (status)
            {
                case 0:
                    return CareTeam.CareTeamStatus.Active;
                default:
                    return null;
            }
        }

        private List<Extension> MapSpecialtyExtension(PatientCareTeamMember src)
        {
            var extensions = new List<Extension>();

            // Map your specialty properties to extensions here
            // Example:
            if (!string.IsNullOrEmpty(src.SpecialtyName))
            {
                extensions.Add(new Extension
                {
                    Url = "http://example.com/fhir/extensions#specialty",
                    Value = new CodeableConcept
                    {
                        Text = src.SpecialtyName
                    }
                });
            }

            return extensions;
        }
    }
    public class CareTeamFHIRMapperProfile : Profile
    {

        public CareTeamFHIRMapperProfile()
        {
            CreateMap<PatientCareTeam, CareTeam>()
                 .ForMember(dest => dest.Id, opt => opt.Ignore())
                 .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta
                 {
                     LastUpdated = src.UpdatedAt ?? DateTimeOffset.UtcNow // Assuming UpdatedAt is the last updated time
                 }))
          .ForMember(dest => dest.Subject, opt => opt.MapFrom( src => new ResourceReference($"Patient/{src.PatientId}") ))
          .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => new List<Identifier> { new Identifier(getSystemUrl(src), src.Id.ToString()) }))
          .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
          .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MapStatus(src.Status)))
          .ForMember(dest => dest.Participant, opt => opt.MapFrom(src => src.PatientCareTeamMembers));

            CreateMap<PatientCareTeamMember, CareTeam.ParticipantComponent>()
                // .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.RelationWithPatient))
                .ForMember(dest => dest.Member, opt => opt.MapFrom(src => new ResourceReference($"Practitioner/{src.ProviderReferenceId}")))
                .ForMember(dest => dest.Extension, opt => opt.MapFrom(src => MapSpecialtyExtension(src)));


            //CreateMap<int?, CareTeam.CareTeamStatus?>()
            //    .ConvertUsing(MapStatus);
        }
        private string getSystemUrl(PatientCareTeam patientCareTeam)
        {
            // var baseURL = _options.Value.EMRBaseURL;
            var baseURL = "https://qa.wmi360.com/EHR/api/main/api/v1";

            return $"{baseURL}/patientcareteam/getpatientteambyid?careTeamId={patientCareTeam.Id}&patientId=22{patientCareTeam.PatientId}";
        }
        private CareTeam.CareTeamStatus? MapStatus(int? status)
        {
            // Map your status values here
            // Example mapping: 0 -> CareTeam.CareTeamStatus.Active
            switch (status)
            {
                case 0:
                    return CareTeam.CareTeamStatus.Active;
                default:
                    return null;
            }
        }

        private List<Extension> MapSpecialtyExtension(PatientCareTeamMember src)
        {
            var extensions = new List<Extension>();

            // Map your specialty properties to extensions here
            // Example:
            if (!string.IsNullOrEmpty(src.SpecialtyName))
            {
                extensions.Add(new Extension
                {
                    Url = "http://example.com/fhir/extensions#specialty",
                    Value = new CodeableConcept
                    {
                        Text = src.SpecialtyName
                    }
                });
            }

            return extensions;
        }
    }
}
