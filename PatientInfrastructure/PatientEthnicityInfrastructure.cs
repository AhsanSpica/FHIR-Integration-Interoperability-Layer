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
    public class PatientEthnicityInfrastructure : IPatientEthnicityInfrastructure
    {

        private readonly IPatientEthnicityRepository _patientEthnicityRepository;

        public PatientEthnicityInfrastructure(IPatientEthnicityRepository patientEthnicityRepository)
        {
            _patientEthnicityRepository = patientEthnicityRepository;
        }   

        public async Task<long> Add(List<PatientEthnicity> patientEthnicities)
        {
            return await _patientEthnicityRepository.Add(patientEthnicities);
        }

        public async Task<long?> Delete(long Id)
        {
            return await _patientEthnicityRepository.Delete(Id);
        }

        public async Task<List<PatientEthnicity>> GetListByPatientID(long PatientId)
        {
            return await _patientEthnicityRepository.GetListByPatientID(PatientId);
        }

        public async Task<PatientEthnicity> Update(List<PatientEthnicity> patientEthnicity)
        {
            return await _patientEthnicityRepository.Update(patientEthnicity);
        }
    }
}
