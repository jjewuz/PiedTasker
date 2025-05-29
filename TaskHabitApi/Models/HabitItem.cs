using System.ComponentModel.DataAnnotations;

namespace TaskHabitApi.Models
{
    public class HabitItem
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Frequency { get; set; } = "ежедневно";
        public int Count { get; set; }
        public ScheduledMessage? LinkedMsg { get; set; } = null;
    }
}