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
    public class PatientsPhoneInfrastructure : IPatientsPhoneInfrastructure
    {
        private readonly IPatientsPhoneRepository _patientsPhoneRepository;

        public PatientsPhoneInfrastructure(IPatientsPhoneRepository patientsPhoneRepository)
        {
            _patientsPhoneRepository = patientsPhoneRepository;
        }

        public async Task<long> Add(List<PatientPhone> patientPhones)
        {
            return await _patientsPhoneRepository.Add(patientPhones);
        }

        public async Task<long?> Delete(long Id)
        {
            return await _patientsPhoneRepository.Delete(Id);
        }

        public async Task<PatientPhone> GetByPatientID(long patientId)
        {
            return await _patientsPhoneRepository.GetByPatientID(patientId);
        }

        public async Task<PatientPhone> Update(List<PatientPhone> patientPhones)
        {
            return await _patientsPhoneRepository.Update(patientPhones);
        }

        public async Task<List<PatientPhone>> GetListByPatientID(long patientId)
        {
            return await _patientsPhoneRepository.GetListByPatientID(patientId);
        }
    }
}
