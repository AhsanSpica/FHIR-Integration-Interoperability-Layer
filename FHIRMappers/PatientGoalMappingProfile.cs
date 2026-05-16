using AutoMapper;
using Hl7.Fhir.Model;
using Hl7.Fhir.Support;
using Interface.Models.EncounterModels;
using Interface.Models.InterfaceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIRMappers
{
    public class PatientGoalMappingProfile : Profile
    {
        public PatientGoalMappingProfile()
        {
            CreateMap<GoalItemResponse, GoalR4>()
              //  .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))

                  // Meta and identifer mapped in the mapper layer 
                  .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => MapPatient(src)))
                .ForMember(dest => dest.LifecycleStatus, opt => opt.MapFrom(src => MapLifecycleStatus(src.IsActive, src.IsDeleted)))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => new CodeableConcept
                {
                    Text = string.IsNullOrWhiteSpace(src.GoalDetail) ? src.GoalDetail : "No Description"
                }))
                .ForMember(dest => dest.Target, opt => opt.MapFrom(src => new List<Goal.TargetComponent>
                {
                new Goal.TargetComponent
                {
                     Due = src.StartDate.HasValue ? new FhirDateTime(src.StartDate.Value.UtcDateTime) : null
                }
                }));
        }
        private ResourceReference MapPatient(GoalItemResponse src)
        {
           return src.PatientReference;
            
        }
      
        private string MapLifecycleStatus(bool? isActive, bool? isDeleted)
        {
            if (isDeleted.HasValue && isDeleted.Value)
                return Goal.GoalLifecycleStatus.EnteredInError.ToString();
            if (isActive.HasValue && isActive.Value)
                return Goal.GoalLifecycleStatus.Active.ToString();
            return Goal.GoalLifecycleStatus.Cancelled.ToString();
        }
    }
    public class PatientGoalFHIRMappingProfile : Profile
    {
        public PatientGoalFHIRMappingProfile()
        {
            CreateMap<GoalItemResponse, Goal>()
               // .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
               .ForMember(dest => dest.Id, opt => opt.Ignore())
                  // Meta and identifer mapped in the mapper layer 
                  .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => MapPatient(src)))
                  .ForMember(dest => dest.Target, opt => opt.Ignore())
                .ForMember(dest => dest.LifecycleStatus, opt => opt.MapFrom(src => MapLifecycleStatus(src.IsActive, src.IsDeleted)))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => getGoalDescription(src)));
               
        }
        private CodeableConcept getGoalDescription(GoalItemResponse src)
        {
            var returnString = "";
            if (src.GoalDetail.Trim().Equals(""))
            {
                returnString = "No Description";
            }
           else  if (src.GoalDetail.Equals(null))
            {
                returnString = "No Description";
            }
            else { returnString = src.GoalDetail; }

           return  new CodeableConcept
            {
                Text = returnString
            };
        }
        //private List<Goal.TargetComponent> MapGoalTarget(GoalItemResponse src)
        //{
        //    var result = new Goal.TargetComponent
        //    {
        //          = src.GoalDetail

        //    };
        //    return new List<Goal.TargetComponent>
        //    {
        //        result
        //    };
        //}


        private ResourceReference MapPatient(GoalItemResponse src)
        {
            return src.PatientReference;

        }

        private string MapLifecycleStatus(bool? isActive, bool? isDeleted)
        {
            if (isDeleted.HasValue && isDeleted.Value)
                return Goal.GoalLifecycleStatus.EnteredInError.ToString();
            if (isActive.HasValue && isActive.Value)
                return Goal.GoalLifecycleStatus.Active.ToString();
            return Goal.GoalLifecycleStatus.Cancelled.ToString();
        }
    }
}
