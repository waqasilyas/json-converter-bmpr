using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BalsamiqArchiveModel.Database;
using Newtonsoft.Json.Linq;

namespace BalsamiqArchiveModel.Model
{
    public class BalsamiqArchiveReader
    {
        /// <summary>
        /// Tables of a Balsamiq Archive (BAR) which is an SQL database
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

        /// <summary>
        /// Loads the mockup project into a model
        /// </summary>
        /// <param name="sourceFile">the Balsamiq Mockup archive file</param>
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

                // Load mockups
                InternalLoadResources(db, project);
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

        /*public static Mockup[] LoadResources(String sourceFile)
        {
            using (DatabaseAccess db = new DatabaseAccess(sourceFile))
            {
                // Validate database schema
                ValidDatabase(db, sourceFile);

                return InternalLoadResources(db);
            }
        }*/

        #region Private

        private static ProjectInfo InternalLoadProjectInfo(DatabaseAccess db)
        {
            ProjectInfo info = new ProjectInfo();

            String[][] data = db.GetTableContent(BarTable.INFO.ToString());
            foreach (String[] row in data)
            {
                if (row[0].Equals(ProjectInfo.ATTRIBUTES_PROP))
                    info.Properties.Add(row[0], new ProjectInfoArchiveAttributes(row[1]));
                else
                    info.Properties.Add(row[0], row[1]);
            }

            // Ensure we support this schema
            if (info.SchemaVersion == null 
                || ProjectInfo.SUPPORTED_SCHEMA.Major != new Version(info.SchemaVersion).Major)
                // Not a supported schema, throw exception
                throw new Exception(String.Format("Invalid balsamiq archive format. Only version {0} is supported.", ProjectInfo.SUPPORTED_SCHEMA));

            return info;
        }

        private static void InternalLoadResources(DatabaseAccess db, MockupProject project)
        {
            List<Mockup> mockups = new List<Mockup>();

            String[][] data = db.GetTableContent(BarTable.RESOURCES.ToString());
            foreach (String[] row in data)
            {
                // Read resource attributes
                ResourceAttributes attributes = new ResourceAttributes(row[2]);

                switch(attributes.Kind)
                {
                    case ResourceKind.Mockup:
                        Mockup mockup = new Mockup();
                        mockup.Id = row[0];
                        mockup.BranchId = row[1];
                        mockup.Attributes = attributes;
                        mockup.Data = JObject.Parse(row[3]);

                        project.Mockups.Add(mockup);
                        break;

                    case ResourceKind.SymbolLibrary:
                        break;

                    case ResourceKind.Asset:
                        break;

                    case ResourceKind.Unknown:
                        throw new Exception(String.Format("Unkown resource kind: {0}", row[2]));
                }

                //Console.WriteLine("======================== mockup data ======================");
                //Console.WriteLine(JsonConvert.SerializeObject(attributes, Formatting.Indented));
                //Console.WriteLine(JsonConvert.SerializeObject(JObject.Parse(row[3]), Formatting.Indented));
            }
        }

        private static void ValidDatabase(DatabaseAccess db, String sourceFile)
        {
            String[] tableList = db.GetTableList();
            foreach (BarTable table in Enum.GetValues(typeof(BarTable)))
            {
                // Verify the required tables exist
                if (!tableList.Contains(table.ToString()))
                    throw new Exception(String.Format("Invalid balsamiq archive (missing tables): {0}", sourceFile));

                // verify columns
                FieldInfo field = table.GetType().GetField(table.ToString());
                ColumnInfoAttribute[] attributes = field.GetCustomAttributes(typeof(ColumnInfoAttribute), false) as ColumnInfoAttribute[];

                String[] columnNames = db.GetColumnNames(table.ToString());
                foreach (String col in attributes[0].ColumnNames)
                {
                    if (!columnNames.Contains(col))
                        throw new Exception(String.Format("Invalid balsamiq archive (missing column '{0}.{1}'): {2}", table, col, sourceFile));
                }
            }
        }

        #endregion
    }

    /*switch (row[0])
                {
                    case ProjectInfo.ATTRIBUTES_PROP:
                        // The value is JSON string, so extract attributes
                        JObject attributes = JObject.Parse(row[1]);
                        info.name = (String)attributes[ProjectInfo.NAME_PROP];
                        long date = (long)attributes[ProjectInfo.DATE_PROP];
                        info.creationDate = new DateTime(date);

                        break;

                    case ProjectInfo.FORMAT_PROP:
                        info.archiveFormat = row[1];
                        break;

                    case ProjectInfo.REVISION_PROP:
                        info.archiveRevision = row[1];
                        break;

                    case ProjectInfo.REVISION_UUID_PROP:
                        info.archiveRevisionUuid = row[1];
                        break;

                    case ProjectInfo.SCHEMA_PROP:
                        info.schemaVersion = new Version(row[1]);
                        break;

                    default:
                        info.otherProperties.Add(row[0], row[1]);
                        break;
                }*/
}
