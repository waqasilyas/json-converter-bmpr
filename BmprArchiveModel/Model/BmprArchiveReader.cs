using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BmprArchiveModel.Database;
using BmprArchiveModel.Model.Properties;
using Newtonsoft.Json.Linq;

namespace BmprArchiveModel.Model
{
    public class BmprArchiveReader
    {
        #region Enum

        /// <summary>
        /// Tables of a Bmpr Archive (BAR) which is an SQL database
        /// </summary>
        public enum BarTable
        {
            [ColumnInfoAttribute(new String[] { "NAME", "VALUE" })]
            INFO,

            [ColumnInfoAttribute(new String[] { "ID", "BRANCHID", "ATTRIBUTES", "DATA" })]
            RESOURCES,

            [ColumnInfoAttribute(new String[] { "ID", "ATTRIBUTES" })]
            BRANCHES,

            [ColumnInfoAttribute(new String[] { "ID", "ATTRIBUTES" })]
            THUMBNAILS
        }

        /// <summary>
        /// Attributes for defining columns of the BAR SQL tables
        /// </summary>
        public class ColumnInfoAttribute : Attribute
        {
            public String[] ColumnNames { get; set; }

            public ColumnInfoAttribute(String[] columnNames)
            {
                ColumnNames = columnNames;
            }
        }

        #endregion

        #region PublicStatic

        public const String MOCKUP_PROP = "mockup";
        public const String CONTROLS_PROP = "controls";
        public const String CONTROL_PROP = "control";
        public const String MEASUREDW_PROP = "measuredW";
        public const String MEASUREDH_PROP = "measuredH";
        public const String MOCKUPW_PROP = "mockupW";
        public const String MOCKUPH_PROP = "mockupH";
        public const String VERSION_PROP = "version";

        /// <summary>
        /// Loads the mockup project into the model
        /// </summary>
        /// <param name="sourceFile">the mockup archive file</param>
        /// <returns></returns>
        public static MockupProject LoadProject(String sourceFile)
        {
            MockupProject project = new MockupProject();

            using (DatabaseAccess db = new DatabaseAccess(sourceFile))
            {
                // Validate database schema
                ValidDatabase(db, sourceFile);

                // Load info
                project.Info = InternalLoadProjectInfo(db);

                // Load resources
                InternalLoadResources(db, project);

                // Load branches
                InternalLoadBranches(db, project);

                // Load thumbnails
                InternalLoadThumbnails(db, project);
            }

            return project;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <returns></returns>
        public static ProjectInfo LoadProjectInfo(String sourceFile)
        {
            using (DatabaseAccess db = new DatabaseAccess(sourceFile))
            {
                // Validate database schema
                ValidDatabase(db, sourceFile);

                return InternalLoadProjectInfo(db);
            }
        }

        #endregion

        #region PrivateStatic

        private static ProjectInfo InternalLoadProjectInfo(DatabaseAccess db)
        {
            ProjectInfo info = new ProjectInfo();

            String[][] data = db.GetTableContent(BarTable.INFO.ToString());
            foreach (String[] row in data)
            {
                if (row[0].Equals(ProjectInfo.ATTRIBUTES_PROP))
                {
                    if (row[0].Length == 0)
                        row[0] = "{}";

                    ProjectInfoArchiveAttributes attributes = new ProjectInfoArchiveAttributes(row[1]);
                    info.Properties.Add(row[0], attributes);

#if (DEBUG)
                    ReportUnknownThings(attributes.GetUnknownAttributes(), typeof(ProjectInfo).Name + " attributes");
#endif
                }
                else
                    info.Properties.Add(row[0], row[1]);
            }

            // Ensure we support this schema
            if (info.SchemaVersion == null 
                || ProjectInfo.SUPPORTED_SCHEMA.Major != new Version(info.SchemaVersion).Major)
                // Not a supported schema, throw exception
                throw new Exception(String.Format("Invalid bmpr archive format. Only version {0} is supported.", ProjectInfo.SUPPORTED_SCHEMA));

            return info;
        }

        private static void InternalLoadResources(DatabaseAccess db, MockupProject project)
        {
            List<Mockup> mockups = new List<Mockup>();

            String[][] data = db.GetTableContent(BarTable.RESOURCES.ToString());
            foreach (String[] row in data)
            {
                if (row[2].Length == 0)
                    row[2] = "{}";

                // Read resource attributes
                ResourceAttributes attributes = new ResourceAttributes(row[2]);

#if (DEBUG)
                ReportUnknownThings(attributes.GetUnknownAttributes(), typeof(ResourceAttributes).Name);
#endif

                switch (attributes.Kind)
                {
                    case ResourceKind.Mockup:
                    case ResourceKind.SymbolLibrary:
                        // Create appropariate instance for each case and add to project
                        AbstractControlContainer container;
                        if (attributes.Kind == ResourceKind.Mockup)
                        {
                            container = new Mockup();
                            project.Mockups.Add(container as Mockup);
                        }
                        else
                        {
                            container = new SymbolLibrary();
                            project.SymbolLibraries.Add(container as SymbolLibrary);
                        }
                        
                        // Set properties and attributes
                        container.Id = row[0];
                        container.BranchId = row[1];
                        container.Attributes = attributes;

                        // Load the 'data' value which is a JSON string, basically definition of all controls
                        JObject mData = JObject.Parse(row[3]);
                        if (mData != null && mData[MOCKUP_PROP] != null)
                        {
                            container.MeasuredW = (String)mData[MOCKUP_PROP][MEASUREDW_PROP];
                            container.MeasuredH = (String)mData[MOCKUP_PROP][MEASUREDH_PROP];
                            container.MockupW = (String)mData[MOCKUP_PROP][MOCKUPW_PROP];
                            container.MockupH = (String)mData[MOCKUP_PROP][MOCKUPH_PROP];
                            container.Version = (String)mData[MOCKUP_PROP][VERSION_PROP];

                            if (mData[MOCKUP_PROP][CONTROLS_PROP] != null)
                            {
                                JToken control = mData[MOCKUP_PROP][CONTROLS_PROP][CONTROL_PROP];
                                if (control != null)
                                {
                                    if (attributes.Kind == ResourceKind.Mockup)
                                        (container as Mockup).Controls = control.ToObject<MockupControl[]>();
                                    else
                                        (container as SymbolLibrary).Symbols = control.ToObject<MockupSymbol[]>();
                                }
                                else
                                {
                                    Warning(String.Format("Empty {0}, may be removed: {1}", attributes.Kind.ToString().ToLower(), attributes.Name));
                                }   
                            }
                        }

                        break;

                    case ResourceKind.Asset:
                        ProjectAsset asset = new ProjectAsset();
                        asset.Id = row[0];
                        asset.BranchId = row[1];
                        asset.Attributes = attributes;
                        asset.Data = row[3];

                        using (MD5 md5 = MD5.Create())
                        {
                            byte[] b = ASCIIEncoding.ASCII.GetBytes(asset.Data);
                            b = md5.ComputeHash(b);
                            asset.DataHash = BitConverter.ToString(b);
                        }

                        project.Assets.Add(asset);
                        break;

                    default:
                        Warning(String.Format("Unkown resource kind: {0}", row[2]));
                        break;
                }
            }

#if (DEBUG)
            // Report unrecognized stuff... useful for adding more detail to
            // model classes, which is useful for custom processing, and perhaps
            // editing capabilities in the future
            Dictionary<String, HashSet<String>> unknown = GetUnknownProperties(project);
            foreach (String type in unknown.Keys)
                ReportUnknownThings(unknown[type], type);
#endif
        }

        private static void InternalLoadBranches(DatabaseAccess db, MockupProject project)
        {
            String[][] data = db.GetTableContent(BarTable.BRANCHES.ToString());
            foreach (String[] row in data)
            {
                BranchInfo branch = new BranchInfo();
                branch.Id = row[0];

                // Read resource attributes
                if (row[1].Length == 0)
                    row[1] = "{}";

                BmprAttributes attributes = new BmprAttributes(row[1]);
                branch.Attributes = attributes;

                project.Branches.Add(branch);
            }

#if (DEBUG)
            // Report unrecognized attributes
            HashSet<String> unknown = new HashSet<string>();
            foreach (BranchInfo branch in project.Branches)
                foreach (String u in branch.Attributes.GetUnknownAttributes())
                    unknown.Add(u);

            ReportUnknownThings(unknown, typeof(BranchInfo).Name);
#endif
        }

        private static void InternalLoadThumbnails(DatabaseAccess db, MockupProject project)
        {
            String[][] data = db.GetTableContent(BarTable.BRANCHES.ToString());
            foreach (String[] row in data)
            {
                MockupThumbnail thumbnail = new MockupThumbnail();
                thumbnail.Id = row[0];

                if (row[1].Length == 0)
                    row[1] = "{}";

                ControlProperties attributes = JObject.Parse(row[1]).ToObject<ControlProperties>();
                thumbnail.Attributes = attributes;

                project.Thumbnails.Add(thumbnail);
            }

#if (DEBUG)
            // Report unrecognized attributes
            HashSet<String> unknown = new HashSet<string>();
            foreach (MockupThumbnail thumbnail in project.Thumbnails)
                foreach (String u in thumbnail.Attributes.UnknownProperties.Keys)
                    unknown.Add(u);

            ReportUnknownThings(unknown, typeof(MockupThumbnail).Name);
#endif
        }

        private static void ValidDatabase(DatabaseAccess db, String sourceFile)
        {
            String[] tableList = db.GetTableList();
            foreach (BarTable table in Enum.GetValues(typeof(BarTable)))
            {
                // Verify the required tables exist
                if (!tableList.Contains(table.ToString()))
                    throw new Exception(String.Format("Invalid bmpr archive (missing tables): {0}", sourceFile));

                // verify columns
                System.Reflection.FieldInfo field = table.GetType().GetField(table.ToString());
                ColumnInfoAttribute[] attributes = field.GetCustomAttributes(typeof(ColumnInfoAttribute), false) as ColumnInfoAttribute[];

                String[] columnNames = db.GetColumnNames(table.ToString());
                foreach (String col in attributes[0].ColumnNames)
                {
                    if (!columnNames.Contains(col))
                        throw new Exception(String.Format("Invalid bmpr archive (missing column '{0}.{1}'): {2}", table, col, sourceFile));
                }
            }
        }

        private static void Warning(String message)
        {
            Console.WriteLine("warning: " + message);
        }

        #endregion

        #region DebugInfo

#if (DEBUG)
        public static Dictionary<String, HashSet<String>> GetUnknownProperties(MockupProject project)
        {
            Dictionary<String, HashSet<String>> unknown = new Dictionary<String, HashSet<String>>();

            String acType = typeof(AbstractControlContainer).Name;
            unknown[acType] = new HashSet<String>();

            String mcType = typeof(MockupControl).Name;
            unknown[mcType] = new HashSet<String>();

            String mcpType = typeof(ControlProperties).Name;
            unknown[mcpType] = new HashSet<String>();

            String msType = typeof(MockupSymbol).Name;
            unknown[msType] = new HashSet<String>();

            List<AbstractControlContainer> allContainers = new List<AbstractControlContainer>();
            allContainers.AddRange(project.Mockups);
            allContainers.AddRange(project.SymbolLibraries);

            foreach (AbstractControlContainer container in allContainers)
            {
                // Check Mockup
                foreach (String key in container.UnknownProperties.Keys)
                    unknown[acType].Add(key);

                // Check MockupControl
                if (container is Mockup)
                {
                    Mockup m = (Mockup)container;
                    if (m.Controls != null && m.Controls.Length > 0)
                    {
                        foreach (MockupControl c in m.Controls)
                        {
                            foreach (String key in c.UnknownProperties.Keys)
                                unknown[mcType].Add(key);

                            // Check MockupControlProperties
                            if (c.Properties != null)
                                foreach (String key in c.Properties.UnknownProperties.Keys)
                                    unknown[mcpType].Add(key);
                        }
                    }
                }
                else if (container is SymbolLibrary)
                {
                    SymbolLibrary m = (SymbolLibrary)container;
                    if (m.Symbols != null && m.Symbols.Length > 0)
                    {
                        foreach (MockupSymbol c in m.Symbols)
                        {
                            foreach (String key in c.UnknownProperties.Keys)
                                unknown[msType].Add(key);

                            // Check MockupControlProperties
                            if (c.Properties != null)
                                foreach (String key in c.Properties.UnknownProperties.Keys)
                                    unknown[mcpType].Add(key);
                        }
                    }
                }
            }

            return unknown;
        }

        private static void ReportUnknownThings(HashSet<String> unknownThings, String thingName)
        {
            if (unknownThings.Count == 0)
                return;
            
            bool first = true;
            StringBuilder s = new StringBuilder();
            foreach (String u in unknownThings)
            {
                if (first)
                {
                    first = false;
                    s.Append(u);
                }
                else
                {
                    s.Append(", ").Append(u);
                }
            } 

            Warning(String.Format("Unknown {0}: {1}", thingName, s));
        }
#endif
        #endregion
    }
}
