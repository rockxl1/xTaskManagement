using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xTask.Core.Interfaces
{
    /// <summary>
    /// Contract for hold the current user profile
    /// </summary>
    public interface IUser
    {
        string GetUserName();
    }
}
