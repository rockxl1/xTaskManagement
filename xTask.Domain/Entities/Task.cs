using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTask.Domain.Interfaces;

namespace xTask.Domain.Entities
{
    public class Task: BaseEntity, IBaseAggregator
    {
        public string Title { get; set; }
        public string Notes { get; set; }
        public DateTime? DueDate { get; set; }
        public int Order { get; set; }
        public int TotalMoved { get; set; }
        public int TodoID { get; set; }
        public Todo Todo { get; set; }

    }
}
