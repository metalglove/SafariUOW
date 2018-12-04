using System;
using System.Data.SqlClient;

namespace Safari_Tests.Utilities
{
    internal static class MsSqlDatabaseCreator
    {
        public static void Create(string connectionstring, string tableCreations = "")
        {
            if (DatabaseExists(connectionstring))
            {
                DropDatabase(connectionstring);
            }
            CreateDatabase(connectionstring, tableCreations);
        }

        private static void CreateDatabase(string connectionString, string tableCreations)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

            string databaseName = sqlConnectionStringBuilder.InitialCatalog;

            sqlConnectionStringBuilder.InitialCatalog = "master";

            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = $"CREATE DATABASE {databaseName}";
                    sqlCommand.ExecuteNonQuery();
                    if (!string.IsNullOrWhiteSpace(tableCreations))
                    {
                        sqlCommand.CommandText = $"USE {databaseName} " + tableCreations;
                        sqlCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        private static bool DatabaseExists(string connectionString)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

            string databaseName = sqlConnectionStringBuilder.InitialCatalog;

            sqlConnectionStringBuilder.InitialCatalog = "master";

            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                sqlConnection.Open();

                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandText = $"SELECT db_id('{databaseName}')";

                    return command.ExecuteScalar() != DBNull.Value;
                }
            }
        }

        private static void DropDatabase(string connectionString)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

            string databaseName = sqlConnectionStringBuilder.InitialCatalog;

            sqlConnectionStringBuilder.InitialCatalog = "master";

            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                {
                    sqlCommand.CommandText = $@"
                    ALTER DATABASE {databaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    DROP DATABASE [{databaseName}]
                ";
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
