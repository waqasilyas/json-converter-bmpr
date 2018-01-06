using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace BmprArchiveModel.Model.Properties
{
    public class ResourceAttributes : CommonNamedAttributes
    {
        public const String IMPORTED_ATTRIB = "importedFrom";
        public const String KIND_ATTRIB = "kind";
        public const String MIME_ATTRIB = "mimeType";
        public const String MODIFIED_ATTRIB = "modifiedBy";
        public const String NOTES_ATTRIB = "notes";
        public const String ORDER_ATTRIB = "order";
        public const String PARENT_ATTRIB = "parentID";
        public const String THUMBNAIL_ATTRIB = "thumbnailID";
        public const String TRASHED_ATTRIB = "trashed";

        public ResourceAttributes(String attributesJson) : base(attributesJson) { }

        protected override void initializeAttributes(JObject attributes)
        {
            base.initializeAttributes(attributes);

            // Resource kind
            foreach (ResourceKind kind in Enum.GetValues(typeof(ResourceKind)))
            {
                StringBuilder kindStr = new StringBuilder(kind.ToString());
                kindStr[0] = Char.ToLower(kindStr[0]);
                if (kindStr.ToString().Equals((String)attributes[KIND_ATTRIB]))
                    Kind = kind;
            }
            
            // Is the resource in trash bin?
            Trashed = (bool)attributes[TRASHED_ATTRIB];

            // View "Z order" for the resource. Only present when 'kind' is 'mockup'
            Order = (String)attributes[ORDER_ATTRIB];

            // The ID of the thumbnail of this resource
            ThumbnailID = (String)attributes[THUMBNAIL_ATTRIB];

            // Associates this resource with another resource ID
            ParentID = (String)attributes[PARENT_ATTRIB];

            // Notes for the resource
            Notes = (String)attributes[NOTES_ATTRIB];

            // User who modified the resource
            ModifiedBy = (String)attributes[MODIFIED_ATTRIB];

            // Mime type of the resource
            MimeType = (String)attributes[MIME_ATTRIB];

            ImportedFrom = (String)attributes[IMPORTED_ATTRIB];
            
            // Remove all known attributes
            attributes.Remove(KIND_ATTRIB);
            attributes.Remove(TRASHED_ATTRIB);
            attributes.Remove(ORDER_ATTRIB);
            attributes.Remove(THUMBNAIL_ATTRIB);
            attributes.Remove(PARENT_ATTRIB);
            attributes.Remove(NOTES_ATTRIB);
            attributes.Remove(MODIFIED_ATTRIB);
            attributes.Remove(MIME_ATTRIB);
            attributes.Remove(IMPORTED_ATTRIB);
        }

        #region Properties

        [JsonProperty(Order = 2)]
        public ResourceKind? Kind { get; set; }

        [JsonProperty(Order = 3)]
        public bool Trashed { get; set; }

        [JsonProperty(Order = 4)]
        public String Order { get; set; }

        [JsonProperty(Order = 5)]
        public String ThumbnailID { get; set; }

        [JsonProperty(Order = 6)]
        public String ParentID { get; set; }

        [JsonProperty(Order = 7)]
        public String Notes { get; set; }

        [JsonProperty(Order = 11)]
        public String ModifiedBy { get; set; }

        [JsonProperty(Order = 12)]
        public String MimeType { get; set; }

        [JsonProperty(Order = 13)]
        public String ImportedFrom { get; set; }

        #endregion
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ResourceKind
    {
        Mockup,
        SymbolLibrary,
        Asset,
        OtherAsset,
        Unknown
    }
}
