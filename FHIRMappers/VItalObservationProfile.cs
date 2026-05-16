
using AutoMapper;
using Hl7.Fhir.ElementModel.Types;
using Hl7.Fhir.Model;
using Interface.Models.EncounterModels;

namespace FHIRMappers
{
    public class VitalObservationProfile : Profile
    {
        public VitalObservationProfile()
        {
            CreateMap<EncounterPatientVitalDto, Observation>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => new Meta
            {
                Profile = new List<string> { "http://hl7.org/fhir/us/core/StructureDefinition/us-core-vital-signs" },
                LastUpdated = src.UpdatedAt ?? DateTimeOffset.UtcNow // Assuming UpdatedAt is the last updated time
            }))
            .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => new List<Identifier> { new Identifier(GetSystemUrl(src), src.Id.ToString()),
                 new Identifier ("http://hl7.org/fhir/us/core/StructureDefinition/us-core-vital-signs",src.Id.ToString() ) }))


            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => GetCategory()))
            .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.PatientReference))
            .ForMember(dest => dest.Encounter, opt => opt.MapFrom(src => src.EncounterReference))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => new CodeableConcept
            {
                Text = src.VitalName,
                Coding = new List<Coding>
                {
                    new Coding
                    {
                        System = "http://loinc.org",
                        Code = src.LOINC
                    }
                }
            }))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetStatus(src)))
            //ignoring potentionally decimal to datatype error
            .ForMember(dest => dest.Value, opt => opt.Ignore())
            .ForMember(dest => dest.Component, opt => opt.Ignore())

            //.ForMember(dest => dest.Performer, opt => opt.MapFrom(src => src.PractitionerReference.ToList()))

            ;
        }

        private static ObservationStatus GetStatus(EncounterPatientVitalDto src)
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
        private static List<CodeableConcept> GetCategory()
        {
            return new List<CodeableConcept> { new CodeableConcept {
                Coding = new List<Coding> {
                    new Coding {
                        System = "http://terminology.hl7.org/CodeSystem/observation-category", Code = "vital-signs", Display = "Vital Signs" } } } };
        }

        private static string GetSystemUrl(EncounterPatientVitalDto encounterInfoDto)
        {
            // var baseURL = _options.Value.EMRBaseURL;
            var baseURL = "https://qa.wmi360.com/EHR/api/main/api/v1";

            return $"{baseURL}dashboard/getpatientvitals?PatientId={encounterInfoDto.EncounterId}";
        }

        private List<Observation.ComponentComponent> getComponent(EncounterPatientVitalDto src)
        {
            return new List<Observation.ComponentComponent> {
                new Observation.ComponentComponent {
                    Code = new CodeableConcept("http://loinc.org", src.LOINC, src.VitalName),
                Value =  getQuantity(src),
                }
                ,
            new Observation.ComponentComponent{
            Code = new CodeableConcept("http://loinc.org", src.LOINC,src.VitalName),
            Value = getRange(src)}
            };
        }
        private Hl7.Fhir.Model.Quantity getQuantity(EncounterPatientVitalDto encounterInfoDto)
        {
            var quantity = new Hl7.Fhir.Model.Quantity
            {
                System = "http://unitsofmeasure.org",
                // Code =  encounterInfoDto.
                Unit = encounterInfoDto.Unit,
                Value = encounterInfoDto.Value.Value,
            };

            Hl7.Fhir.Model.DataType dataType = quantity;
            return quantity;
        }

        private Hl7.Fhir.Model.Range getRange(EncounterPatientVitalDto encounterInfoDto)
        {
            var quantity = new Hl7.Fhir.Model.Range
            {
                High = new Hl7.Fhir.Model.Quantity { System = "http://unitsofmeasure.org", Unit = encounterInfoDto.Unit, Value = encounterInfoDto.MaxRanage.Value },
                Low = new Hl7.Fhir.Model.Quantity { System = "http://unitsofmeasure.org", Unit = encounterInfoDto.Unit, Value = encounterInfoDto.MinRange.Value }
            };

            Hl7.Fhir.Model.DataType dataType = quantity;
            return quantity;
        }

    }



}
