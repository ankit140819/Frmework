using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Frmework.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly ConcurrentQueue<(string User, string Message, DateTime Time)> _history = new();
        private const int MaxHistory = 50;

        public override async Task OnConnectedAsync()
        {
            // Send recent history to the caller
            var items = _history.ToArray();
            await Clients.Caller.SendAsync("LoadHistory", items);
            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            var entry = (User: string.IsNullOrWhiteSpace(user) ? "Guest" : user, Message: message ?? string.Empty, Time: DateTime.UtcNow);
            _history.Enqueue(entry);
            while (_history.Count > MaxHistory)
            {
                _history.TryDequeue(out _);
            }

            await Clients.All.SendAsync("ReceiveMessage", entry.User, entry.Message, entry.Time.ToString("s"));

            // Simulate a quick automated confirmation from support (non-blocking)
            _ = Task.Run(async () =>
            {
                await Task.Delay(1200);
                await Clients.Caller.SendAsync("ReceiveMessage", "Support", "Thanks for your message — an agent will join shortly.", DateTime.UtcNow.ToString("s"));
            });
        }
    }
}
