using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BalsamiqArchiveModel.Model
{
    public class ProjectInfo
    {
        public static readonly Version SUPPORTED_SCHEMA = new Version("1.0");

        public const String ATTRIBUTES_PROP = "ArchiveAttributes";
        public const String NAME_PROP = "name";
        public const String DATE_PROP = "creationDate";
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

    public class ProjectInfoArchiveAttributes : BalsamiqArchiveAttributes
    {
        public ProjectInfoArchiveAttributes(String attributesJson) : base(attributesJson) { }

        public String Name
        {
            get
            {
                return (String)Attributes[ProjectInfo.NAME_PROP];
            }
        }

        public DateTime CreationDate
        {
            get
            {
                // The creation date is in attributes defined using JSON
                if (Attributes != null)
                {
                    if (Attributes[ProjectInfo.DATE_PROP] != null)
                    {
                        // The date is in Unix epoch format
                        return EPOCH.AddMilliseconds((long)Attributes[ProjectInfo.DATE_PROP]);
                    }
                }

                return DateTime.MinValue;
            }
        }
    }
}
