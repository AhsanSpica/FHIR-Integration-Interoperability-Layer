using Interface.Models.EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEncounterService
{
    public interface IProblemConditionService
    {
        Task<List<PatientProblem>> GetPatientProblemById(long? patientId = null, long? problemId = null, string? tableName = null, long? encounterId=null);
    }
}
