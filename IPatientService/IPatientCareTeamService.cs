using Interface.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPatientService
{
    public interface IPatientCareTeamService
    {
        Task<List<PatientCareTeam>> GetListByPatientID(long? patienId = null, long? careTeamId =null);
        Task<List<PatientCareTeamMember>> GetMemeberListByPatientID(long patienId);
    }
}
