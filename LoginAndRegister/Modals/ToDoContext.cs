using Microsoft.EntityFrameworkCore;

namespace LoginAndRegister.Modals
{
    public class ToDoContext:DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
        {

        }
        public DbSet<LocalUser> LocalUsers { get; set; }
    }
}
