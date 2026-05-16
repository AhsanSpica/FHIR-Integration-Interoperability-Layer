using AutoMapper;
using GlobalHelpers;
using Hl7.Fhir.Model;
using Hl7.Fhir.Utility;
using IEncounterMapper;
using IEncounterService;
using Interface.Misc.Helpers;
using Interface.Models.BackgroundServices;
using Interface.Models.EncounterModels;

namespace EncounterMapper
{
    public class VitalMapper : IVitalMapper
    {
        private readonly IVitalsService _vitalsService;
        private readonly IMapper _mapper;
        private readonly IFhirSerializer.IFhirSerializer _fhirSerializer;

        public VitalMapper (IVitalsService vitalsService,
            IMapper mapper, IFhirSerializer.IFhirSerializer fhirSerializer)
        {
            _vitalsService = vitalsService;
            _mapper = mapper;
            _fhirSerializer = fhirSerializer;
        }

        // Componenet defined here, giving decimal to datatype error in Profile
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
        public Bundle MapSync(PatientResourceRecords inputs)
        {
            var vitalsColl =  _vitalsService.PatientVitalsSessionViewModels(inputs.ResourceId,inputs.PatientId.Value).GetAwaiter().GetResult();

            var customBundle = new Bundle
            {
                Entry = new List<Bundle.EntryComponent>(),
                Type = Bundle.BundleType.Transaction,
            };
            Observation vitalFHIR = new Observation();
            foreach (var dto in vitalsColl)
            {
               
                        if (inputs.ResourceId.HasValue) 
                        { 
                            
                        if (inputs.ResourceId.Equals(dto.Id))
                        {
                     //   try
                     //   {
                            vitalFHIR = _mapper.Map<Observation>(dto);
                       
                        if (dto.SessionDate.HasValue)
                        {
                             vitalFHIR.Effective = new FhirDateTime(dto.SessionDate.Value);
                            vitalFHIR.Issued = dto.SessionDate;
                        }
                            
                            if (dto.Value.HasValue)
                            {
                                vitalFHIR.Component = getComponent(dto);
                            }
                        
                            var jsonString = _fhirSerializer.FhirR4SerializeResource(vitalFHIR);

                            var deserialized = _fhirSerializer.FhirR4DeSerialize(jsonString);

                            customBundle.Entry.Add(new Bundle.EntryComponent { Resource = deserialized,
                                Request = new Bundle.RequestComponent { Method = Bundle.HTTPVerb.POST, Url = "Observation" }
                            });
                     //   }
                    //    catch (Exception ex)
                    //    {
                    //        HelperMethods.CreateConsoleLog($"Exception {ex.Message}{ex.Source}{ex.InnerException} for {inputs.ResourceType} record id {inputs.ResourceId}{dto.VitalName} for {inputs.PatientId}   ");
                    //    }
                    }   
                        }
                        else
                        {
                //    try
                //    {                      
                            vitalFHIR = _mapper.Map<Observation>(dto);

                    if (dto.SessionDate.HasValue)
                    {
                        vitalFHIR.Effective = new FhirDateTime(dto.SessionDate.Value);
                        vitalFHIR.Issued = dto.SessionDate;
                    }
                        if (dto.Value.HasValue)
                        {
                            vitalFHIR.Component = getComponent(dto);
                        }
                        var jsonString = _fhirSerializer.FhirR4SerializeResource(vitalFHIR);

                            var deserialized = _fhirSerializer.FhirR4DeSerialize(jsonString);

                            customBundle.Entry.Add(new Bundle.EntryComponent { Resource = deserialized,
                                Request = new Bundle.RequestComponent { Method = Bundle.HTTPVerb.POST, Url = "Observation" }
                            });
               //     }
               //     catch (Exception ex)
               //     {
               //         HelperMethods.CreateConsoleLog($"Exception {ex.Message} for {inputs.ResourceType} record id {inputs.ResourceId}{dto.VitalName} for {inputs.PatientId}   ");
                //    }
                }
                 
            }
          //  customBundle.Total = customBundle.Entry.Count;
          //  customBundle.Id = Guid.NewGuid().ToString();

            return customBundle;
        }
    }
}
