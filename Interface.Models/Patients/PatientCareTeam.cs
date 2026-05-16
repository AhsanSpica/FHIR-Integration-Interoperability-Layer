
using Hl7.Fhir.Model;
using Interface.Models.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.Patients
{  
    public class PatientCareTeam : BaseModel
    {
        [Key]
        public long Id { get; set; }
        public long PatientId { get; set; }

        [MaxLength(100), MinLength(0)]
        public string? Name { get; set; }
        public int? Status { get; set; }
        public bool IsActive { get; set; }

        public List<PatientCareTeamMember>? PatientCareTeamMembers { get; set; }
    }

    public class PatientCareTeamMember : BaseModel
    {
        [Key]
        public long Id { get; set; }
        public bool? IsActive { get; set; }
        public long? ProviderReferenceId { get; set; }

        public string? SpecialtyName { get; set; }

        [MaxLength(50), MinLength(0)]
        public string? Phone { get; set; }

        [MaxLength(50), MinLength(0)]
        public string? Fax { get; set; }

        //[MaxLength(50), MinLength(0)]
        public int? Specialty { get; set; }


        [MaxLength(15), MinLength(0)]
        public string? NPI { get; set; }
        public string? Taxonomy { get; set; }
        public string? TaxonomyDescription { get; set; }
        //[MaxLength(50), MinLength(0)]
        public int? RelationWithPatient { get; set; }

        [MaxLength(100), MinLength(0)]
        public string? Email { get; set; }
        public string? Title { get; set; }

        [MaxLength(100), MinLength(0)]
        public string? FullName { get; set; }

        [Required]
        public long PatientCareTeamId { get; set; }
        public long PatientId { get; set; }

        //ETL 
        public string? PatientMrn { get; set; }
        public ResourceReference PatientReference { get; set; }
        public ResourceReference EncounterReference { get; set; }
        public ResourceReference PractitionerReference { get; set; }
    }
    public class PatientCareTeamAndMemberResponseModel : BaseModel
    {
        [Key]
        public long Id { get; set; }
        public long PatientId { get; set; }

        [MaxLength(100), MinLength(0)]
        public string? Name { get; set; }
        public int? Status { get; set; }
        public bool IsActive { get; set; }

        public List<PatientCareTeamMember>? PatientCareTeamMembers
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.PatientCareTeamMembersJson))
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<PatientCareTeamMember>>(this.PatientCareTeamMembersJson);
                }
                return new List<PatientCareTeamMember> { };
            }
        }
        [JsonIgnore]
        public string? PatientCareTeamMembersJson { get; set; }
    }
}
