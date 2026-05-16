using AutoMapper;
using GlobalHelpers;
using Hl7.Fhir.Model;
using IEncounterMapper;
using IEncounterService;
using Interface.Misc.Helpers;
using Interface.Models.BackgroundServices;
using Interface.Models.EncounterModels;
using Interface.Models.InterfaceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EncounterMapper
{
    public class SmokingStatusMapper : ISmokingStatusMapper
    {
        private readonly ISmokingStatusService _smokingStatusService;
        private readonly IMapper _mapper;
        private LookUpScoped _lookUpScoped;
        private readonly IFhirSerializer.IFhirSerializer _fhirSerializer;
        public SmokingStatusMapper(ISmokingStatusService smokingStatusService,
            IMapper mapper,
            LookUpScoped lookUpScoped,
            IFhirSerializer.IFhirSerializer fhirSerializer )
        {
            _smokingStatusService = smokingStatusService;
            _mapper = mapper;   
            _lookUpScoped = lookUpScoped;
            _fhirSerializer = fhirSerializer;
        }
    
        public Bundle MapSync(PatientResourceRecords inputs)
        {
            var dtos =  _smokingStatusService.GetSmokingByPatientId(inputs.PatientId.Value, inputs.EncounterId.Value).GetAwaiter().GetResult();

            var customBundle = new Bundle
            {
                Entry = new List<Bundle.EntryComponent>(),
                Type = Bundle.BundleType.Transaction,
              //  Meta = new Meta { LastUpdated = DateTimeOffset.Now }
            };

            Observation smokingstatusFHIR;
            foreach (var dto in dtos)
            {
                if (inputs.ResourceId != null)
            {
               
                if (inputs.ResourceId.Equals(dto.Id))
                {
                    smokingstatusFHIR = _mapper.Map<Observation>(dto);
                       
                        if (!string.IsNullOrEmpty(dto.FHIRStatusCode))
                        {
                            smokingstatusFHIR.Value = GetSnomedValue(dto.FHIRStatusCode, dto.UseName);
                        }
                        if (dto.StartDate.HasValue)
                        {
                            smokingstatusFHIR.Issued = dto.StartDate;
                        }

                        try
                        {
                            var jsonString = _fhirSerializer.FhirR4SerializeResource(smokingstatusFHIR);
                            
                            var deserialized = _fhirSerializer.FhirR4DeSerialize(jsonString);
   
                            customBundle.Entry.Add(new Bundle.EntryComponent { Resource = deserialized,
                        Request = new Bundle.RequestComponent { Method = Bundle.HTTPVerb.POST, Url = "Observation" }
                    });
                    }
            catch (Exception ex)
            {
                        HelperMethods.CreateConsoleLog($"Error : {ex.Message} for mapping Record  of type {smokingstatusFHIR.TypeName}  ");
                    }
                }    
            }
                else
            {              
                    smokingstatusFHIR = _mapper.Map<Observation>(dto);

                    if (!string.IsNullOrEmpty(dto.FHIRStatusCode))
                    {
                        smokingstatusFHIR.Value = GetSnomedValue(dto.FHIRStatusCode, dto.UseName);
                    }

                    if (dto.StartDate.HasValue)
                    {
                        smokingstatusFHIR.Issued = dto.StartDate;
                    }

                    try { 
                    var jsonString = _fhirSerializer.FhirR4SerializeResource(smokingstatusFHIR);

                    var deserialized = _fhirSerializer.FhirR4DeSerialize(jsonString);

                    customBundle.Entry.Add(new Bundle.EntryComponent { Resource = deserialized,
                        Request = new Bundle.RequestComponent { Method = Bundle.HTTPVerb.POST, Url = "Observation" }
                    });
            }
            catch (Exception ex)
            {
                HelperMethods.CreateConsoleLog($"Error :{ex.Message} for mapping Record  of type {smokingstatusFHIR.TypeName}  ");
            }
        }
            }

          //  customBundle.Total = customBundle.Entry.Count;
          //  customBundle.Id = Guid.NewGuid().ToString();

            return customBundle;
           
        }

        private static CodeableConcept GetSnomedValue(string? FHIRStatusCode, string? UseName )
        
        {
            return new CodeableConcept
            {
                Coding = new List<Coding>
                    {
                    new Coding
                    {
                        System = "http://snomed.info/sct",
                        Code =  FHIRStatusCode ,
                        Display = UseName
                    }
                    },
                Text = UseName
            };

        }
    }
}
