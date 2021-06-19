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
    public class TodoController : BaseController
    {
        private ITodoService _service;

        public TodoController(ITodoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult<List<TodoDTO>>> Get() //todo, put pagination (total_rows, skip, etc, or ODATA :))
        {
            return Ok(await _service.AsQueryable().ToListAsync());
        }

        [HttpGet("{id}")]
        public async System.Threading.Tasks.Task<ActionResult<TodoDTO>> GetById(int id)
        {
            return Ok(await _service.FindAsync(id));
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult<TodoDTO>> Create([FromBody] TodoDTO model)
        {
            return Ok(await _service.CreateAsync(model));
        }

        [HttpPut]
        public async System.Threading.Tasks.Task<ActionResult<TodoDTO>> Update([FromBody] TodoDTO model)
        {
            return Ok(await _service.UpdateAsync(model));
        }

        [HttpDelete("{id}")]
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();

        }
    }
}
