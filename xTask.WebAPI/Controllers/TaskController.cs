using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using xTask.Core.Interfaces;
using xTask.SharedEntities.DTOs;

namespace xTask.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : BaseController
    {
        private ITaskService _service;

        public TaskController(ITaskService service)
        {
            _service = service;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult<List<TaskDTO>>> Get(int? todoId) //todo, put pagination (total_rows, skip, etc, or ODATA :))
        {
            var query = _service.AsQueryable();

            if (todoId.HasValue)
            {
                query = query.Where(x => x.TodoID == todoId.Value);
            }
            return Ok(await query.ToListAsync());
        }

        [HttpGet("{id}")]
        public async System.Threading.Tasks.Task<ActionResult<TaskDTO>> GetById(int id)
        {
            return Ok(await _service.FindAsync(id));
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult<TaskDTO>> Create([FromBody] TaskDTO model)
        {
            return Ok(await _service.CreateAsync(model));
        }

        [HttpPut]
        public async System.Threading.Tasks.Task<ActionResult<TaskDTO>> Update([FromBody] TaskDTO model)
        {
            return Ok(await _service.UpdateAsync(model));
        }

        [HttpPost("move/{id}/{todoid}")]
        public async System.Threading.Tasks.Task<ActionResult> Move(int id, int todoid)
        {
            await _service.MoveAsync(id, todoid);
            return Ok();
        }

        [HttpPost("reorder/{id}/{order}")]
        public async System.Threading.Tasks.Task<ActionResult> ReOrder(int id, int order)
        {
            await _service.SetOrderAsync(id, order);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();

        }
    }
}
