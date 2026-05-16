using EHR.Models.Patients;
using Interface.Models.Patients;
using IPatientsInfrastructure;
using IPatientsInfrasturcture;
using IPatientsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsInfrastructure
{
    public class PatientRaceInfrastructure : IPatientRaceInfrastructure
    {

        private readonly IPatientRaceRepository _patientRaceRepository;


        public PatientRaceInfrastructure(IPatientRaceRepository patientRaceRepository)
        {
            _patientRaceRepository = patientRaceRepository;
        }

        public async Task<long> Add(List<PatientRace> patientRaces)
        {
            return await _patientRaceRepository.Add(patientRaces);
        }

        public async Task<long?> Delete(long Id)
        {
            return await _patientRaceRepository.Delete(Id);
        }

        public async Task<List<PatientRace>> GetListByPatientID(long PatientId)
        {
            return await _patientRaceRepository.GetListByPatientID(PatientId);
        }

        public async Task<PatientRace> Update(List<PatientRace> patientRace)
        {
            return await _patientRaceRepository.Update(patientRace);
        }
    }
}
