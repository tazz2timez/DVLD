using System;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public static class clsApplicationsDataAccess
    {
        public static bool GetApplicationInfoByID(int appID, ref int applicantPersonID, ref DateTime appDate, ref int appTypeID, ref int appStatusID,
                                                    ref DateTime lastStatusDate, ref decimal paidFees, ref int createdByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Applications WHERE ApplicationID = @appID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@appID", appID);

            bool isFound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if(reader.Read())
                {
                    appID = (int)reader["applicationID"];
                    applicantPersonID = (int)reader["applicantPersonID"];
                    appStatusID = (int)reader["applicationStatusID"];
                    createdByUserID = (int)reader["createdByUserID"];
                    appTypeID = (int)reader["applicationTypeID"];
                    appDate = (DateTime)reader["applicationDate"];
                    lastStatusDate = (DateTime)reader["lastStatusDate"];
                    paidFees = (decimal)reader["paidFees"];

                    isFound = true;
                }
            }
            catch (Exception ex)
            {
                 clsErrorsLogger.LogError($"An error occurred in clsApplicationsDataAccess Class: {ex.Message}");
                isFound = false;
            }
            finally { connection.Close(); }

            return isFound;
        }

        public static int NewApplication(int applicantPersonID, DateTime appDate, int appTypeID, int appStatusID,
                                                    DateTime lastStatusDate, decimal paidFees, int createdByUserID)
        {
            // this function will return the new personID if succeeded or -1 if not
            int applicationID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO 
                                Applications(ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatusID, LastStatusDate, PaidFees, CreatedByUserID)
                                VALUES (@applicantPersonID, @appDate, @appTypeID, @appStatusID, @lastStatusDate, @paidFees, @createdByUserID);
                                SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@applicantPersonID", applicantPersonID);
            command.Parameters.AddWithValue("@appDate", appDate);
            command.Parameters.AddWithValue("@appTypeID", appTypeID);
            command.Parameters.AddWithValue("@appStatusID", appStatusID);
            command.Parameters.AddWithValue("@lastStatusDate", lastStatusDate);
            command.Parameters.AddWithValue("@paidFees", paidFees);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    applicationID = insertedID;
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsApplicationsDataAccess Class: {ex.Message}");
                applicationID = -1;
            }
            finally { connection.Close(); }

            return applicationID;
        }

        public static bool UpdateApplicationInfo(int appID, int applicantPersonID, DateTime appDate, int appTypeID, int appStatusID,
                                                    DateTime lastStatusDate, decimal paidFees, int createdByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE Applications 
                                SET ApplicantPersonID = @applicantPersonID,
                                ApplicationDate = @appDate,
                                ApplicationTypeID = @appTypeID,
                                ApplicationStatusID = @appStatusID,
                                LastStatusDate = @lastStatusDate,
                                PaidFees = @paidFees,
                                CreatedByUserID = @createdByUserID
                            WHERE ApplicationID = @appID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@appID", appID);
            cmd.Parameters.AddWithValue("@applicantPersonID", applicantPersonID);
            cmd.Parameters.AddWithValue("@appDate", appDate);
            cmd.Parameters.AddWithValue("@appTypeID", appTypeID);
            cmd.Parameters.AddWithValue("@appStatusID", appStatusID);
            cmd.Parameters.AddWithValue("@lastStatusDate", lastStatusDate);
            cmd.Parameters.AddWithValue("@paidFees", paidFees);
            cmd.Parameters.AddWithValue("@createdByUserID", createdByUserID);

            int rowsAffected = 0;

            try
            {
                connection.Open();

                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                 clsErrorsLogger.LogError($"An error occurred in clsApplicationsDataAccess Class: {ex.Message}");
                rowsAffected = 0;
            }
            finally { connection.Close(); }

            return rowsAffected > 0;
        }

        public static bool DeleteApplication(int applicationID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "DELETE Applications WHERE ApplicationID = @applicationID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@applicationID", applicationID);

            int rowsAffected = 0;

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                 clsErrorsLogger.LogError($"An error occurred in clsApplicationsDataAccess Class: {ex.Message}");
                rowsAffected = 0;
            }
            finally { connection.Close(); }

            return rowsAffected > 0;
        }

        public static bool CancelApplication(int applicationID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // Number 2 == Cancel in ApplicationStatuses table *** Number 3 == Completed in ApplicationStatuses table
            string query = @"UPDATE Applications 
                                SET ApplicationStatusID = 2
                                WHERE (ApplicationID = @applicationID AND ApplicationStatusID != 3)";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@applicationID", applicationID);

            int rowsAffected = 0;

            try
            {
                connection.Open();

                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                 clsErrorsLogger.LogError($"An error occurred in clsApplicationsDataAccess Class: {ex.Message}");
                rowsAffected = 0;
            }
            finally { connection.Close(); }

            return rowsAffected > 0;
        }

        public static string GetApplicationStatus(int applicationID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT ApplicationStatuses.StatusName FROM  Applications
	                            INNER JOIN ApplicationStatuses ON Applications.ApplicationStatusID = ApplicationStatuses.StatusID
	                            WHERE (Applications.ApplicationID = @applicationID)";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@applicationID", applicationID);

            string status = string.Empty;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                    status = (string)reader[0];
            }
            catch (Exception ex)
            {
                 clsErrorsLogger.LogError($"An error occurred in clsApplicationsDataAccess Class: {ex.Message}");
                status = string.Empty;
            }
            finally { connection.Close(); }

            return status;
        }

    }
}
