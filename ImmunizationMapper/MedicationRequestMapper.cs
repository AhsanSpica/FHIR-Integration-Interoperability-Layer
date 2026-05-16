using AutoMapper;
using Hl7.Fhir.Model;
using IEncounterMapper;
using IEncounterService;
using Interface.Misc.Helpers;
using Interface.Models.BackgroundServices;
using Interface.Models.InterfaceModels;
using Interface.Models.Medication;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterMapper
{
    public class MedicationRequestMapper : IMedicationRequestMapper
    {
        private readonly IMedicationService _medicationService;
        private readonly IMapper _mapper;
        private readonly IFhirSerializer.IFhirSerializer _fhirSerializer;

        public MedicationRequestMapper(IMedicationService medicationService, IMapper mapper,
            IFhirSerializer.IFhirSerializer fhirSerializer)
        {
            _medicationService = medicationService;
            _mapper = mapper;
            _fhirSerializer = fhirSerializer;
        }
  
        public Bundle MapSync(PatientResourceRecords inputs)
        {
            MedicationRequest medicationFHIR;
            var medicationDtos =  _medicationService.GetChartPrescriptionView(inputs.PatientId.Value).GetAwaiter().GetResult();

            var bundle = new Bundle
            {
                Entry = new List<Bundle.EntryComponent>(),
                Type = Bundle.BundleType.Transaction,
               // Meta = new Meta { LastUpdated = DateTimeOffset.Now }
            };

             

            if (medicationDtos != null)
            {
                foreach (ORMChartPrescriptionView medicationDto in medicationDtos.Where(medDto => inputs.ResourceId.HasValue && medDto.Id.Equals(inputs.ResourceId)).ToList())
                {
                    
                            medicationFHIR = _mapper.Map<MedicationRequest>(medicationDto);
                            try
                            {
                                var jsonString = _fhirSerializer.FhirR4SerializeResource(medicationFHIR);

                            var deserialized = _fhirSerializer.FhirR4DeSerialize(jsonString);

                            bundle.Entry.Add(new Bundle.EntryComponent { Resource = deserialized,
                                Request = new Bundle.RequestComponent { Method = Bundle.HTTPVerb.POST, Url = "MedicationRequest" }
                            });
                            }
                            catch (Exception ex)
                            {
                                HelperMethods.CreateConsoleLog($"Error :{ex.Message} for mapping Record  of type {medicationFHIR.TypeName}  ");
                            }                       
                }
            }
           // bundle.Total = bundle.Entry.Count;
            bundle.Id = Guid.NewGuid().ToString();

            return bundle;
            
        }

    }
}
