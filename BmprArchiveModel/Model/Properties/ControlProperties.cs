using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BmprArchiveModel.Model.Properties
{
    public class ControlProperties
    {
        // There are too many properties so not adding more here, until there is a need
        public String ControlName { get; set; }
        public String Text { get; set; }
        public String State { get; set; }

        [JsonExtensionData]
        public Dictionary<String, JToken> Unknown { get; set; } = new Dictionary<String, JToken>();
    }
}
