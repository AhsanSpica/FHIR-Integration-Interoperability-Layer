using EHR.Models.Patients;
using Interface.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPatientsRepository
{
    public interface IPatientEthnicityRepository
    {
        Task<long> Add(List<PatientEthnicity> patientEthnicities);

        Task<PatientEthnicity> Update(List<PatientEthnicity> patientEthnicity);

        Task<long?> Delete(long Id);

        Task<List<PatientEthnicity>> GetListByPatientID(long PatientId);

    }
}
