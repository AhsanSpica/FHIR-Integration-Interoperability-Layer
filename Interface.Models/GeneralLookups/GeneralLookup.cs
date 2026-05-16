using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.GeneralLookups
{
    public class GeneralLookup : BaseModel
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public long PracticeId { get; set; }
        public bool IsActive { get; set; }
        public long ParentGeneralLookupId { get; set; }
        public int Precedence { get; set; }

    }
}
