using Newtonsoft.Json;
using System;

namespace StudentSvc.Api.Cosmos
{
    public abstract class CosmosBase
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("_etag")]
        public string ETag { get; set; }

        [JsonProperty("_rid")]
        public string RId { get; set; }

        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("_attachments")]
        public string Attachments { get; set; }

        [JsonProperty("_ts")]
        public string Ts { get; set; }
    }
}
