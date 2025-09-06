using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DVLD_DataAccess
{
    public static class clsUsersDataAccess
    {
        public static bool GetUserInfoByUserID(int userID, ref int personID,  ref string username, ref string password, ref bool isActive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Users WHERE userID = @userID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userID", userID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.Read())
                {
                    personID = (int)reader["personID"];
                    username = (string)reader["username"];
                    password = (string)reader["password"];
                    isActive = (bool)reader["isActive"];

                    isFound = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsUsersDataAccess Class: {ex.Message}");
                isFound = false;
            }
            finally { connection.Close(); }

            return isFound;
        }

        public static bool GetUserInfoByUserName(ref int userID, ref int personID, string username, ref string password, ref bool isActive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Users WHERE username = @username";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@username", username);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    userID = (int)reader["userID"];
                    personID = (int)reader["personID"];
                    password = (string)reader["password"];
                    isActive = (bool)reader["isActive"];

                    isFound = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsUsersDataAccess Class: {ex.Message}");
                isFound = false;
            }
            finally { connection.Close(); }

            return isFound;
        }

        public static int AddNewUser(int personID, string username, string password, bool isActive)
        {
            // this function will return the new personID if succeeded or -1 if not
            int userID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO Users(personID, username, password, isActive)
                                VALUES (@personID, @username, @password, @isActive);
                                SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@personID", personID);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@isActive", isActive);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    userID = insertedID;
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsUsersDataAccess Class: {ex.Message}");
                userID = -1;
            }
            finally { connection.Close(); }

            return userID;
        }

        public static bool UpdateUserInfo(int userID, int personID, string username, string password, bool isActive)
        {

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE Users 
                                SET personID = @personID, 
                                    username = @username,
                                    password = @password,
                                    isActive = @isActive
                                WHERE userID = @userID
                            ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userID", userID);
            command.Parameters.AddWithValue("@personID", personID);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@isActive", isActive);


            int rowsAffected = 0;

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsUsersDataAccess Class: {ex.Message}");
                rowsAffected = 0;
            }
            finally { connection.Close(); }

            return (rowsAffected > 0);
        }

        public static bool DeleteUser(int userID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "DELETE Users WHERE userID = @userID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userID", userID);

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
                clsErrorsLogger.LogError($"An error occurred in clsUsersDataAccess Class: {ex.Message}");
                isDeleted = false;  
            }
            finally { connection.Close(); }

            return isDeleted;
        }

        public static bool DeleteUser(string username)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "DELETE Users WHERE username = @username";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@username", username);

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
                clsErrorsLogger.LogError($"An error occurred in clsUsersDataAccess Class: {ex.Message}");
                isDeleted = false;
            }
            finally { connection.Close(); }

            return isDeleted;
        }

        public static bool isUserExist(string username, string password)
        {
            bool isExist = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT 1 AS Found 
                             FROM Users 
                             WHERE username = @username AND password = @password";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

           try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                isExist = reader.HasRows;
            }
            catch(Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsUsersDataAccess Class: {ex.Message}");
                isExist = false;
            }
            finally { connection.Close(); }

            return isExist;
        }

        public static bool isUserExist(string username)
        {
            bool isExist = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT 1 AS Found 
                             FROM Users 
                             WHERE username = @username";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@username", username);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                isExist = reader.HasRows;
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsUsersDataAccess Class: {ex.Message}");
                isExist = false;
            }
            finally { connection.Close(); }

            return isExist;
        }

        public static bool isUserExist(int userID)
        {
            bool isExist = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT 1 AS Found 
                             FROM Users 
                             WHERE userID = @userID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userID", userID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                isExist = reader.HasRows;
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsUsersDataAccess Class: {ex.Message}");
                isExist = false;
            }
            finally { connection.Close(); }

            return isExist;
        }

        public static bool isPersonAlreadyUser(int personID)
        {
            bool isExist = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT 1 AS Found 
                             FROM Users 
                             WHERE personID = @personID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@personID", personID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                isExist = reader.HasRows;
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsUsersDataAccess Class: {ex.Message}");
                isExist = false;
            }
            finally { connection.Close(); }

            return isExist;
        }

        public static DataTable GetUsersList()
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            DataTable dt = new DataTable();

            string query = "SELECT * FROM Users_View";

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
                clsErrorsLogger.LogError($"An error occurred in clsUsersDataAccess Class: {ex.Message}");
                dt = null;
            }
            finally { connection.Close(); }

            return dt;
        }

    }
}
