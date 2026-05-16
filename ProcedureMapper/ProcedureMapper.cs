using AutoMapper;
using Hl7.Fhir.Model;
using IFhirSerializer;
using Interface.Misc.Helpers;
using Interface.Models.BackgroundServices;
using Interface.Models.InterfaceModels;
using IProcedureMapper;
using IProcedureService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcedureMapper
{
    public class ProcedureMapper : IProcedureMapper.IProcedureMapper
    {
        private readonly IProcedureService.IProcedureService _procedureService;
        private readonly IMapper _mapper;
        private readonly IFhirSerializer.IFhirSerializer _fhirSerializer;
        public ProcedureMapper( IMapper mapper,
            IProcedureService.IProcedureService procedureService,
            IFhirSerializer.IFhirSerializer fhirSerializer)
        {
            _procedureService = procedureService;
            _mapper = mapper;
            _fhirSerializer = fhirSerializer;
        }
      
        public Bundle MapSync(PatientResourceRecords inputs)
        {
            // return Map(inputs).GetAwaiter().GetResult();
            var customBundle = new Bundle
            {
                Entry = new List<Bundle.EntryComponent>(),
                Type = Bundle.BundleType.Transaction,
               // Meta = new Meta { LastUpdated = DateTimeOffset.Now }
            };

            var procedureDtos =  _procedureService.GetCombinedProcedures(inputs.PatientId, inputs.ResourceId, inputs.TableName).GetAwaiter().GetResult();
            var procedureFHIRList = new List<Procedure>();
            var count = 0;

            foreach (var procedureDto in procedureDtos)
            {
                var procedureFHIR = _mapper.Map<Procedure>(procedureDto);


                var jsonString = _fhirSerializer.FhirR4SerializeResource(procedureFHIR);


                var deserialized = _fhirSerializer.FhirR4DeSerialize(jsonString);


                customBundle.Entry.Add(new Bundle.EntryComponent
                {
                    Resource = deserialized,
                    Request = new Bundle.RequestComponent { Method = Bundle.HTTPVerb.POST, Url = "Procedure" }
                });
            }

          //  customBundle.Total = customBundle.Entry.Count;
          //  customBundle.Id = Guid.NewGuid().ToString();

            return customBundle;
        }
    }
}
