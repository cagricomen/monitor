using System.Collections.Generic;
using Newtonsoft.Json;

namespace Baron.Web
{
    public class BReturn
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string InternalMessage { get; set; }
        public int Code { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }
        public bool Success { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<BReturnError> Errors { get; set; }
    }
    public class BReturnPagedData<T>
    {
        public List<T> Items { get; set; }
        public int PageCount { get; set; }
        public int ItemCount { get; set; }
        public int CurrentPage { get; set; }
    }


    public class BReturnError
    {
        public string Message { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string InternalMessage { get; set; }
        public string Name { get; set; }
    }
}
