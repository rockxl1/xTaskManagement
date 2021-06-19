using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using xTask.Language.Translations;

namespace xTask.SharedEntities.DTOs
{
    public class TodoDTO : BaseEntityDTO
    {
        [Display(Name = "Name", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldRequired")]
        [StringLength(64, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldMaxLength")]
        public string Name { get; set; }
    }
}
