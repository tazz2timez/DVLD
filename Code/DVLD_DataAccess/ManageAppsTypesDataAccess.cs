using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public static class clsManageAppsTypesDataAccess
    {
        public static bool GetAppTypeInfoByID(int appID, ref string appTitle, ref decimal appFees)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @appID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@appID", appID);

            bool isFound = false;   

            try
            {
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if(reader.Read())
                {
                    appTitle = (string)reader["ApplicationTypeTitle"];
                    appFees = (decimal)reader["ApplicationFees"];
                    isFound = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsManageAppsTypesDataAccess Class: {ex.Message}");
                isFound = false;
            }
            finally { connection.Close(); }

            return isFound;
        }

        public static bool UpdateAppTypeInfo(int appID, string appTitle, decimal appFees)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update ApplicationTypes
                                SET ApplicationTypeTitle = @appTitle,
                                ApplicationFees = @appFees
                            WHERE ApplicationTypeID = @appID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@appID", appID);
            cmd.Parameters.AddWithValue("@appTitle", appTitle);
            cmd.Parameters.AddWithValue("@appFees", appFees);

            int affectedRows = 0;

            try
            {
                connection.Open();

                affectedRows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsManageAppsTypesDataAccess Class: {ex.Message}");
                affectedRows = 0;
            }
            finally { connection.Close(); }

            return affectedRows > 0;
        }

        public static DataTable GetAppTypesList()
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            DataTable appsList = new DataTable();

            string query = "SELECT * FROM ApplicationTypes";

            SqlCommand cmd = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                    appsList.Load(reader);

                reader.Close();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsManageAppsTypesDataAccess Class: {ex.Message}");
                appsList = null;
            }
            finally { connection.Close(); }

            return appsList;
        }

    }
}
