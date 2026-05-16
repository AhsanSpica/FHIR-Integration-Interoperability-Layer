using Hl7.Fhir.Model;
using Interface.Models.BackgroundServices;
using Interface.Models.InterfaceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEncounterMapper
{
    public interface IProblemConditionMapper
    {
    //    Task<CustomBundle> Map(PatientResourceRecords inputs);
        Bundle MapSync(PatientResourceRecords inputs);
    }
}
