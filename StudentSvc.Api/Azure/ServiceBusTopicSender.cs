using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace StudentSvc.Api.Azure
{
    public class ServiceBusTopicSender
    {
        private readonly ILogger<ServiceBusSender> _logger;
        private readonly TopicSettings _topicSettings;

        public ServiceBusTopicSender(ILogger<ServiceBusSender> logger, IOptions<TopicSettings> options)
        {
            this._logger = logger;
            this._topicSettings = options.Value;
        }

        public async Task SendMessageAsync<T>(T payload) where T : class
        {
            ServiceBusClient client = new ServiceBusClient(_topicSettings.ConnectionString);
            ServiceBusSender sender = client.CreateSender(_topicSettings.TopicName);

            try
            {
                string json = JsonConvert.SerializeObject(payload);
                ServiceBusMessage message = new ServiceBusMessage(json);
                await sender.SendMessageAsync(message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message, payload);
            }
            finally
            {
                await sender.DisposeAsync().ConfigureAwait(false);
                await client.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}
