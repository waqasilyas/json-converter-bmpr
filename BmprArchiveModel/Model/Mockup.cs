using Newtonsoft.Json;

namespace BmprArchiveModel.Model
{
    public class Mockup : AbstractControlContainer
    {
        [JsonProperty(Order = 10)]
        public MockupControl[] Controls { get; set; }
    }
}
