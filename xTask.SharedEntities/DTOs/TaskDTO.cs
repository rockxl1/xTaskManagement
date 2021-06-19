using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTask.Language.Translations;

namespace xTask.SharedEntities.DTOs
{
    public class TaskDTO: BaseEntityDTO
    {
        [Display(Name = "Title", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldRequired")]
        [StringLength(64, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldMaxLength")]
        public string Title { get; set; }

        [Display(Name = "Notes", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldRequired")]
        [StringLength(256, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldMaxLength")]
        public string Notes { get; set; }

        [Display(Name = "DueDate", ResourceType = typeof(Resource))]
        public DateTime? DueDate { get; set; }

        [Display(Name = "Order", ResourceType = typeof(Resource))]
        public int Order { get;  set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldRequired")]
        public int TodoID { get; set; }
        public TodoDTO Todo { get; set; }
    }
}
