using Microsoft.EntityFrameworkCore;

namespace TodoApii.Models
{
    public class TimetableDbContext : DbContext
    {
        public TimetableDbContext(DbContextOptions<TimetableDbContext> options) : base(options) { }

        public DbSet<Timetable_User> TimetableUsers { get; set; }
    }
}
