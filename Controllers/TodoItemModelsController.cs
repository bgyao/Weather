using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Weather.Models.TodoItems;

namespace Weather.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemModelsController : ControllerBase
    {
        private readonly TodoDbContext _context;

        public TodoItemModelsController(TodoDbContext context)
        {
            _context = context;
        }

        // GET: api/TodoItemModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemModel>>> GetToDoItems()
        {
          if (_context.ToDoItems == null)
          {
              return NotFound();
          }
            return await _context.ToDoItems.ToListAsync();
        }

        // GET: api/TodoItemModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemModel>> GetTodoItemModel(long id)
        {
          if (_context.ToDoItems == null)
          {
              return NotFound();
          }
            var todoItemModel = await _context.ToDoItems.FindAsync(id);

            if (todoItemModel == null)
            {
                return NotFound();
            }

            return todoItemModel;
        }

        // PUT: api/TodoItemModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItemModel(long id, TodoItemModel todoItemModel)
        {
            if (id != todoItemModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItemModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TodoItemModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItemModel>> PostTodoItemModel(TodoItemModel todoItemModel)
        {
          if (_context.ToDoItems == null)
          {
              return Problem("Entity set 'TodoDbContext.ToDoItems'  is null.");
          }
            _context.ToDoItems.Add(todoItemModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItemModel", new { id = todoItemModel.Id }, todoItemModel);
        }

        // DELETE: api/TodoItemModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItemModel(long id)
        {
            if (_context.ToDoItems == null)
            {
                return NotFound();
            }
            var todoItemModel = await _context.ToDoItems.FindAsync(id);
            if (todoItemModel == null)
            {
                return NotFound();
            }

            _context.ToDoItems.Remove(todoItemModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemModelExists(long id)
        {
            return (_context.ToDoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
