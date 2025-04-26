using Microsoft.AspNetCore.Mvc;
using TaskHabitApi.Models;

namespace TaskHabitApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public IActionResult GetAll(string userId)
        {
            var tasks = _context.Tasks.Where(t => t.UserId == userId).ToList();
            var habits = _context.Habits.Where(h => h.UserId == userId).ToList();
            return Ok(new { tasks, habits });
        }

        [HttpPost("task")]
        public IActionResult AddTask([FromBody] TaskItem task)
        {
            _context.Tasks.Add(task);
            ScheduledMessage msg = new (){Id = _context.ScheduledMessages.Count() + 1, ChatId = Convert.ToInt64(task.UserId), 
            IsSent = false, MessageText = $"Напоминание о задаче: {task.Title}!", ScheduledTime = task.Date, IsHabit = false, ItemId = task.Id};
            _context.ScheduledMessages.Add(msg);
            _context.SaveChanges();
            return Ok(task);
        }

        [HttpPost("habit")]
        public IActionResult AddHabit([FromBody] HabitItem habit)
        {
            _context.Habits.Add(habit);
            DateTime dateTime = DateTime.Now;
            switch (habit.Frequency)
            {
                case "еженедельно":
                    dateTime.AddDays(7);
                break;
                default:
                    dateTime.AddDays(1);
                break;
            }
            ScheduledMessage msg = new (){Id = _context.ScheduledMessages.Count() + 1, ChatId = Convert.ToInt64(habit.UserId), 
            IsSent = false, MessageText = $"Напоминание о привычке: {habit.Name}!", ScheduledTime = dateTime, IsHabit = true, ItemId = habit.Id};
            _context.ScheduledMessages.Add(msg);
            _context.SaveChanges();
            return Ok(habit);
        }

        [HttpPut("task/{id}")]
        public IActionResult ToggleTask(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task == null) return NotFound();
            task.Done = !task.Done;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("task/{id}")]
        public IActionResult DeleteTask(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task == null) return NotFound();
            _context.Tasks.Remove(task);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("habit/{id}")]
        public IActionResult DeleteHabit(int id)
        {
            var habit = _context.Habits.Find(id);
            if (habit == null) return NotFound();
            _context.Habits.Remove(habit);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPost("habit/{id}")]
        public IActionResult AddHabitCount(int id, [FromBody] HabitItem habit)
        {
            var habitItem = _context.Habits.Find(id);
            if (habitItem == null) return NotFound();
            Console.WriteLine(habit.Count);
            habitItem.Count = habit.Count;
            _context.SaveChanges();
            return NoContent();
        }
    }
}