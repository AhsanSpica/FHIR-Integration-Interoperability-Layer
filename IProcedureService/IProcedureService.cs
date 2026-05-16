using Interface.Models.Procedure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IProcedureService
{
    public interface IProcedureService
    {
        Task<List<CombinedProcedureDTO>> GetCombinedProcedures(long? PatientId = null, long? ResourceId = null, string? tableName = null);
    }
}
