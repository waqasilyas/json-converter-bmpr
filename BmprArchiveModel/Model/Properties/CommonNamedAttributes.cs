using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BmprArchiveModel.Model.Properties
{
    public class CommonNamedAttributes : BmprAttributes
    {
        public const String NAME_ATTRIB = "name";
        public const String DATE_ATTRIB = "creationDate";

        public static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public CommonNamedAttributes(String attributesJson) : base(attributesJson) { }

        #region Properties

        [JsonProperty(Order = 1)]
        public String Name
        {
            get
            {
                return (String)Attributes[NAME_ATTRIB];
            }
        }

        [JsonProperty(Order = 10)]
        public DateTime CreationDate
        {
            get
            {
                // The creation date is in attributes defined using JSON
                if (Attributes[DATE_ATTRIB] != null)
                {
                    // The date is in Unix epoch format
                    return EPOCH.AddMilliseconds((long)Attributes[DATE_ATTRIB]);
                }
                
                return DateTime.MinValue;
            }
        }

        [JsonIgnore]
        public override JObject Attributes { get; set; }

        #endregion

#if (DEBUG)

        /// <summary>
        /// To identify known attributes so that we can evaluate unknown ones
        /// </summary>
        /// <returns></returns>
        protected override List<String> GetKnownAttributes()
        {
            return new List<String>(new String[] { NAME_ATTRIB, DATE_ATTRIB });
        }

#endif
    }
}
