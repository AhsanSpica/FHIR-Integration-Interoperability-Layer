using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.Common
{
    
        public class BaseModel
        {

            public string? CreatedBy { get; set; }
            public string? UpdatedBy { get; set; }
            public DateTimeOffset? CreatedAt { get; set; }
            public DateTimeOffset? UpdatedAt { get; set; }
            public bool? IsDeleted { get; set; }
            public int TotalRows { get; set; } = 0;
            public int TotalCount { get; set; } = 0;


        //public BaseModel()
        //{
        //    IsDeleted = false;
        //}

    }
    public class BaseEntity
    {
        public DateTimeOffset CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }

}
