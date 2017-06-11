using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BalsamiqArchiveModel.Model
{
    [JsonObject(MemberSerialization.OptOut)]
    public class BalsamiqArchiveAttributes
    {
        public static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public BalsamiqArchiveAttributes(String attributesJson)
        {
            InputJson = attributesJson;

            // Parse Json and collect attributes
            Attributes = JObject.Parse(InputJson);
        }

        #region Properties

        [JsonIgnore]
        private String InputJson;

        [JsonIgnore]
        public JObject Attributes { get; }

        #endregion
    }
}
