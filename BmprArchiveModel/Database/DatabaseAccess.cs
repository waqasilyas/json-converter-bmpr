using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace BmprArchiveModel.Database
{
    public class DatabaseAccess : IDisposable
    {
        #region Properties

        private SQLiteConnection Connection;

        #endregion

        #region Public

        public DatabaseAccess(String bmprFile)
        {
            Connection = new SQLiteConnection(String.Format("Data Source={0};Version=3;Read Only=True;", bmprFile));
            Connection.Open();
        }

        public String[] GetTableList()
        {
            String sql = "SELECT name FROM sqlite_master WHERE type='table';";
            return ExecuteSql(sql, 0);
        }

        public String[][] GetTableContent(String tableName)
        {
            String sql = String.Format("SELECT * FROM {0}", tableName);
            return ExecuteSql(sql);
        }

        public String[] GetColumnNames(String tableName)
        {
            String sql = String.Format("PRAGMA table_info({0})", tableName);
            return ExecuteSql(sql, 1);
        }

        public int GetRowCount(String table)
        {
            String sql = String.Format("PRAGMA table_info({0});", table);
            return 0;
        }

        public void Dispose()
        {
            Connection.Dispose();
        }

        #endregion

        #region Private

        private String[] ExecuteSql(String sql, int column)
        {
            String[][] data = ExecuteSql(sql);
            String[] list = new String[data.Length];

            for (int i = 0; i < list.Length; i++)
                list[i] = data[i][column];

            return list;
        }

        private String[][] ExecuteSql(String sql)
        {
            SQLiteCommand command = new SQLiteCommand(sql, Connection);
            SQLiteDataReader reader = command.ExecuteReader();

            List<String[]> rows = new List<string[]>();

            while (reader.Read())
            {
                String[] cols = new String[reader.FieldCount];

                for (int i = 0; i < cols.Length; i++)
                    cols[i] = reader.GetValue(i).ToString();

                rows.Add(cols);
            }

            return rows.ToArray();
        }

        #endregion
    }
}
