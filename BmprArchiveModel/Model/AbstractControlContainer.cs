using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BmprArchiveModel.Model
{
    [JsonObject(MemberSerialization.OptOut)]
    public abstract class AbstractControlContainer
    {
        #region Properties

        public String Id { get; set; }
        public String BranchId { get; set; }
        public String MeasuredH { get; set; }
        public String MeasuredW { get; set; }
        public String MockupH { get; set; }
        public String MockupW { get; set; }
        public String Version { get; set; }
        public ResourceAttributes Attributes { get; set; }
        
        [JsonExtensionData]
        public Dictionary<String, JToken> UnknownProperties { get; set; } = new Dictionary<String, JToken>();

        #endregion
    }
}
