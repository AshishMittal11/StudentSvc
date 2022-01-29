using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace StudentSvc.Api.Azure
{
    public class ServiceBusConsumer : IServiceBusConsumer
    {
        private readonly TopicSettings _topicSettings;
        private readonly ILogger<ServiceBusConsumer> _logger;
        private readonly ServiceBusClient _client;
        private readonly IProcessData _processData;
        private ServiceBusProcessor _processor;

        public ServiceBusConsumer(IOptions<TopicSettings> options, ILogger<ServiceBusConsumer> logger, IProcessData processData)
        {
            this._topicSettings = options.Value;
            this._logger = logger;
            this._client = new ServiceBusClient(_topicSettings.ConnectionString);
            this._processData = processData;
        }

        public async Task RegisterOnMessageHandlerAndReceiveMessages()
        {
            ServiceBusProcessorOptions _serviceBusProcessorOptions = new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false,
            };

            _processor = _client.CreateProcessor(_topicSettings.TopicName, _topicSettings.Subscription, _serviceBusProcessorOptions);
            _processor.ProcessMessageAsync += ProcessMessagesAsync;
            _processor.ProcessErrorAsync += ProcessErrorAsync;
            await _processor.StartProcessingAsync().ConfigureAwait(false);
        }


        private Task ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            _logger.LogError(arg.Exception, "Message handler encountered an exception");
            _logger.LogDebug($"- ErrorSource: {arg.ErrorSource}");
            _logger.LogDebug($"- Entity Path: {arg.EntityPath}");
            _logger.LogDebug($"- FullyQualifiedNamespace: {arg.FullyQualifiedNamespace}");

            return Task.CompletedTask;
        }

        private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
        {
            var myPayload = args.Message.Body.ToObjectFromJson<MyPayload>();
            await _processData.Process(myPayload).ConfigureAwait(false);
            await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
        }

        public async ValueTask DisposeAsync()
        {
            if (_processor != null)
            {
                await _processor.DisposeAsync().ConfigureAwait(false);
            }

            if (_client != null)
            {
                await _client.DisposeAsync().ConfigureAwait(false);
            }
        }

        public async Task CloseQueueAsync()
        {
            await _processor.CloseAsync().ConfigureAwait(false);
        }
    }
}
