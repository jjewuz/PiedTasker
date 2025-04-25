using Microsoft.EntityFrameworkCore;

namespace TaskHabitApi.Models
{
    public class AppDbContext : DbContext
    {
    
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<HabitItem> Habits { get; set; }
        
        public DbSet<ScheduledMessage> ScheduledMessages { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка таблицы ScheduledMessages
            modelBuilder.Entity<ScheduledMessage>()
                .HasKey(sm => sm.Id);

            modelBuilder.Entity<ScheduledMessage>()
                .Property(sm => sm.ChatId)
                .IsRequired();

            modelBuilder.Entity<ScheduledMessage>()
                .Property(sm => sm.MessageText)
                .IsRequired();

            modelBuilder.Entity<ScheduledMessage>()
                .Property(sm => sm.ScheduledTime)
                .IsRequired();
        }
    }
}