using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using xTask.Language.Translations;

namespace xTask.WebAPI.Entities
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldRequired")]
        public string Username { get; set; }
        
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldRequired")]
        public string Password { get; set; }
    }

    public class Register
    {
        [Display(Name = "Email", ResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldMaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldRequired")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "LoginName", ResourceType = typeof(Resource))]
        [StringLength(50, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldMaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldRequired")]        
        public string LoginName { get; set; }

        [StringLength(30, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FieldMinLength", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPass", ResourceType = typeof(Resource))]
        [Compare("Password", ErrorMessageResourceName = "PasswordMatch", ErrorMessageResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; }
    }

    public class AuthenticateResponse
    {

        public AuthenticateResponse()
        {
        }
        //public bool Result { get; set; } = false;
        public string Token { get; set; } = string.Empty;
       // public string Username { get; set; } = string.Empty;

        public DateTime Expires { get; set; }
        //public string Message { get; set; } = string.Empty;
        //public bool IsLockedOut { get; set; } = false;
        //public bool IsNotAllowed { get; set; } = false;
        //public bool RequiresTwoFactor { get; set; } = false;
    }
}
