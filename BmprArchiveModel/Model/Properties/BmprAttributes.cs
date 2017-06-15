using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BmprArchiveModel.Model.Properties
{
    [JsonObject(MemberSerialization.OptOut)]
    public class BmprAttributes
    {
        public BmprAttributes(String attributesJson)
        {
            InputJson = attributesJson;

            // Parse Json and collect attributes
            Attributes = JObject.Parse(InputJson);
        }

        [JsonIgnore]
        private String InputJson;

        public virtual JObject Attributes { get; set; }

#if (DEBUG)
        #region DebugInfo

        /// <summary>
        /// To identify known attributes so that we can evaluate unknown ones
        /// </summary>
        /// <returns></returns>
        protected virtual List<String> GetKnownAttributes()
        {
            return new List<String>();
        }

        /// <summary>
        /// To identify and emit warnings about extra and unkown properties that were read
        /// </summary>
        /// <returns></returns>
        public HashSet<String> GetUnknownAttributes()
        {
            List<String> known = GetKnownAttributes();
            HashSet<String> unknown = new HashSet<String>();

            Dictionary<String, Object> all = Attributes.ToObject<Dictionary<String, Object>>();
            foreach (String key in all.Keys)
            {
                if (!known.Contains(key))
                    unknown.Add(key);
            }

            return unknown;
        }
    }

    #endregion
#endif
}
