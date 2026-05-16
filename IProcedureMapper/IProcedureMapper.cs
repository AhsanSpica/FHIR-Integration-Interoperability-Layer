using Hl7.Fhir.Model;
using Interface.Models.BackgroundServices;
using Interface.Models.InterfaceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IProcedureMapper
{
    public interface IProcedureMapper
    {
        Bundle MapSync(PatientResourceRecords inputs);
    }
}

