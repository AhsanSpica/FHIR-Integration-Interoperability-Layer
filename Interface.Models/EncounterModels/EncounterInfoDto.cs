using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.EncounterModels
{
   
    public class EncounterInfoDto
    {

        public int? Id { get; set; }
        public int? LocationId { get; set; }
        public int? ProviderId { get; set; }
        public DateTimeOffset? EncounterDateTime { get; set; }
        public int? Reason { get; set; }
        public string? ReasonName { get; set; }
        public int? AppointmentOccurrenceId { get; set; }
        public int? AppointmentId { get; set; }
        public int? AppointmentTypeId { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public bool? IsDeleted { get; set; }
        public int? PracticeId { get; set; }
        public DateTimeOffset? DateOfService { get; set; }
        public int? RoomId { get; set; }
        public int? Duration { get; set; }
        public string? ReasonString { get; set; }
        public bool? Signed { get; set; }
        public bool? CoSigned { get; set; }
        public int? EncounterTypeId { get; set; }
        public string? ProviderFullName { get; set; }
        public string? EncounterTypeName { get; set; }
        public string? ProviderFirstName { get; set; }
        public string? ProviderLastName { get; set; }
        public string? AppointmentTypeName { get; set; }
        public int? PatientId { get; set; }
        public string? PatientMrn { get; set; }
        public string? PatientFirstName { get; set; }
        public string? PatientLastName { get; set; }
        public string? LocationName { get; set; }
        public int? TotalRows { get; set; }
        public int? PatientDisposition { get; set; }
        public string? PatientDispositionText { get; set; }
        public ResourceReference PatientReference { get; set; }
        public ResourceReference LocationReference { get; set; }
        public ResourceReference AppointmentReference { get; set; }
    }
    public class EncounterPagedWrapperModel
    {
        public List<EncounterInfoDto> EncounterInfos { get; set; }
        public int TotalSigned { get; set; }
        public int TotalUnsigned { get; set; }
        public int Total { get { return TotalSigned + TotalUnsigned; } }
    }
    public class PatientLatestEncounter
    {
        public long EncounterId { get; set; }
        public long PatientId { get; set; }
        public DateTimeOffset DateOfService { get; set; }
    }
}
