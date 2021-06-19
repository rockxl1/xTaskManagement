using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using xTask.WebAPI.Exceptions;

namespace xTask.WebAPI.Controllers
{
    public class BaseController : Controller
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public void CheckValidation(List<ValidationResult> errors) //todo add this to prevent exceptions
        {
            if (errors.Count > 0)
            {
                foreach (ValidationResult item in errors)
                {
                    ModelState.AddModelError(item.MemberNames.FirstOrDefault(), item.ErrorMessage);
                }
                throw new CustomBadRequestExceptions("Invalid Request") { ModelState = ModelState };
            }
        }
    }
}
