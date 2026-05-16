using AutoMapper;
using Hl7.Fhir.Model;
using IEncounterMapper;
using IEncounterService;
using Interface.Misc.Helpers;
using Interface.Models.BackgroundServices;
using Interface.Models.EncounterModels;
using Interface.Models.InterfaceModels;
using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterMapper
{
    public class AssessmentGoalMapper :IAssessmentGoalMapper
    {
        private readonly IAssessmentGoalService _assessmentGoalService;
        private readonly IMapper _mapper;
        private readonly IFhirSerializer.IFhirSerializer _fhirSerializer;
        public AssessmentGoalMapper( IAssessmentGoalService assessmentGoalService,
            IMapper mapper,
            IFhirSerializer.IFhirSerializer mapperSerializer)

        {
            _assessmentGoalService = assessmentGoalService;
            _mapper = mapper;
            _fhirSerializer = mapperSerializer;   
        }

       
        private List<Annotation> MapNotes(GoalMasterResponse src)
        {
            List<Annotation> annotations = new List<Annotation>();
            annotations.Add(new Annotation { Text = string.IsNullOrWhiteSpace(src.Notes) ? src.Notes : "No Description"   });
                    
                    
            return annotations;
        }
        private List<Identifier> MapIdentifier(GoalMasterResponse srcMaster, GoalItemResponse goalitem)
        {
            var baseURL = "https://qa.wmi360.com/EHR/api/main/api/v1";
            if (srcMaster.EncounterId > 0)
            {
                baseURL= $"{baseURL}/encounters/getencounterwrapperbyid/{srcMaster.EncounterId}?IncludeGoals=true";
            }
            else
            {
                baseURL= $"{baseURL}/encounters/getencounterwrapperbyid";

            }
           return  new List<Identifier> { new Identifier(baseURL, goalitem.Id.ToString()) };
           
        }
        private Meta MapMeta(GoalMasterResponse src)
        {          
            return new Meta
            {
                LastUpdated = src.UpdatedAt ?? DateTimeOffset.UtcNow,
                Profile = new List<string> { "http://hl7.org/fhir/us/core/StructureDefinition/us-core-goal" }
            };
        }

        public Bundle MapSync(PatientResourceRecords inputs)
        {
            // return Map(inputs).GetAwaiter().GetResult();
             
                var goalDTOs = _assessmentGoalService.GoalGetByEncounterId(inputs.PatientId).GetAwaiter().GetResult();
            

            var customBundle = new Bundle
            {
                Entry = new List<Bundle.EntryComponent>(),
                Type = Bundle.BundleType.Transaction,
              //  Meta = new Meta { LastUpdated = DateTimeOffset.Now }
            };
             foreach (var goalDTO in goalDTOs)
            {
                foreach (var goalitem in goalDTO.GoalItems)
                {
                    //:: Mapped here

                    if (inputs.ResourceId.HasValue)
                    {
                        if (inputs.ResourceId.Equals(goalitem.Id))
                        {
                            var goalFHIR = _mapper.Map<Goal>(goalitem);
                            try
                            {
                                goalFHIR.Identifier = MapIdentifier(goalDTO, goalitem);
                          //  goalFHIR.Meta = MapMeta(goalDTO);
                            goalFHIR.Note = MapNotes(goalDTO);

                            var jsonString = _fhirSerializer.FhirR4SerializeResource(goalFHIR);

                            var deserialized = _fhirSerializer.FhirR4DeSerialize(jsonString);

                            customBundle.Entry.Add(new Bundle.EntryComponent { Resource = deserialized , Request = new Bundle.RequestComponent { Method = Bundle.HTTPVerb.POST, Url = "Goal" } });
                            }
                            catch (Exception ex)
                            {
                                HelperMethods.CreateConsoleLog($"Error :{ex.Message} for mapping Record  of type {goalFHIR.TypeName}  ");
                            }
                        }
                    }
                    else
                    {
                        var goalFHIR = _mapper.Map<Goal>(goalitem);
                        try
                        {
                            goalFHIR.Identifier = MapIdentifier(goalDTO, goalitem);
                      //  goalFHIR.Meta = MapMeta(goalDTO);
                        goalFHIR.Note = MapNotes(goalDTO);

                        var jsonString = _fhirSerializer.FhirR4SerializeResource(goalFHIR);

                        var deserialized = _fhirSerializer.FhirR4DeSerialize(jsonString);

                        customBundle.Entry.Add(new Bundle.EntryComponent { Resource = deserialized
                            , Request = new Bundle.RequestComponent { Method = Bundle.HTTPVerb.POST, Url = "Goal" } });
                        }
                        catch (Exception ex)
                        {
                            HelperMethods.CreateConsoleLog($"Error :{ex.Message} for mapping Record  of type {goalFHIR.TypeName}  ");
                        }
                    }

                }
            }

          //  customBundle.Total = customBundle.Entry.Count;
            customBundle.Id = Guid.NewGuid().ToString();

            return customBundle;
        }

    }
}
