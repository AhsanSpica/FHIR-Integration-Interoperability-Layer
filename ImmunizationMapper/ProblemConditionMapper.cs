using AutoMapper;
using GlobalHelpers;
using Hl7.Fhir.Model;
using IEncounterMapper;
using IEncounterService;
using Interface.Misc.Helpers;
using Interface.Models.BackgroundServices;
using Interface.Models.InterfaceModels;
using IPractitionerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterMapper
{
    public class ProblemConditionMapper : IProblemConditionMapper
    {
        private IMapper _mapper;
        private readonly LookUpScoped _lookUpScoped;
        private readonly IProblemConditionService _problemConditionService;
        private readonly IFhirSerializer.IFhirSerializer _fhirSerializer;

        public ProblemConditionMapper (IMapper mapper,
            LookUpScoped lookUpScoped,
            IProblemConditionService problemConditionService,
            IFhirSerializer.IFhirSerializer fhirSerializer )
        {
            _mapper = mapper;
         
            _lookUpScoped = lookUpScoped;
            _problemConditionService = problemConditionService;
            _lookUpScoped.FetchAllLookup();
            _fhirSerializer = fhirSerializer;
        }

        

        public Bundle MapSync(PatientResourceRecords inputs)
        {
            // return Map(inputs).GetAwaiter().GetResult();

            var customBundle = new Bundle
            {
                Entry = new List<Bundle.EntryComponent>(),
                Type = Bundle.BundleType.Transaction,
              //  Meta = new Meta { LastUpdated = DateTimeOffset.Now }
            };

            var problemCondtionDTOs =  _problemConditionService.GetPatientProblemById(inputs.PatientId, inputs.ResourceId,inputs.TableName).GetAwaiter().GetResult();
             var count = 0;

            foreach (var problemCondtionDTO in problemCondtionDTOs)
            {
                var problemCondtionFHIR = _mapper.Map<Condition>(problemCondtionDTO);

                try { 

                var jsonString = _fhirSerializer.FhirR4SerializeResource(problemCondtionFHIR);

                var deserialized = _fhirSerializer.FhirR4DeSerialize(jsonString);

                customBundle.Entry.Add(new Bundle.EntryComponent { Resource = deserialized,
                    Request = new Bundle.RequestComponent { Method = Bundle.HTTPVerb.POST, Url = "Condition" }
                });
            }
            catch (Exception ex)
            {
                HelperMethods.CreateConsoleLog($"Error :{ex.Message} for mapping Record  of type {problemCondtionFHIR.TypeName}  ");
            }

            //  customBundle.Entry.Add(new Bundle.EntryComponent { Resource = problemCondtionFHIR });
        }

          //  customBundle.Total = customBundle.Entry.Count;
         //   customBundle.Id = Guid.NewGuid().ToString();

            return customBundle;
        }
    }
}
