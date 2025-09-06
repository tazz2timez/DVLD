using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public static class clsDriversDataAccess
    {
        public static bool GetDriverInfoByDriverID(int driverID, ref int personID, ref int createdByUserID, ref DateTime creationDate)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Drivers WHERE driverID = @driverID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@driverID", driverID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    personID = (int)reader["personID"];
                    createdByUserID = (int)reader["createdByUserID"];
                    creationDate = (DateTime)reader["creationDate"];

                    isFound = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsDriversDataAccess Class: {ex.Message}");
                isFound = false;
            }
            finally { connection.Close(); }

            return isFound;
        }

        public static bool GetDriverInfoByPersonID(ref int driverID, int personID, ref int createdByUserID, ref DateTime creationDate)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Drivers WHERE personID = @personID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@personID", personID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    driverID = (int)reader["driverID"];
                    createdByUserID = (int)reader["createdByUserID"];
                    creationDate = (DateTime)reader["creationDate"];

                    isFound = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsDriversDataAccess Class: {ex.Message}");
                isFound = false;
            }
            finally { connection.Close(); }

            return isFound;
        }

        public static bool isPersonAlreadyDriver(int personID)
        {
            bool isDriver = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Drivers WHERE personID = @personID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@personID", personID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isDriver = reader.HasRows;
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsDriversDataAccess Class: {ex.Message}");
                isDriver = false;
            }
            finally { connection.Close(); }

            return isDriver;
        }

        public static int AddNewDriver(int personID, int createdByUserID, DateTime creationDate)
        {
            // this function will return the new driverID if succeeded or -1 if not
            int driverID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO Drivers(PersonID, CreatedByUserID, CreationDate)
                                VALUES (@personID, @createdByUserID, @creationDate);
                                SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@personID", personID);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);
            command.Parameters.AddWithValue("@creationDate", creationDate);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    driverID = insertedID;
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsDriversDataAccess Class: {ex.Message}");
                driverID = -1;
            }
            finally { connection.Close(); }

            return driverID;
        }

        public static bool UpdateDriverInfo(int driverID, int personID, int createdByUserID, DateTime creationDate)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE Drivers 
                                SET PersonID = @personID, 
                                    CreatedByUserID = @createdByUserID,
                                    CreationDate = @creationDate
                                WHERE DriverID = @driverID
                            ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@driverID", driverID);
            command.Parameters.AddWithValue("@personID", personID);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);
            command.Parameters.AddWithValue("@creationDate", creationDate);


            int rowsAffected = 0;

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsDriversDataAccess Class: {ex.Message}");
                rowsAffected = 0;
            }
            finally { connection.Close(); }

            return (rowsAffected > 0);
        }

        public static bool DeleteDriver(int driverID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "DELETE Drivers WHERE driverID = @driverID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@driverID", driverID);

            bool isDeleted = false;

            try
            {
                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                    isDeleted = true;
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsDriversDataAccess Class: {ex.Message}");
                isDeleted = false;
            }
            finally { connection.Close(); }

            return isDeleted;
        }

        public static DataTable GetDriversList()
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            DataTable dt = new DataTable();

            string query = "SELECT * FROM Drivers_View";

            SqlCommand cmd = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                    dt.Load(reader);

                reader.Close();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsDriversDataAccess Class: {ex.Message}");
                dt = null;
            }
            finally { connection.Close(); }

            return dt;
        }

    }
}
