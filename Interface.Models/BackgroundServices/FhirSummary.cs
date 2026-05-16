using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.BackgroundServices
{
    public class FhirSummary
    { public string ResourceType { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public int Total {  get; set; }
  
    }
}
