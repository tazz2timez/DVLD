using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public static class clsTestsDataAccess
    {
        public static int GetTotalPassedTests(int LDL_AppID)
        {
            int totalPassedTests = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT COUNT(*) AS TotalPassedTests
                                FROM TestAppointments INNER JOIN
                                     Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                                WHERE  (Tests.TestResult = 1 AND TestAppointments.LocalDrivingLicenseApplicationID = @LDL_AppID)";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@LDL_AppID", LDL_AppID);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                    totalPassedTests = (int)reader["TotalPassedTests"];

                reader.Close();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsTestsDataAccess Class: {ex.Message}");
                totalPassedTests = 0;
            }
            finally { connection.Close(); }

            return totalPassedTests;

        }

        public static bool GetTestInfoByID(int testID, ref int appointmentID, ref bool testResult, ref string notes, ref int createdByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = $"SELECT * FROM Tests WHERE TestID = @TestID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestID", testID);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    appointmentID = (int)reader["TestAppointmentID"];
                    createdByUserID = (int)reader["CreatedByUserID"];
                    testResult = (bool)reader["TestResult"];

                    // Handling the fields that accept null 
                    if (reader["Notes"] != DBNull.Value)
                        notes = (string)reader["Notes"];
                    else
                        notes = string.Empty;

                    isFound = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsTestsDataAccess Class: {ex.Message}");
                isFound = false;
            }
            finally { connection.Close(); }

            return isFound;
        }

        public static bool UpdateTestInfo(int testID, int appointmentID, bool testResult, string notes, int createdByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE Tests
                                SET TestAppointmentID = @appointmentID,
                                TestResult = @testResult,
                                Notes = @notes,
                                CreatedByUserID = @createdByUserID
                                WHERE TestID = @testID
                          ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@testID", testID);
            command.Parameters.AddWithValue("@appointmentID", appointmentID);
            command.Parameters.AddWithValue("@testResult", testResult);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);

            // Handling the fields that accept null
            if (string.IsNullOrWhiteSpace(notes))
                command.Parameters.AddWithValue("@notes", DBNull.Value);
            else
                command.Parameters.AddWithValue("@notes", notes);


            int rowsAffected = 0;

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsTestsDataAccess Class: {ex.Message}");
                rowsAffected = 0;
            }
            finally { connection.Close(); }

            return (rowsAffected > 0);
        }

        public static int AddNewTest(int appointmentID, bool testResult, string notes, int createdByUserID)
        {
            //this function will return the new test id if succeeded and -1 if not.
            int testID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT Tests
                                (TestAppointmentID, testResult, notes, createdByUserID)
                                VALUES (@TestAppointmentID, @testResult, @notes, @createdByUserID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", appointmentID);
            command.Parameters.AddWithValue("@testResult", testResult);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);

            // Handling the fields that accept null
            if (string.IsNullOrWhiteSpace(notes))
                command.Parameters.AddWithValue("@notes", DBNull.Value);
            else
                command.Parameters.AddWithValue("@notes", notes);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    testID = insertedID;
                }
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsTestsDataAccess Class: {ex.Message}");
                testID = -1;
            }
            finally { connection.Close(); }

            return testID;
        }

        public static int CountFailedTests(int testTypeID, int LDL_AppID)
        {
            int totalFailedTests = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT COUNT(*) AS totalFailedTests
                                FROM LocalDrivingLicenseApplications INNER JOIN
                                     Applications ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID INNER JOIN
                                     TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                     Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID INNER JOIN
                                     TestTypes ON TestAppointments.TestTypeID = TestTypes.TestTypeID
				                WHERE TestTypes.TestTypeID = @testTypeID AND
                                      LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LDL_AppID;";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@testTypeID", testTypeID);
            cmd.Parameters.AddWithValue("@LDL_AppID", LDL_AppID);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                    totalFailedTests = (int)reader["TotalFailedTests"];

                reader.Close();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsTestsDataAccess Class: {ex.Message}");
                totalFailedTests = 0;
            }
            finally { connection.Close(); }

            return totalFailedTests;
        }

    }
}
