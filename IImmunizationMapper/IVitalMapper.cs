using Hl7.Fhir.Model;
using Interface.Models.BackgroundServices;
using Interface.Models.EncounterModels;
using Interface.Models.InterfaceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEncounterMapper
{
    public interface IVitalMapper
    {
        Bundle MapSync(PatientResourceRecords inputs);
      
    }
}
