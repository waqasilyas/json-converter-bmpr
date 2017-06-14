using Newtonsoft.Json;

namespace BalsamiqArchiveModel.Model
{
    public class Mockup : AbstractControlContainer
    {
        [JsonProperty(Order = 10)]
        public MockupControl[] Controls { get; set; }
    }
}
