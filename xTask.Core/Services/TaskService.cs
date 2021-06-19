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

            if (_todoService.FindAsync(model.TodoID) == null)
            {
                throw new UnauthorizedAccessException("Invalid TodoID");
            }

            Task task = new Task()
            {
                Title = model.Title,
                Notes = model.Notes,
                DueDate = model.DueDate,
                Order = _taskRep.AsQueryable().Where(x=>x.TodoID == model.TodoID).Count() + 1, //to the end of list
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

            int totalMoved = _taskRep.AsQueryable().Where(x => x.ID == model.ID).Select(x => x.TotalMoved).First();

            Task task = new Task()
            {
                ID = model.ID,
                Title = model.Title,
                Notes = model.Notes,
                DueDate = model.DueDate,
                TotalMoved = totalMoved,
                Order = actual.Order, //set the old value. Update order is other method
                TodoID = actual.TodoID //set the old value. transfer task is other method
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

            if (_todoService.FindAsync(id) == null)
            {
                throw new UnauthorizedAccessException("Invalid TodoID");
            }

            //2º Get the actual task
            TaskDTO actual = AsQueryable().Where(x => x.ID == id).FirstOrDefault();
            
            if (actual == null)
            {
                throw new UnauthorizedAccessException();
            }


            actual.TodoID = todoId;

            int totalMoved = _taskRep.AsQueryable().Where(x => x.ID == id).Select(x => x.TotalMoved).First();

            await _taskRep.UpdateAsync(new Task()
            {
                ID = actual.ID,
                Title = actual.Title,
                Notes = actual.Notes,
                DueDate = actual.DueDate,
                Order = actual.Order,
                TotalMoved= totalMoved+1,
                TodoID = actual.TodoID
            });
        }
    }
}
