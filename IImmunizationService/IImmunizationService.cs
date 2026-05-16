using Interface.Models.ImmunizationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IImmunizationService
{
    public interface IImmunizationService
    {
        Task<List<ImmunizationDTO>> GetAllImmunization(long? PatientId = null, long? ImmunizationId = null);
    }
}
