using Interface.Models.Medication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEncounterService
{
    public interface IMedicationService
    {
        Task<List<ORMChartPrescriptionView>> GetChartPrescriptionView(long PatientId);
    }
}
