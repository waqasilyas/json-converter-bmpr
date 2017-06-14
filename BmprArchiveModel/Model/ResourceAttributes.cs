using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BmprArchiveModel.Model
{
    public class ResourceAttributes : BmprArchiveAttributes
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

        #region Properties

        [JsonProperty(Order = 2)]
        public ResourceKind Kind
        {
            get
            {
                foreach (ResourceKind kind in Enum.GetValues(typeof(ResourceKind)))
                {
                    StringBuilder kindStr = new StringBuilder(kind.ToString());
                    kindStr[0] = Char.ToLower(kindStr[0]);
                    if (kindStr.ToString().Equals((String)Attributes[KIND_ATTRIB]))
                        return kind;
                }

                return ResourceKind.Unknown;
            }
        }

        [JsonProperty(Order = 3)]
        public bool Trashed
        {
            get
            {
                return (bool)Attributes[TRASHED_ATTRIB];
            }
        }

        [JsonProperty(Order = 4)]
        public String Order
        {
            get
            {
                // Only present when 'kind' is 'mockup'
                return (String)Attributes[ORDER_ATTRIB];
            }
        }

        [JsonProperty(Order = 5)]
        public String ThumbnailID
        {
            get
            {
                return (String)Attributes[THUMBNAIL_ATTRIB];
            }
        }

        [JsonProperty(Order = 6)]
        public String ParentID
        {
            get
            {
                return (String)Attributes[PARENT_ATTRIB];
            }
        }

        [JsonProperty(Order = 7)]
        public String Notes
        {
            get
            {
                return (String)Attributes[NOTES_ATTRIB];
            }
        }

        [JsonProperty(Order = 9)]
        public String ModifiedBy
        {
            get
            {
                return (String)Attributes[MODIFIED_ATTRIB];
            }
        }

        [JsonProperty(Order = 10)]
        public String MimeType
        {
            get
            {
                return (String)Attributes[MIME_ATTRIB];
            }
        }

        [JsonProperty(Order = 11)]
        public Uri ImportedFrom
        {
            get
            {
                String path = (String)Attributes[IMPORTED_ATTRIB];
                if (path != null && path.Length > 0)
                    return new Uri(path);

                return null;
            }
        }

        #endregion

#if (DEBUG)
        public override List<String> GetKnownAttributes()
        {
            List<String> known = base.GetKnownAttributes();
            known.AddRange(new String[]
            {
                IMPORTED_ATTRIB,
                KIND_ATTRIB,
                MIME_ATTRIB,
                MODIFIED_ATTRIB,
                NOTES_ATTRIB,
                ORDER_ATTRIB,
                PARENT_ATTRIB,
                THUMBNAIL_ATTRIB,
                TRASHED_ATTRIB
            });

            return known;
        }
#endif
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
