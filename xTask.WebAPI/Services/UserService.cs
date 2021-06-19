using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xTask.Core.Interfaces;

namespace xTask.WebAPI.Services
{
    public class UserService : IUser
    {
        private IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserName()
        {
            return _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
        }
    }
}
