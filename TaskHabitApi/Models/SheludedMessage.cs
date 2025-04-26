using System;

namespace TaskHabitApi.Models
{
    public class ScheduledMessage
    {
        public int Id { get; set; }
        public long ChatId { get; set; } // ID чата получателя
        public string MessageText { get; set; } = string.Empty; // Текст сообщения
        public DateTime ScheduledTime { get; set; } // Время отправки
        public bool IsSent { get; set; } = false; // Флаг отправки
        public bool IsHabit {get; set;} = false; // Задача или привычка
        public int ItemId {get; set;} // Id задачи или привычки
    }
}