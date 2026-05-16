using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.Patients
{
    public class Providers
    {

        public class Practices : BaseModel
        {
            public int PracticeId { get; set; }
            public string? Name { get; set; }
            public string? LegalName { get; set; }
            public string? TIN { get; set; }
            public string? GroupNPI { get; set; }
            public string? CCN { get; set; }
            public int PrimaryLocationId { get; set; }
            public int PracticeTypeId { get; set; }
            public string? Type { get; set; }
            public int PracticeStatusId { get; set; }
            public string? Status { get; set; }

        }


        public class PracticeLocation : BaseModel
        {
            public int LocationId { get; set; }
            public int PracticeId { get; set; }
            public string? PracticeName { get; set; }
            public string? PracticeLegalName { get; set; }
            public string? Address { get; set; }
            public string? Address2 { get; set; }
            public string? City { get; set; }
            public string? State { get; set; }
            public int? Zip { get; set; }
            public string? Phone { get; set; }
            public string? Fax { get; set; }
            public string? LocationName { get; set; }
            public bool IsOpenMonday { get; set; }
            public int HourOpenMonday { get; set; }
            public int HourCloseMonday { get; set; }
            public bool IsOpenTuesday { get; set; }
            public int HourOpenTuesday { get; set; }
            public int HourCloseTuesday { get; set; }
            public bool IsOpenWednesday { get; set; }
            public int HourOpenWednesday { get; set; }
            public int HourCloseWednesday { get; set; }
            public bool IsOpenThursday { get; set; }
            public int HourOpenThursday { get; set; }
            public int HourCloseThursday { get; set; }
            public bool IsOpenFriday { get; set; }
            public int HourOpenFriday { get; set; }
            public int HourCloseFriday { get; set; }
            public bool IsOpenSaturday { get; set; }
            public int HourOpenSaturday { get; set; }
            public int HourCloseSaturday { get; set; }
            public bool IsOpenSunday { get; set; }
            public int HourOpenSunday { get; set; }
            public int HourCloseSunday { get; set; }
            public string? OfficeHoursNote { get; set; }
            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }
            public bool IsPrimary { get; set; }
            public bool IsPlaceOfService { get; set; }
            public bool IsBillingLocation { get; set; }

        }

        public class Provider : BaseModel
        {
            public int LocationId { get; set; }
            public int ProviderId { get; set; }
            public int PracticeId { get; set; }
            public int ProviderTypeId { get; set; }
            public string? ProviderType { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public byte[]? Address { get; set; }
            public string? Title { get; set; }
            public string? Email { get; set; }
            public string? PhoneOffice { get; set; }
            public string? PhoneMobile { get; set; }
            public string? NPI { get; set; }
            public int SpecialityId { get; set; }
            public string? Fax { get; set; }
            public string? Gender { get; set; }
            public string? IndividualTin { get; set; }
            public bool IsActive { get; set; }
            public bool PreferEmail { get; set; }
            public bool PreferPhone { get; set; }
            public bool PreferText { get; set; }
            public bool PreferFax { get; set; }
            public string? ProviderName { get; set; }
        }
        public class GetLocationProvider : Provider
        {
            public string? ProviderColor { get; set; }

        }

    }
}
