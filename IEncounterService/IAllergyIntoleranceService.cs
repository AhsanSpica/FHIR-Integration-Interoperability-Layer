using Interface.Models.EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEncounterService
{
    public interface IAllergyIntoleranceService
    {
        Task<List<ORMChartAllergyView>> GetAllergiesView(long patientId);
        Task<List<ORMChartAllergyView>> GetAllergiesViewSingular(long? chartallergiesid = null);
    }
}
