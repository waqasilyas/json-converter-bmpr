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

        protected override void initializeAttributes(JObject attributes)
        {
            Name = (String)attributes[NAME_ATTRIB];

            if (attributes[DATE_ATTRIB] != null)
            {
                // The date is in Unix epoch format
                CreationDate = EPOCH.AddMilliseconds((long)attributes[DATE_ATTRIB]);
            }

            attributes.Remove(NAME_ATTRIB);
            attributes.Remove(DATE_ATTRIB);
        }

        #region Properties

        [JsonProperty(Order = 1)]
        public String Name { get; set; }

        [JsonProperty(Order = 10)]
        public DateTime? CreationDate { get; set; }

        #endregion
    }
}
