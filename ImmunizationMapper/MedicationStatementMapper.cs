using AutoMapper;
using FhirSerializer;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using IEncounterMapper;
using IEncounterService;
using IFhirSerializer;
using Interface.Misc.Helpers;
using Interface.Models.BackgroundServices;
using Interface.Models.InterfaceModels;
using Interface.Models.Medication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterMapper
{
    public class MedicationStatementMapper :IMedicationStatementMapper
    {
        private readonly IMedicationService _medicationService;
        private readonly IMapper _mapper;
        private readonly IFhirSerializer.IFhirSerializer _fhirSerializer;

        public MedicationStatementMapper(IMedicationService medicationService, IMapper mapper,
            IFhirSerializer.IFhirSerializer fhirSerializer)
        {
            _medicationService = medicationService;
            _mapper = mapper;
            _fhirSerializer = fhirSerializer;
        }
        public async Task<CustomBundle> Map(PatientResourceRecords inputs)
        {
            var customBundle = new CustomBundle
            {
                Entry = new List<CustomBundleEntry>(),
                Type = Bundle.BundleType.Searchset.ToString(),
                Meta = new Meta { LastUpdated = DateTimeOffset.Now }
            };
            var medicationDtos = await _medicationService.GetChartPrescriptionView(inputs.PatientId.Value);
            var medicationFHIRList = new List<MedicationStatementR4>();

            if (medicationDtos != null) { 
            foreach (ORMChartPrescriptionView medicationDto in medicationDtos)
            {
                var medicationFHIR = _mapper.Map<MedicationStatementR4>(medicationDto);
                medicationFHIRList.Add(medicationFHIR);
                customBundle.Entry.Add(new CustomBundleEntry { Resource = medicationFHIR });

            }
        }

            customBundle.Total = customBundle.Entry.Count;
            customBundle.Id = Guid.NewGuid().ToString();

            return customBundle;
        }
        public Bundle MapSync(PatientResourceRecords inputs)
        {
            // return Map(inputs).GetAwaiter().GetResult();
            
            var bundle = new Bundle
            {
                Entry = new List<Bundle.EntryComponent>(),
                Type = Bundle.BundleType.Transaction,
             //   Meta = new Meta { LastUpdated = DateTimeOffset.Now }
            };
            MedicationStatement medicationFHIR ;
            var medicationDtos = _medicationService.GetChartPrescriptionView(inputs.PatientId.Value).GetAwaiter().GetResult();

            if (medicationDtos != null)
            {
                foreach (ORMChartPrescriptionView medicationDto in medicationDtos.Where(medDto => inputs.ResourceId.HasValue && medDto.Id.Equals(inputs.ResourceId)).ToList())
                {
                    medicationFHIR = _mapper.Map<MedicationStatement>(medicationDto);

                    var jsonString = _fhirSerializer.FhirR4SerializeResource(medicationFHIR);

                    var deserialized = _fhirSerializer.FhirR4DeSerialize(jsonString);

                    bundle.Entry.Add(new Bundle.EntryComponent
                    {
                        Resource = medicationFHIR,
                        Request = new Bundle.RequestComponent { Method = Bundle.HTTPVerb.POST, Url = "MedicationStatement" }
                    });

                }
            }

            bundle.Id = Guid.NewGuid().ToString();
            return bundle;
        }

    }
}
//   }
//    catch (Exception ex)
//    {
//        HelperMethods.CreateConsoleLog($"Error :{ex.Message} for mapping Record  of type {medicationFHIR.TypeName}  ");
//    }

//else
//{  
//        medicationFHIR = _mapper.Map<MedicationStatement>(medicationDto);
//  //  try { 
//        var jsonString = _fhirSerializer.FhirR4SerializeResource(medicationFHIR);

//        var deserialized2 = _fhirSerializer.FhirR4DeSerialize(jsonString);

//        bundle.Entry.Add(new Bundle.EntryComponent { Resource = deserialized2,
//            Request = new Bundle.RequestComponent { Method = Bundle.HTTPVerb.POST, Url = "MedicationStatement" }
//        });
//     //   }
//    //    catch (Exception ex)
//    //    {
//    //        HelperMethods.CreateConsoleLog($"Error :{ex.Message} for mapping Record  of type {medicationFHIR.TypeName}  ");
//    //    }

//    }

//  customBundle.Total = customBundle.Entry.Count;
// customBundle.Id = Guid.NewGuid().ToString();

