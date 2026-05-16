using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.InterfaceModels
{
    public class CarePlanCore : CarePlan
    {

        public Narrative Text { get; set; }

        public Code Status { get; set; }

        public Code Intent { get; set; }

        public List<CodeableConcept> Category { get; set; }

        public ResourceReference Subject { get; set; }
    }
}
