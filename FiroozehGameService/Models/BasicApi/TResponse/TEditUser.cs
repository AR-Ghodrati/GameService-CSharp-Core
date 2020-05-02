using System;
using FiroozehGameService.Models.GSLive;
using Newtonsoft.Json;

namespace FiroozehGameService.Models.BasicApi.TResponse
{
    [Serializable]
    internal class TEditUser
    {
        [JsonProperty("status")] public bool Status { set; get; }

        [JsonProperty("user")] public Member Member { set; get; }
    }
}