using Interface.Models.EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEncounterInfrastructure
{
    public interface  IAllergyIntoleranceInfrastructure
    {
        Task<List<ORMChartAllergyView>> GetAllergiesView(long? encounterId = null, long? chartallergiesid = null);
    }
}
