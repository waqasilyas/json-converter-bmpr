using Newtonsoft.Json;

namespace BalsamiqArchiveModel.Model
{
    public class SymbolLibrary : AbstractControlContainer
    {
        [JsonProperty(Order = 10)]
        public MockupSymbol[] Symbols { get; set; }
    }
}
