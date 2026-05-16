using Interface.Models.Medication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEncounterRepository
{
    public interface IMedicationRepository
    {
        Task<List<ORMChartPrescriptionView>> GetChartPrescriptionView(long encounterId);
    }
}
