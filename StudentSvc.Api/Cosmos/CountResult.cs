using Newtonsoft.Json;
using System.Collections.Generic;

namespace StudentSvc.Api.Cosmos
{
    public class CountResult
    {
        [JsonProperty("$1")]
        public int _1 { get; set; }
    }
}
