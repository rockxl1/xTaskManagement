using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xTask.Domain.Entities
{
    /// <summary>
    /// Base Entite
    /// </summary>
    public class BaseEntity
    {
        public BaseEntity()
        {
            CreatedOn = DateTime.Now;
            ModifiedOn = DateTime.Now;
        }

        public int ID { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

    }
}
