using Microsoft.EntityFrameworkCore;

namespace DomainModel
{
    public class StackOverflowDbContext:DbContext
    {
        public StackOverflowDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Question>? Questions { get; set; }
        public DbSet<Answer>? Answers { get; set; }
        public DbSet<Vote>? Votes { get; set; }
    }
}
