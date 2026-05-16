using Interface.Models.Procedure;
using IProcedureInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcedureService
{
    public class ProcedureService : IProcedureService.IProcedureService
    {
        private readonly IProcedureInfrastructure.IProcedureInfrastructure _Infrastructure;

        public ProcedureService(IProcedureInfrastructure.IProcedureInfrastructure procedureInfrastructure)
        {
            _Infrastructure = procedureInfrastructure;
        }
        public async Task<List<CombinedProcedureDTO>> GetCombinedProcedures(long? PatientId = null, long? ResourceId = null, string? tableName = null)
        {
            return await _Infrastructure.GetCombinedProcedures(PatientId, ResourceId, tableName);
        }
    }
}
