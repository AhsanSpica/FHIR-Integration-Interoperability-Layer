using EHR.Models.Patients;
using Interface.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPatientsRepository
{
    public interface IPatientsAddressRepository
    {

        Task<long> Add(List<PatientAddress> patientAddresses);

        Task<PatientAddress> Update(List<PatientAddress> patientAddress);

        Task<long?> Delete(long Id);

        Task<PatientAddress> GetByPatientID(long patientId);

        Task<List<PatientAddress>> GetListByPatientID(long patientId);
    }
}
