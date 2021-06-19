using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTask.SharedEntities.DTOs;

namespace xTask.Core.Interfaces
{
    public interface ITaskService
    {
        IQueryable<TaskDTO> AsQueryable();
        Task<TaskDTO> FindAsync(int id);
        Task<TaskDTO> CreateAsync(TaskDTO model);
        Task<TaskDTO> UpdateAsync(TaskDTO model);
        Task DeleteAsync(int id);
        Task SetOrderAsync(int id, int order);
        Task MoveAsync(int id, int todoId);
    }
}
