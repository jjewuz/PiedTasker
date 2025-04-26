using System.ComponentModel.DataAnnotations;

namespace TaskHabitApi.Models;

public class TaskItem
{
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool Done { get; set; }
}
