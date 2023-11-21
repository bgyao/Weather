using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Weather.Models.TodoItems;

namespace Weather.Controllers;

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
    public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetToDoItems()
    {
        if (_context.ToDoItems == null)
        {
            return NotFound();
        }

        #region Using Model
        // NOTE: Use TodoItemModel
        //return await _context.ToDoItems.ToListAsync();
        #endregion

        #region Using DTO
        return await _context.ToDoItems
            .Select(x => MapToGetOutputDto(x))
            .ToListAsync();
        #endregion
    }

    // GET: api/TodoItemModels/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDto>> GetTodoItemModel(long id)
    {
        if (_context.ToDoItems == null)
        {
            return NotFound();
        }
        var todoItem = await _context.ToDoItems.FindAsync(id);

        if (todoItem == null)
        {
            return NotFound();
        }

        #region Using Model
        // NOTE: Use TodoItemModel
        //return todoItem
        #endregion

        #region Using DTO
        return MapToGetOutputDto(todoItem);
        #endregion
    }

    // PUT: api/TodoItemModels/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Create>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItemModel(long id, TodoItemDto todoItemDto)
    {
        if (id != todoItemDto.Id)
        {
            return BadRequest();
        }
        // Using Model
        // NOTE: Use TodoItemModel todoItemModel as variable
        //_context.Entry(todoItemModel).State = EntityState.Modified;

        // Using DTO
        var todoItem = await _context.ToDoItems.FindAsync(id);

        if(todoItem == null)
        {
            return NotFound();
        }

        // Using DTO, need to map Name and IsComplete
        todoItem.Name = todoItemDto.Name;
        todoItem.IsComplete = todoItemDto.IsComplete;

        try
        {
            await _context.SaveChangesAsync();
        }
        // NOTE: Long way of implementing try-catch on DbUpdateConcurrencyException
        //catch (DbUpdateConcurrencyException)
        //{
        //    if (!TodoItemModelExists(id))
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        throw;
        //    }
        //}
        catch (DbUpdateConcurrencyException) when (!TodoItemModelExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }
    // </snippet_Create>

    // POST: api/TodoItemModels
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // <snippet_Create>
    [HttpPost]
    public async Task<ActionResult<TodoItemDto>> PostTodoItemModel(TodoItemDto todoItemDto)
    {
        #region Using Model
        // NOTE: Use TodoItemModel
        //if (_context.ToDoItems == null)
        //{
        //    return Problem("Entity set 'TodoDbContext.ToDoItems'  is null.");
        //}
        //  _context.ToDoItems.Add(todoItemModel);
        //  await _context.SaveChangesAsync();

        //  return CreatedAtAction("GetTodoItemModel", new { id = todoItemModel.Id }, todoItemModel);
        #endregion

        #region Using DTO
        var todoItem = new TodoItemModel
        {
            IsComplete = todoItemDto.IsComplete,
            Name = todoItemDto.Name
        };

        _context.ToDoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTodoItemModel),
            new { id = todoItem.Id },
            MapToGetOutputDto(todoItem)
            );

        #endregion
    }
    // </snippet_Create>

    // DELETE: api/TodoItemModels/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItemModel(long id)
    {
        if (_context.ToDoItems == null)
        {
            return NotFound();
        }
        var todoItem = await _context.ToDoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        _context.ToDoItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TodoItemModelExists(long id)
    {
        return (_context.ToDoItems?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    // Maps TodoItemModel to TodoItemDto
    private static TodoItemDto MapToGetOutputDto(TodoItemModel todoItemModel) =>
        new TodoItemDto
        {
            Id = todoItemModel.Id,
            Name = todoItemModel.Name,
            IsComplete = todoItemModel.IsComplete
        };
}
