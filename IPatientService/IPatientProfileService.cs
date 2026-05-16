using Interface.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPatientService
{
    public interface IPatientProfileService
    {
        Task<PatientInfo> GetByID(long PatientId, long PracticeId);

    }
}
