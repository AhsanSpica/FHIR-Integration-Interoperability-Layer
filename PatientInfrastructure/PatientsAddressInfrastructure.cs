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
    public class PatientsAddressInfrastructure : IPatientsAddressInfrastructure
    {
        private readonly IPatientsAddressRepository _patientsAddressRepository;

        public PatientsAddressInfrastructure(IPatientsAddressRepository PatientsAddressRepository)
        {
            _patientsAddressRepository = PatientsAddressRepository;
        }

        public async Task<long> Add(List<PatientAddress> patientAddresses)
        {
            return await _patientsAddressRepository.Add(patientAddresses);
        }

        public async Task<long?> Delete(long Id)
        {
            return await _patientsAddressRepository.Delete(Id);
        }

        public async Task<PatientAddress> GetByPatientID(long patientId)
        {
            return await _patientsAddressRepository.GetByPatientID(patientId);
        }

        public async Task<PatientAddress> Update(List<PatientAddress> patientAddress)
        {
            return await _patientsAddressRepository.Update(patientAddress);
        }

        public async Task<List<PatientAddress>> GetListByPatientID(long patientId)
        {
            return await _patientsAddressRepository.GetListByPatientID(patientId);
        }
    }
}
