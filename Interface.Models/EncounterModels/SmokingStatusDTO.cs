using Hl7.Fhir.Model;
using Interface.Models.Common;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.EncounterModels
{
    public class SmokingStatusDTO : BaseModel, IValidatableObject

    {
        public long? Id { get; set; }
        public long? EncounterId { get; set; }
        public long? PatientId { get; set; }
        public int? UseId { get; set; }
        public int? TobaccoType { get; set; }
        public int? PackDay { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public int? Cessation { get; set; }
        public string? Notes { get; set; }
        public string? RefillsUsedDay { get; set; }
        public int? Status { get; set; }
        public string? StatusName { get; set; }
        public int? StartYear { get; set; }
        public string? TobaccoTypeName { get; set; }
        public string? PackDayName { get; set; }
        public string? CessationName { get; set; }
        public bool? CurrentStatus { get; set; }
        public string? UseName { get; set; }
        public string? FHIRStatusCode { get; set; }
        public string? ObservationStatus { get; set; }
        public string? PatientMrn { get; set; }
        public ResourceReference PatientReference { get; set; }
        public ResourceReference EncounterReference { get; set; }
        public ResourceReference PractitionerReference { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        { 

            if (Id == null && validationContext.GetService(typeof(IActionContextAccessor)) is IActionContextAccessor actionContextAccessor)
            {
                var actionContext = actionContextAccessor.ActionContext;
                if (actionContext != null && actionContext.ActionDescriptor.RouteValues["action"] == "AddSmokingStatus")
                {
                    yield break;
                }
                if (actionContext != null && actionContext.ActionDescriptor.RouteValues["action"] == "UpdateSmokingStatus")
                {
                    yield return new ValidationResult("Id is required", new[] { nameof(Id) });
                }
            }


        }
    }


     
}
