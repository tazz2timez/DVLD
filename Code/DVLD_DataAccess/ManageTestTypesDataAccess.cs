using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public static class clsManageTestTypesDataAccess
    {
        public static bool GetTestTypeInfoByID(int testTypeID, ref string testTypeTitle, ref string testTypeDescription, ref decimal testTypeFees)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM TestTypes WHERE TestTypeID = @testTypeID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@testTypeID", testTypeID);

            bool isFound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    testTypeTitle = (string)reader["TestTypeTitle"];
                    testTypeDescription = (string)reader["TestTypeDescription"];
                    testTypeFees = (decimal)reader["TestTypeFees"];
                    isFound = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsManageTestTypesDataAccess Class: {ex.Message}");
                isFound = false;
            }
            finally { connection.Close(); }

            return isFound;
        }

        public static bool UpdateTestTypeInfo(int testTypeID, string testTypeTitle, string testTypeDescription, decimal testTypeFees)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update TestTypes
                                SET TestTypeTitle = @testTypeTitle,
                                TestTypeDescription = @testTypeDescription,
                                TestTypeFees = @testTypeFees
                            WHERE TestTypeID = @testTypeID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@testTypeID", testTypeID);
            cmd.Parameters.AddWithValue("@testTypeTitle", testTypeTitle);
            cmd.Parameters.AddWithValue("@testTypeDescription", testTypeDescription);
            cmd.Parameters.AddWithValue("@testTypeFees", testTypeFees);

            int affectedRows = 0;

            try
            {
                connection.Open();

                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsManageTestTypesDataAccess Class: {ex.Message}");
                affectedRows = 0;
            }
            finally { connection.Close(); }

            return affectedRows > 0;
        }

        public static DataTable GetTestTypesList()
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            DataTable testTypesList = new DataTable();

            string query = "SELECT * FROM TestTypes";

            SqlCommand cmd = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                    testTypesList.Load(reader);

                reader.Close();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsManageTestTypesDataAccess Class: {ex.Message}");
                testTypesList = null;
            }
            finally { connection.Close(); }

            return testTypesList;
        }

    }
}
