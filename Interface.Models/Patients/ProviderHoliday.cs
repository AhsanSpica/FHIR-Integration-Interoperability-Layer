using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Interface.Models.Patients
{
    public class ProviderHoliday
    {
        public long Id { get; set; }
        public long PracticeId { get; set; }
        public string? ProviderId { get; set; }
        public long LocationId { get; set; }
        public DateTime OffDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class ProviderHolidayRange
    {
        public long Id { get; set; }
        public long ProviderId { get; set; }
        public long LocationId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Title { get; set; }
        public string? Comments { get; set; }

        [JsonIgnore]
        public long PracticeId { get; set; }

        [JsonIgnore]
        public string? CreatedBy { get; set; }
        
        [JsonIgnore]
        public string? UpdatedBy { get; set; }
    }
}
