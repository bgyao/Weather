using Microsoft.EntityFrameworkCore;

namespace Weather.Models.TodoItems;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options)
    {

    }

    public DbSet<TodoItemModel> ToDoItems { get; set; }
}
