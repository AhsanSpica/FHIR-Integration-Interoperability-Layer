using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.ProviderModels
{
    public class GetProviderResponse : BaseEntity
    {
        public long Id { get; set; }
        public long PracticeId { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public int Gender { get; set; }
        public string? Npi { get; set; }
        public bool ValidNPI { get; set; }
        public string? Title { get; set; }
        public string? Upin { get; set; }
        public string? Dea { get; set; }
        public DateTimeOffset? DeaExpiry { get; set; }
        public string? LicenseState { get; set; }
        public string? StateLicenseNumber { get; set; }
        public DateTimeOffset? LicenseExpiryDate { get; set; }
        public string? Taxonomy { get; set; }
        public string? Email { get; set; }
        public string? DirectEmail { get; set; }
        public string? ContactNumber { get; set; }
        public string? OfficePhone { get; set; }
        public string? EINTIN { get; set; }
        public string? NADEAN { get; set; }
        public bool? IsBillingProvider { get; set; }
        public string? BillingAddress1 { get; set; }
        public string? BillingAddress2 { get; set; }
        public string? BillingZipCode { get; set; }
        public string? BillingCity { get; set; }
        public string? BillingState { get; set; }
        public string? BillingPTAN { get; set; }
        public string? BillingCAQAHId { get; set; }
        public string? BillingTaxId { get; set; }
        public string? Signature { get; set; }
        public long? UserId { get; set; }
        public string? Username { get; set; }
        public string? password { get; set; }
        public bool IsUser { get; set; }

        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
    }
    
}
