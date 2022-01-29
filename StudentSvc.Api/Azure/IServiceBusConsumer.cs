using System.Threading.Tasks;

namespace StudentSvc.Api.Azure
{
    public interface IServiceBusConsumer
    {
        Task RegisterOnMessageHandlerAndReceiveMessages();
        Task CloseQueueAsync();
        ValueTask DisposeAsync();
    }
}
