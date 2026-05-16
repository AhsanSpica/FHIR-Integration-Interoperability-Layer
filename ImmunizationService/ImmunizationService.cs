using Hl7.Fhir.Model;
using IImmunizationInfrastructure;
using IImmunizationService;
using Interface.Models.ImmunizationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmunizationService
{
    public class ImmunizationService : IImmunizationService.IImmunizationService
    {
        private readonly IImmunizationInfrastructure.IImmunizationInfrastructure _immunizationInfrastructure;
        public ImmunizationService(IImmunizationInfrastructure.IImmunizationInfrastructure immunizationInfrastructure)
        {
            _immunizationInfrastructure = immunizationInfrastructure;
        } 

        public async Task<List<ImmunizationDTO>> GetAllImmunization(long? patientId = null, long? ImmunizationId = null)
        {
            return await _immunizationInfrastructure.GetAllImmunization(patientId,ImmunizationId);
        }
    }
}
