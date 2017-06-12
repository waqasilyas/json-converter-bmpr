using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BalsamiqArchiveModel.Model
{
    [JsonObject(MemberSerialization.OptOut)]
    public class BalsamiqArchiveAttributes
    {
        public const String NAME_ATTRIB = "name";
        public const String DATE_ATTRIB = "creationDate";

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

        [JsonProperty(Order = 1)]
        public String Name
        {
            get
            {
                return (String)Attributes[NAME_ATTRIB];
            }
        }

        [JsonProperty(Order = 8)]
        public DateTime CreationDate
        {
            get
            {
                // The creation date is in attributes defined using JSON
                if (Attributes != null)
                {
                    if (Attributes[DATE_ATTRIB] != null)
                    {
                        // The date is in Unix epoch format
                        return EPOCH.AddMilliseconds((long)Attributes[DATE_ATTRIB]);
                    }
                }

                return DateTime.MinValue;
            }
        }

        #endregion
    }
}
