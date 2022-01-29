using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StudentSvc.Api.Azure
{
    public class WorkerServiceBus : IHostedService, IDisposable
    {
        private readonly ILogger<WorkerServiceBus> _logger;
        private readonly IServiceBusConsumer _serviceBusConsumer;

        public WorkerServiceBus(ILogger<WorkerServiceBus> logger, IServiceBusConsumer serviceBusConsumer)
        {
            this._logger = logger;
            this._serviceBusConsumer = serviceBusConsumer;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Starting the service bus queue consumer and the subscription");
            await _serviceBusConsumer.RegisterOnMessageHandlerAndReceiveMessages().ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Stopping the service bus queue consumer and the subscription");
            await _serviceBusConsumer.CloseQueueAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async void Dispose(bool disposing)
        {
            if (disposing)
            {
                await _serviceBusConsumer.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}
