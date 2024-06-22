using hngstageone.Entities;
using Microsoft.EntityFrameworkCore;

namespace hngstageone.Data
{
    public class TodoDb : DbContext
    {
        public TodoDb(DbContextOptions<TodoDb> options) : base(options) { }

        public DbSet<Todo> Todos => Set<Todo>();
    }
}
