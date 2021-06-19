using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xTask.WebAPI.Exceptions
{
    public class CustomBadRequestExceptions : Exception
    {
        public ModelStateDictionary ModelState;

        public CustomBadRequestExceptions(string message) : base(message)
        {
        }
    }
}
