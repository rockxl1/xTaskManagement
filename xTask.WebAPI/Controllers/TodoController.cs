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
        public async System.Threading.Tasks.Task<ActionResult<PaginatedResultDTO<TodoDTO>>> Get(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10)
        {
            if (pageSize > 100) pageSize = 100; // Limit page size to prevent abuse
            if (page < 1) page = 1;

            return Ok(await _service.GetPaginatedAsync(page, pageSize));
        }

        [HttpGet("all")]
        public async System.Threading.Tasks.Task<ActionResult<List<TodoDTO>>> GetAll() //Legacy endpoint for backward compatibility
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
