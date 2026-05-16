using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.Patients
{
    public class PatientInsurance : BaseModel
    {
        public long Id { get; set; }
        public long PatientId { get; set; }
        public string? PlanName { get; set; }
        public DateTimeOffset? StartDate { get; set; }//TODO
        public DateTimeOffset? EndDate { get; set; }

        [StringLength(25)]
        public string? SubscriberId { get; set; }
        public long? Coverage { get; set; }
        public long? RelationWithOwner { get; set; }
        public bool? IsActive { get; set; }
        public string? RelationFirstName { get; set; }
        public string? RelationLastName { get; set; }
        public long? RelationGender { get; set; }
        public DateTimeOffset? RelationDateOfBirth { get; set; }
        public string? RelationSSN { get; set; }
        public bool SelfPay { get; set; }
        public long? PayerId { get; set; }
        public long? PayerTypeId { get; set; }
        public long? InsurancePlanId { get; set; }
        public decimal? Copay { get; set; }        
        public bool? CopayIsPercentage { get; set; }
        public decimal? DeductibleAmount { get; set; }
        public string? SubscriberAddress { get; set; }
        public string? SubscriberZip { get; set; }
        public string? SubscriberCity { get; set; }
        public string? SubscriberState { get; set; }
        public string? SubscriberPhone { get; set; }

        [StringLength(25)]
        public string? GroupId { get; set; }
    }
}
