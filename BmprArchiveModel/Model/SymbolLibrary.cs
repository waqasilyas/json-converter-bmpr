using Newtonsoft.Json;

namespace BmprArchiveModel.Model
{
    public class SymbolLibrary : AbstractControlContainer
    {
        [JsonProperty(Order = 10)]
        public MockupSymbol[] Symbols { get; set; }
    }
}
