using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BmprArchiveModel.Model.Properties
{
    [JsonObject(MemberSerialization.OptOut)]
    public abstract class BmprAttributes
    {
        public BmprAttributes(String attributesJson)
        {
            InputJson = attributesJson;

            // Parse Json and collect attributes
            var attributes = JObject.Parse(InputJson);
            initializeAttributes(attributes);

            // Save the rest of the attributes as unknown
            Unknown = new HashSet<String>(attributes.ToObject<Dictionary<String, JToken>>().Keys);
        }

        [JsonIgnore]
        private String InputJson;

        [JsonIgnore]
        public HashSet<String> Unknown { get; }

        /// <summary>
        /// Initializes the attributes known to this class, or any child class. All overriding classes
        /// should remove the known attributes after initializing. Any attributes remaining after 
        /// the call to this method will be considered "unknown" and could raise warnings.
        /// </summary>
        /// <param name="attributes"></param>
        protected abstract void initializeAttributes(JObject attributes);
    }
}
