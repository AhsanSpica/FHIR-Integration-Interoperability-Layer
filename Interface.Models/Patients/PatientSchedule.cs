using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Interface.Models.Patients
{
    public class AppointmentScreenWrapper2
    {
        public List<AppointmentScreenResult2> appointmentScreenResults { get; set; }
        public List<CountModel2> ProviderAppointmentCounts { get; set; }
        public List<CountModel2> TypeAppointmentCounts { get; set; }
        public List<CountModel2> StatusAppointmentCounts { get; set; }
        public List<CountModel2> ReasonAppointmentCounts { get; set; }
    }

    public class AppointmentScreenResult2 : BaseModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MRN { get; set; }
        public List<PatientAddress>? PatientAddress { get; set; }
        public List<PatientPhone>? PatientPhone { get; set; }
        public DateTimeOffset? Accessed { get; set; }
        public string? AccessedBy { get; set; }
        public long? BirthSex { get; set; }

        public int Id { get; set; }
        public int ProviderId { get; set; }
        public string Note { get; set; }
        
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string AppointmentType { get; set; }
        public string DocInitals { get; set; }
        public string PatientId { get; set; }
        public string ProviderName { get; set; }
        public string ProviderColor { get; set; }
        public string PatientName { get; set; }
        //public string PatientFirstName { get; set; }
        //public string PatientLastName { get; set; }
        public string ProfilePic { get; set; }
        public int PatientAge { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PatientContact { get; set; }
        public string PatientInsurance { get; set; }
        public string PatientEligibility { get; set; }
        public string PatientCopay { get; set; }
        public string ReasonForVisit { get; set; }
        public string AppointmentStatus { get; set; }
        public string StatusColor { get; set; }
        public string BlockReason { get; set; }
        public int TotalRows { get; set; }
        public string? Phone { get; set; }
        public string? PhoneType { get; set; }
        public bool IsStatusActive { get; set; }
    }

    public class CountModel2
    {
        public string Type { get; set; }
        public int Id { get; set; }
        public int AppointmentCount { get; set; }

    }

    public class PatientDto4 : AppointmentScreenResult2 { }
}
