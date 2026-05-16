using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hl7.Fhir.Model.Bundle;

namespace Interface.Models.InterfaceModels
{
    public class BundleR4
    {
       public Identifier Id { get; set; }
        public BundleType Type { get; set; }
        public List<Bundle.EntryComponent> Entry { get; set; }
        public List<Bundle.LinkComponent> Link { get; set; }
        public string ResourceType { get; set; } = "Bundle";
        // string ResourceType { get; set; }
    }
}
