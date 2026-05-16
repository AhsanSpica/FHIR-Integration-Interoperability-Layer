using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHR.Models.Patients
{
    public class TaskUser : BaseModel
    {
        public long Id { get; set; }
        public long TaskId { get; set; }
        public long UserId { get; set; }
        public bool? IsActive { get; set; }
    }

    public class TaskUserDto : BaseModel
    {
        public long Id { get; set; }
        public long TaskId { get; set; }
        public long UserId { get; set; }
        public bool? IsActive { get; set; }

        //Other Optional Fields or List will Come here onwards
    }
}
