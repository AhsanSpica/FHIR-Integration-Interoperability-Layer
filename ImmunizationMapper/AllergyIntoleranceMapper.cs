using AutoMapper;
using Hl7.Fhir.Model;
using IEncounterMapper;
using IEncounterService;
using Interface.Misc.Helpers;
using Interface.Models.BackgroundServices;
using Interface.Models.InterfaceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterMapper
{
    public class AllergyIntoleranceMapper : IAllergyIntoleranceMapper
    {
        private readonly IAllergyIntoleranceService _allergyIntoleranceService;
        private readonly IMapper _mapper;
        private readonly IFhirSerializer.IFhirSerializer _fhirSerializer;

        public AllergyIntoleranceMapper(IAllergyIntoleranceService allergyIntoleranceService,
            IMapper mapper,
            IFhirSerializer.IFhirSerializer fhirSerializer)
        {
            _allergyIntoleranceService = allergyIntoleranceService;
            _mapper = mapper;
            _fhirSerializer = fhirSerializer;
        }
        //public async Task<CustomBundle> Map(PatientResourceRecords inputs)
        //{

        //    var allergyDTOs = await _allergyIntoleranceService.GetAllergiesView(inputs.PatientId.Value);
        //    var allergyFHIRList = new List<AllergyIntoleranceR4>();
        //    var customBundle = new CustomBundle
        //    {
        //        Entry = new List<CustomBundleEntry>(),
        //        Type = Bundle.BundleType.Searchset.ToString(),
        //        Meta = new Meta { LastUpdated = DateTimeOffset.Now }
        //    };

        //    if (allergyDTOs != null)
        //    {
        //        foreach (var allergyDTO in allergyDTOs)
        //        {
        //            var encounterFHIR = _mapper.Map<AllergyIntoleranceR4>(allergyDTO);

        //            allergyFHIRList.Add(encounterFHIR);
        //            customBundle.Entry.Add(new CustomBundleEntry
        //            {
        //                Resource = encounterFHIR
        //            });
        //        }
        //    }
        //    customBundle.Total = customBundle.Entry.Count;
        //    customBundle.Id = Guid.NewGuid().ToString();

        //    return customBundle;
        //}
          public async Task<Bundle> Map(PatientResourceRecords inputs)
        {
            var allergyDTOs =  await _allergyIntoleranceService.GetAllergiesView(inputs.PatientId.Value);
 
            var customBundle = new Bundle
            {
                Entry = new List<Bundle.EntryComponent>(),
                Type = Bundle.BundleType.Transaction,
               // Meta = new Meta { LastUpdated = DateTimeOffset.Now }
            };
            var count = 0;
            if (allergyDTOs != null)
            {
                foreach (var allergyDTO in allergyDTOs)
                {
                    //::Mapped here
                    var encounterFHIR = _mapper.Map<AllergyIntolerance>(allergyDTO);

                    var jsonString = _fhirSerializer.FhirR4SerializeResource(encounterFHIR);

                    var deserialized = _fhirSerializer.FhirR4DeSerialize(jsonString);

                    customBundle.Entry.Add(new Bundle.EntryComponent { Resource = deserialized , Request = new Bundle.RequestComponent { Method=Bundle.HTTPVerb.POST,Url= "AllergyIntolerance" } });

                    //customBundle.Entry.Add(new Bundle.EntryComponent
                    //{
                    //    Resource = encounterFHIR
                    //});
                    count++;
                }
            }

            //customBundle.Total = customBundle.Entry.Count;
           // customBundle.Id = Guid.NewGuid().ToString();

            return customBundle;
        }
        public Bundle MapSync(PatientResourceRecords inputs)
        {
            var allergyDTOs =  _allergyIntoleranceService.GetAllergiesViewSingular(inputs.ResourceId).GetAwaiter().GetResult();
 
            var customBundle = new Bundle
            {
                Entry = new List<Bundle.EntryComponent>(),
                Type = Bundle.BundleType.Transaction,
                Meta = new Meta { LastUpdated = DateTimeOffset.Now }
            };
          
                var count = 0;
                if (allergyDTOs != null)
                {
                    foreach (var allergyDTO in allergyDTOs)
                    {
                        //::Mapped here
                        var encounterFHIR = _mapper.Map<AllergyIntolerance>(allergyDTO);
                     try
                        { 
                        var jsonString = _fhirSerializer.FhirR4SerializeResource(encounterFHIR);

                        var deserialized = _fhirSerializer.FhirR4DeSerialize(jsonString);

                        customBundle.Entry.Add(new Bundle.EntryComponent { Resource = deserialized, Request = new Bundle.RequestComponent { Method=Bundle.HTTPVerb.POST,Url= "AllergyIntolerance" } });
                        }
                        catch (Exception ex ) 
                        {
                            HelperMethods.CreateConsoleLog($"Exception {ex.Message } for record id :  of type {encounterFHIR.TypeName}");
                        }
                        //customBundle.Entry.Add(new Bundle.EntryComponent
                        //{
                        //    Resource = encounterFHIR
                        //});
                        count++;
                    
                    }
                }
           
          //  customBundle.Total = customBundle.Entry.Count;
            customBundle.Id = Guid.NewGuid().ToString();

            return customBundle;
        }
    }
}
