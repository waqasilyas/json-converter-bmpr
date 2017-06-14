using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BmprArchiveModel.Model
{
    public class ProjectInfo
    {
        public static readonly Version SUPPORTED_SCHEMA = new Version("1.0");

        public const String ATTRIBUTES_PROP = "ArchiveAttributes";
        public const String FORMAT_PROP = "ArchiveFormat";
        public const String REVISION_PROP = "ArchiveRevision";
        public const String REVISION_UUID_PROP = "ArchiveRevisionUUID";
        public const String SCHEMA_PROP = "SchemaVersion";

        #region Properties

        public ProjectInfoArchiveAttributes ArchiveAttributes
        {
            get
            {
                return (ProjectInfoArchiveAttributes) Properties[ATTRIBUTES_PROP];
            }
        }

        public String ArchiveFormat
        {
            get
            {
                return (String)Properties[FORMAT_PROP];
            }
        }

        public String ArchiveRevision
        {
            get
            {
                return (String)Properties[REVISION_PROP];
            }
        }

        public String ArchiveRevisionUuid
        {
            get
            {
                return (String)Properties[REVISION_UUID_PROP];
            }
        }

        public String SchemaVersion
        {
            get
            {
               return (String)Properties[SCHEMA_PROP];
            }
        }

        [JsonIgnore]
        public Dictionary<String, Object> Properties { get; } = new Dictionary<String, Object>();

        #endregion
    }

    public class ProjectInfoArchiveAttributes : BmprArchiveAttributes
    {
        public ProjectInfoArchiveAttributes(String attributesJson) : base(attributesJson) { }
    }
}
