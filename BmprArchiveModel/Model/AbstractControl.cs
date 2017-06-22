using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using BmprArchiveModel.Model.Properties;

namespace BmprArchiveModel.Model
{
    public abstract class AbstractControl
    {
        #region Properties

        [JsonProperty(Order = 1)]
        public String ID { get; set; }

        [JsonProperty(Order = 2)]
        public String Name
        {
            get
            {
                if (Properties != null)
                    return Properties.ControlName;

                return null;
            }
        }

        [JsonProperty(Order = 3)]
        public String TypeID { get; set; }

        [JsonProperty(Order = 10)]
        public String X { get; set; }

        [JsonProperty(Order = 11)]
        public String Y { get; set; }

        [JsonProperty(Order = 12)]
        public String W { get; set; }

        [JsonProperty(Order = 13)]
        public String H { get; set; }

        [JsonProperty(Order = 14)]
        public String MeasuredW { get; set; }

        [JsonProperty(Order = 15)]
        public String MeasuredH { get; set; }

        [JsonProperty(Order = 16)]
        public String ZOrder { get; set; }

        [JsonProperty(Order = 17)]
        public ControlProperties Properties { get; set; }

        [JsonProperty(Order = 18)]
        public MockupControlChildren Children { get; set; }

        [JsonProperty(Order = 19)]
        [JsonExtensionData]
        public Dictionary<String, JToken> UnknownProperties { get; set; } = new Dictionary<String, JToken>();

        #endregion
    }

    /// <summary>
    /// This class is just empty container to match JSON hierarchy
    /// </summary>
    public class MockupControlChildren
    {
        public MockupControlContainer Controls { get; set; }
    }

    /// <summary>
    /// This class is just empty container to match JSON hierarchy
    /// </summary>
    public class MockupControlContainer
    {
        public MockupControl[] Control { get; set; }
    }
}
