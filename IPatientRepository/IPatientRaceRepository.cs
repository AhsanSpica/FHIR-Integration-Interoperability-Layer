using EHR.Models.Patients;
using Interface.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPatientsRepository
{
    public interface IPatientRaceRepository
    {
        Task<long> Add(List<PatientRace> patientRaces);

        Task<PatientRace> Update(List<PatientRace> patientRace);

        Task<long?> Delete(long Id);

        Task<List<PatientRace>> GetListByPatientID(long PatientId);
    }
}
