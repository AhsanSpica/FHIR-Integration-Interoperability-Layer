using Hl7.Fhir.Model;
using Interface.Models.BackgroundServices;
using Interface.Models.InterfaceModels;
using Interface.Models.Medication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEncounterMapper
{
    public interface IMedicationStatementMapper
    {
        Task<CustomBundle> Map(PatientResourceRecords patientId);
        Bundle MapSync(PatientResourceRecords inputs);
    }
}
