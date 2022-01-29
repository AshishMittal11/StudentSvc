using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StudentSvc.Api.Azure
{
    public class ProcessData : IProcessData
    {
        private readonly IHttpClientFactory _factory;
        private readonly ILogger<ProcessData> _logger;

        public ProcessData(IHttpClientFactory factory, ILogger<ProcessData> logger)
        {
            this._factory = factory;
            this._logger = logger;
        }

        public async Task Process(MyPayload myPayload)
        {
            try
            {
                using (var client = this._factory.CreateClient())
                {
                    await client.SendAsync(new HttpRequestMessage
                    {
                        Content = new StringContent(content: myPayload.Message, encoding: Encoding.UTF8, mediaType: "application/json"),
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(myPayload.Url)
                    }).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message, myPayload);
                throw;
            }
        }
    }
}
