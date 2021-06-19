using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTask.Domain.Interfaces;

namespace xTask.Domain.Entities
{
    public class Todo: BaseEntity, IBaseAggregator
    {
        public string Name { get; set; }
        public List<Task> Taks { get; set; }
    }
}
