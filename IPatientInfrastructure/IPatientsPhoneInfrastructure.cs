using EHR.Models.Patients;
using Interface.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPatientsInfrastructure
{
    public interface IPatientsPhoneInfrastructure
    {
        Task<long> Add(List<PatientPhone> patientPhones);

        Task<PatientPhone> Update(List<PatientPhone> patientPhones);

        Task<long?> Delete(long Id);

        Task<PatientPhone> GetByPatientID(long patientId);

        Task<List<PatientPhone>> GetListByPatientID(long patientId);
    }
}
