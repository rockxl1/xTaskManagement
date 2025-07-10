using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xTask.Core.Interfaces;
using xTask.Domain.Entities;
using xTask.SharedEntities.DTOs;

namespace xTask.Core.Services
{
    /// <summary>
    /// Business Logic Layer Here for creating Task Inside Todo Lists
    /// </summary>
    public class TaskService: ITaskService
    {
        private IRepository<Task> _taskRep { get; set; }
        private ITodoService _todoService { get; set; }
        private IUser _user { get; set; }

        public TaskService(IRepository<Task> todoService,  ITodoService todoservice, IUser user)
        {
            _taskRep = todoService;
            _user = user;
            _todoService = todoservice;
        }

        public IQueryable<TaskDTO> AsQueryable()
        {
            string username = _user.GetUserName();

            return (from cursor in _taskRep.AsQueryable()
                    where cursor.CreatedBy == username
                    select new TaskDTO()
                    {
                        ID = cursor.ID,
                        CreatedBy = cursor.CreatedBy,
                        ModifiedBy = cursor.ModifiedBy,
                        CreatedOn = cursor.CreatedOn,
                        ModifiedOn = cursor.ModifiedOn,
                        DueDate = cursor.DueDate,
                        Notes = cursor.Notes,
                        Order = cursor.Order,
                        Title = cursor.Title,
                        TodoID = cursor.TodoID
                    });
        }

        public async System.Threading.Tasks.Task<TaskDTO> FindAsync(int id)
        {
            return await System.Threading.Tasks.Task.FromResult(AsQueryable().Where(x => x.ID == id).FirstOrDefault());
        }

        public async System.Threading.Tasks.Task<TaskDTO> CreateAsync(TaskDTO model)
        {
           
            //1º check if the todoID is for this user
            if (await _todoService.FindAsync(model.TodoID) == null)
            {
                throw new UnauthorizedAccessException("Invalid TodoID");
            }

            // Optimized: Get the count in a more efficient way
            int nextOrder = _taskRep.AsQueryable()
                .Where(x => x.TodoID == model.TodoID)
                .Count() + 1;

            Task task = new Task()
            {
                Title = model.Title,
                Notes = model.Notes,
                DueDate = model.DueDate,
                Order = nextOrder, //to the end of list
                TodoID = model.TodoID,
                TotalMoved = 0,
            };

            task = await _taskRep.AddAsync(task);

            return new TaskDTO()
            {
                ID = task.ID,
                CreatedBy = task.CreatedBy,
                ModifiedBy = task.ModifiedBy,
                CreatedOn = task.CreatedOn,
                ModifiedOn = task.ModifiedOn,
                Title = task.Title,
                DueDate = task.DueDate,
                Notes = task.Notes,
                Order = task.Order,
                TodoID = task.TodoID
            };
        }

        public async System.Threading.Tasks.Task<TaskDTO> UpdateAsync(TaskDTO model)
        {
            //1º Get the actual task
            TaskDTO actual = AsQueryable().Where(x => x.ID == model.ID).FirstOrDefault();

            if(actual == null)
            {
                throw new UnauthorizedAccessException();
            }

            // Optimized: Get totalMoved in a single query instead of separate query
            var taskEntity = _taskRep.AsQueryable().Where(x => x.ID == model.ID)
                .Select(x => new { x.TotalMoved, x.Order, x.TodoID })
                .FirstOrDefault();

            if (taskEntity == null)
            {
                throw new UnauthorizedAccessException();
            }

            Task task = new Task()
            {
                ID = model.ID,
                Title = model.Title,
                Notes = model.Notes,
                DueDate = model.DueDate,
                TotalMoved = taskEntity.TotalMoved,
                Order = taskEntity.Order, //set the old value. Update order is other method
                TodoID = taskEntity.TodoID //set the old value. transfer task is other method
            };

            task = await _taskRep.UpdateAsync(task);

            return new TaskDTO()
            {
                ID = task.ID,
                CreatedBy = task.CreatedBy,
                ModifiedBy = task.ModifiedBy,
                CreatedOn = task.CreatedOn,
                ModifiedOn = task.ModifiedOn,
                Title = task.Title,
                DueDate = task.DueDate,
                Notes = task.Notes,
                Order = task.Order,
                TodoID = task.TodoID
            };
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id)
        {
            await _taskRep.DeleteAsync(id);
        }

        public async System.Threading.Tasks.Task SetOrderAsync(int id, int order)
        {
            //1º Get the actual task
            TaskDTO actual = AsQueryable().Where(x => x.ID == id).FirstOrDefault();

            if (actual == null)
            {
                throw new UnauthorizedAccessException();
            }

            actual.Order = order; //todo: make a shift to other records?

            await _taskRep.UpdateAsync(new Task()
            {
                ID = actual.ID,
                Title = actual.Title,
                Notes = actual.Notes,
                DueDate = actual.DueDate,
                Order = actual.Order, 
                TodoID = actual.TodoID 
            });
        }

        public async System.Threading.Tasks.Task MoveAsync(int id, int todoId)
        {
            //1º check if the todoID is for this user

            if (_todoService.FindAsync(todoId) == null)
            {
                throw new UnauthorizedAccessException("Invalid TodoID");
            }

            //2º Get the actual task - optimized to get all needed data in one query
            var taskData = _taskRep.AsQueryable()
                .Where(x => x.ID == id && x.CreatedBy == _user.GetUserName())
                .Select(x => new { x.ID, x.Title, x.Notes, x.DueDate, x.Order, x.TotalMoved })
                .FirstOrDefault();
            
            if (taskData == null)
            {
                throw new UnauthorizedAccessException();
            }

            await _taskRep.UpdateAsync(new Task()
            {
                ID = taskData.ID,
                Title = taskData.Title,
                Notes = taskData.Notes,
                DueDate = taskData.DueDate,
                Order = taskData.Order,
                TotalMoved = taskData.TotalMoved + 1,
                TodoID = todoId
            });
        }

        public async System.Threading.Tasks.Task<PaginatedResultDTO<TaskDTO>> GetPaginatedAsync(int? todoId, int page = 1, int pageSize = 10)
        {
            var query = AsQueryable();

            if (todoId.HasValue)
            {
                query = query.Where(x => x.TodoID == todoId.Value);
            }

            var totalCount = await System.Threading.Tasks.Task.FromResult(query.Count());
            var skip = (page - 1) * pageSize;
            
            var data = await System.Threading.Tasks.Task.FromResult(query
                .OrderBy(x => x.Order)
                .ThenBy(x => x.CreatedOn)
                .Skip(skip)
                .Take(pageSize)
                .ToList());

            return new PaginatedResultDTO<TaskDTO>
            {
                Data = data,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
