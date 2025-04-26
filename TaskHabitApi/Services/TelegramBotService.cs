using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TaskHabitApi.Models;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace TaskHabitApi.Services
{
    public class TelegramBotService : IHostedService
    {
        private readonly ITelegramBotClient _botClient;
        private AppDbContext _dbContext;
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private Timer _timer;

        public TelegramBotService(IDbContextFactory<AppDbContext> dbContext)
        {
            // Получаем токен из переменной окружения
            var telegramBotToken = Environment.GetEnvironmentVariable("REACT_APP_BOT_TOKEN");
            if (string.IsNullOrEmpty(telegramBotToken))
            {
                throw new InvalidOperationException("Telegram bot token is not set in the environment variables.");
            }

            _botClient = new TelegramBotClient(telegramBotToken);
            _dbContextFactory = dbContext;
            _dbContext = _dbContextFactory.CreateDbContext(); 
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting Telegram Bot...");

            // Запускаем планировщик задач
            _timer = new Timer(async _ => await CheckAndSendMessages(), null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Stopping Telegram Bot...");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private async Task CheckAndSendMessages()
        {
            if (_dbContext == null)
            {
                _dbContext = _dbContextFactory.CreateDbContext(); 
            }

            _dbContext.Database.EnsureCreated();

            var now = DateTime.Now;

            // Получаем сообщения, которые нужно отправить
            var messagesToSend = await _dbContext.ScheduledMessages
                .Where(sm => !sm.IsSent && sm.ScheduledTime <= now)
                .ToListAsync();

            foreach (var message in messagesToSend)
            {
                try
                {
                    // Отправляем сообщение
                    await _botClient.SendMessage(
                        chatId: message.ChatId,
                        text: message.MessageText,
                        parseMode: ParseMode.Markdown
                    );

                    if (!message.IsHabit)
                    {
                        // Помечаем сообщение как отправленное
                        message.IsSent = true;
                    }
                    else 
                    {
                        var habit = _dbContext.Habits.Find(message.ItemId);
                        switch (habit.Frequency) //Resend later
                        {
                            case "еженедельная":
                                message.ScheduledTime = message.ScheduledTime.AddDays(7);
                            break;
                            default:
                                message.ScheduledTime = message.ScheduledTime.AddDays(1);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending message to chat {message.ChatId}: {ex.Message}");
                }
            }

            // Сохраняем изменения в базе данных
            await _dbContext.SaveChangesAsync();
        }
    }
}