using Hl7.Fhir.Model;
using Interface.Models.BackgroundServices;
using Interface.Models.InterfaceModels;
using Interface.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPatientMapper
{
    public interface IPatientMapper
    {

       // Task<CustomBundle> Map(PatientResourceRecords inputs);
        Bundle MapSync(PatientResourceRecords inputs);
        //  Task<CareTeamBundle> GetPatientByID(long PatientId, long PracticeId);

    }
}
