using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using CQRS.Commands;
using Newtonsoft.Json;

namespace CQRS.Services
{
    public class CommandQueuingService : ICommandQueuingService
    {
        private readonly QueueClient _queueClient;
        private readonly Task _initTask;

        public CommandQueuingService()
        {
            _queueClient = new QueueClient("UseDevelopmentStorage=true", "command-queue");

            _initTask = _queueClient.CreateIfNotExistsAsync();
        }

        public async Task QueueAsync(ICommand command)
        {
            await _initTask;

            var payload = JsonConvert.SerializeObject(command, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            var base64Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(payload));

            await _queueClient.SendMessageAsync(base64Data);
        }
    }
}
