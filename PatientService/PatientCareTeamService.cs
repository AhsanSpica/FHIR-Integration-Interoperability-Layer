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
    public class PatientCareTeamService : IPatientCareTeamService
    {
        private readonly IPatientCareTeamInfrastructure _patientCareTeamInfrastructure;
        private readonly IPatientCareTeamMemberInfrastructure _patientCareTeamMemberInfrastructure;

        public PatientCareTeamService(IPatientCareTeamMemberInfrastructure patientCareTeamMemberInfrastructure, IPatientCareTeamInfrastructure patientCareTeamInfrastructure)
        {
            _patientCareTeamInfrastructure = patientCareTeamInfrastructure;
            _patientCareTeamMemberInfrastructure = patientCareTeamMemberInfrastructure;
        }
       public async Task<List<PatientCareTeam>> GetListByPatientID(long? patienId = null, long? careTeamId = null)
        {

            return await _patientCareTeamInfrastructure.GetListByPatientID(patienId,careTeamId);
        }
        public async Task<List<PatientCareTeamMember>> GetMemeberListByPatientID(long patienId)
        {

            return await _patientCareTeamMemberInfrastructure.GetListByPatientID(patienId);
        }
    }
}
