using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTask.Core.Extensions;
using xTask.Core.Interfaces;
using xTask.Domain.Entities;
using xTask.SharedEntities.DTOs;

namespace xTask.Core.Services
{
    /// <summary>
    /// Business Logic Layer Here for creating TODO Lists
    /// </summary>
    public class TodoService : ITodoService
    {
        private IRepository<Todo> _todoRep { get; set; }
        private IUser _user { get; set; }

        public TodoService(IRepository<Todo> todoService, IUser user)
        {
            _todoRep = todoService;
            _user = user;
        }

        public IQueryable<TodoDTO> AsQueryable()
        {
            string username = _user.GetUserName(); //security here

            return (from cursor in _todoRep.AsQueryable()
                    where cursor.CreatedBy == username
                    select new TodoDTO()
                    {
                        ID = cursor.ID,
                        CreatedBy = cursor.CreatedBy,
                        ModifiedBy = cursor.ModifiedBy,
                        CreatedOn = cursor.CreatedOn,
                        ModifiedOn = cursor.ModifiedOn,
                        Name = cursor.Name
                    });
        }

        public async Task<TodoDTO> FindAsync(int id)
        {
            return await System.Threading.Tasks.Task.FromResult(AsQueryable().Where(x => x.ID == id).FirstOrDefault());
        }

        public async Task<TodoDTO> CreateAsync(TodoDTO model)
        {
            Todo todo = new Todo()
            {
                Name = model.Name
            };

            todo = await _todoRep.AddAsync(todo);

            return new TodoDTO()
            {
                ID = todo.ID,
                CreatedBy = todo.CreatedBy,
                ModifiedBy = todo.ModifiedBy,
                CreatedOn = todo.CreatedOn,
                ModifiedOn = todo.ModifiedOn,
                Name = todo.Name
            };

        }

        public async Task<TodoDTO> UpdateAsync(TodoDTO model)
        {
            Todo todo = new Todo()
            {
                Name = model.Name,
                ID = model.ID
            };

            await _todoRep.UpdateAsync(todo);

            return new TodoDTO()
            {
                ID = todo.ID,
                CreatedBy = todo.CreatedBy,
                ModifiedBy = todo.ModifiedBy,
                CreatedOn = todo.CreatedOn,
                ModifiedOn = todo.ModifiedOn,
                Name = todo.Name
            };
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            await _todoRep.DeleteAsync(id);
        }


    }
}
