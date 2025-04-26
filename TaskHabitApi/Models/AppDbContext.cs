using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

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

            Console.WriteLine("Настраиваем таблицы");

            // Настройка таблицы TaskItem
            modelBuilder.Entity<TaskItem>(entity => {
                entity.HasKey(ti => ti.Id);
                entity.Property(ti => ti.Title).IsRequired();
                entity.Property(ti => ti.UserId).IsRequired();
                entity.Property(ti => ti.Date).IsRequired();
                entity.Property(ti => ti.Done).IsRequired();
            });

            modelBuilder.Entity<HabitItem>(entity => {
                entity.HasKey(hi => hi.Id);
                entity.Property(hi => hi.Count).IsRequired();
                entity.Property(hi => hi.UserId).IsRequired();
                entity.Property(hi => hi.Frequency).IsRequired();
                entity.Property(hi => hi.Count).IsRequired();
            });

            modelBuilder.Entity<ScheduledMessage>(entity => {
                entity.HasKey(sm => sm.Id);
                entity.Property(sm => sm.ChatId).IsRequired();
                entity.Property(sm => sm.MessageText).IsRequired();
                entity.Property(sm => sm.ScheduledTime).IsRequired();
                entity.Property(sm => sm.IsSent).IsRequired();
                entity.Property(sm => sm.IsHabit).IsRequired();
                entity.Property(sm => sm.ItemId).IsRequired();
            });
        }
    }
}