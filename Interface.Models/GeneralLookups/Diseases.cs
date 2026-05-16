using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.GeneralLookups
{
    public class Diseases : BaseModel
    {
        public long DiseasesId { get; set; }

        public string DiseaseName { get; set; }

        public string DiseaseType { get; set; }

        public long PracticeId { get; set; }

    }
}
