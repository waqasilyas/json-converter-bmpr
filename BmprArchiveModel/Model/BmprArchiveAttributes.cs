using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BmprArchiveModel.Model
{
    [JsonObject(MemberSerialization.OptOut)]
    public class BmprArchiveAttributes
    {
        public const String NAME_ATTRIB = "name";
        public const String DATE_ATTRIB = "creationDate";

        public static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public BmprArchiveAttributes(String attributesJson)
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

        /// <summary>
        /// To identify known attributes so that we can evaluate unknown ones
        /// </summary>
        /// <returns></returns>
        public virtual List<String> GetKnownAttributes()
        {
            return new List<String>(new String[] { NAME_ATTRIB, DATE_ATTRIB });
        }

        /// <summary>
        /// To identify and emit warnings about extra and unkown properties that were read
        /// </summary>
        /// <returns></returns>
        public HashSet<String> GetUnknownAttributes()
        {
            List<String> known = GetKnownAttributes();
            HashSet<String> uknown = new HashSet<String>();

            Dictionary<String, Object> all = Attributes.ToObject<Dictionary<String, Object>>();
            foreach (String key in all.Keys)
            {
                if (!known.Contains(key))
                    uknown.Add(key);
            }

            return uknown;
        }
    }
}
