using Hl7.Fhir.Model;
using Interface.Models.BackgroundServices;
using Interface.Models.InterfaceModels;
using Interface.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPatientMapper
{
    public interface IPatientCareTeamMapper
    {
        Bundle MapSync(PatientResourceRecords inputs);

    }
}
