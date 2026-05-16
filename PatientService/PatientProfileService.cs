using Interface.Models.Patients;
 using IPatientService;
using IPatientsInfrastructure;
using IPatientsInfrasturcture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientService
{
    public class PatientProfileService : IPatientProfileService
    {
        private readonly IPatientsInfoInfrastructure _patientsInfoInfrastructure;
        private readonly IPatientsAddressInfrastructure _patientsAddressInfrastructure;
        private readonly IPatientsPhoneInfrastructure _patientsPhoneInfrastructure;
        private readonly IPatientEmergencyContactInfrastructure _patientEmergencyContactInfrastructure;
        private readonly IPatientRaceInfrastructure _patientRaceInfrastructure;
        private readonly IPatientEthnicityInfrastructure _patientEthnicityInfrastructure;
        private readonly IPatientCareTeamInfrastructure _patientCareTeamInfrastructure;
        private readonly IPatientCareTeamMemberInfrastructure _patientCareTeamMemberInfrastructure;
        public PatientProfileService(IPatientsInfoInfrastructure patientsInfoInfrastructure,
           IPatientsAddressInfrastructure patientsAddressInfrastructure,
           
           IPatientsPhoneInfrastructure patientsPhoneInfrastructure,
           IPatientEmergencyContactInfrastructure patientEmergencyContactInfrastructure,
           IPatientRaceInfrastructure patientRaceInfrastructure,
           IPatientEthnicityInfrastructure patientEthnicityInfrastructure,
            IPatientCareTeamInfrastructure patientCareTeamInfrastructure,
            IPatientCareTeamMemberInfrastructure patientCareTeamMemberInfrastructure
           )
        {
            _patientsInfoInfrastructure = patientsInfoInfrastructure;
            _patientsAddressInfrastructure = patientsAddressInfrastructure;
            _patientsPhoneInfrastructure = patientsPhoneInfrastructure;
            _patientEmergencyContactInfrastructure = patientEmergencyContactInfrastructure;
            _patientRaceInfrastructure = patientRaceInfrastructure;
            _patientEthnicityInfrastructure = patientEthnicityInfrastructure;
            _patientCareTeamInfrastructure = patientCareTeamInfrastructure;
            _patientCareTeamMemberInfrastructure = patientCareTeamMemberInfrastructure;
        }
        public async Task<PatientInfo> GetByID(long PatientId, long PracticeId)
        {

            var patientResult = await _patientsInfoInfrastructure.GetPatientInfoById(PatientId, PracticeId);

            if (patientResult == null || patientResult.Id <= 0)
            {
                return null!;
            }
            patientResult.PatientPhones = await _patientsPhoneInfrastructure.GetListByPatientID(PatientId);
            patientResult.PatientCareTeams = await _patientCareTeamInfrastructure.GetListByPatientID(PatientId);
            patientResult.PatientCareTeamMembers = await _patientCareTeamMemberInfrastructure.GetListByPatientID(PatientId);
           // patientResult.PatientInsurances = await _patientInsuranceInfrastructure.GetListByPatientID(PatientId);
          //  patientResult.PatientPreferredPharmacies = await _patientPreferredPharmacyInfrastructure.GetListByPatientID(PatientId);
            patientResult.PatientEmergencyContacts = await _patientEmergencyContactInfrastructure.GetListByPatientID(PatientId);
          //  patientResult.PatientDefaultAndReferringProviders = await _patientDefaultAndReferringProviderInfrastructure.GetListByPatientID(PatientId);
            patientResult.patientRaces = await _patientRaceInfrastructure.GetListByPatientID(PatientId);
            patientResult.patientEthnicities = await _patientEthnicityInfrastructure.GetListByPatientID(PatientId);
            patientResult.PatientAddresses = await _patientsAddressInfrastructure.GetListByPatientID(PatientId);

            return patientResult;
        }
    }
}
