using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTask.SharedEntities.DTOs;

namespace xTask.Core.Interfaces
{
    public interface ITodoService
    {
        IQueryable<TodoDTO> AsQueryable();
        Task<TodoDTO> FindAsync(int id);
        Task<TodoDTO> CreateAsync(TodoDTO model);
        Task<TodoDTO> UpdateAsync(TodoDTO model);
        Task DeleteAsync(int id);
    }
}
