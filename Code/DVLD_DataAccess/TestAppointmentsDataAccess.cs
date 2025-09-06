using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public static class clsTestAppointmentsDataAccess
    {
        public static bool GetTestAppointmentInfoByID(int testAppointmentID, ref int testTypeID, ref int LDL_AppID, ref DateTime appointmentDate,
                                                            ref decimal paidFees, ref int createdByUserID, ref bool isLocked)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = $"SELECT * FROM TestAppointments WHERE TestAppointmentID = @testAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@testAppointmentID", testAppointmentID);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    testTypeID = (int)reader["testTypeID"];
                    LDL_AppID = (int)reader["LocalDrivingLicenseApplicationID"];
                    createdByUserID = (int)reader["CreatedByUserID"];
                    isLocked = (bool)reader["isLocked"];
                    paidFees = (decimal)reader["paidFees"];
                    appointmentDate = (DateTime)reader["appointmentDate"];


                    isFound = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsTestAppointmentsDataAccess Class: {ex.Message}");
                isFound = false;
            }
            finally { connection.Close(); }

            return isFound;
        }

        public static bool UpdateTestAppointmentInfo(int testAppointmentID, int testTypeID, int LDL_AppID, DateTime appointmentDate,
                                                            decimal paidFees, int createdByUserID, bool isLocked)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE TestAppointments
                                SET TestTypeID = @testTypeID,
                                LocalDrivingLicenseApplicationID = @LDL_AppID,
                                appointmentDate = @appointmentDate,
                                paidFees = @paidFees,
                                CreatedByUserID = @createdByUserID,
                                isLocked = @isLocked
                                WHERE TestAppointmentID = @testAppointmentID
                          ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@testAppointmentID", testAppointmentID);
            command.Parameters.AddWithValue("@testTypeID", testTypeID);
            command.Parameters.AddWithValue("@LDL_AppID", LDL_AppID);
            command.Parameters.AddWithValue("@paidFees", paidFees);
            command.Parameters.AddWithValue("@appointmentDate", appointmentDate);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);
            command.Parameters.AddWithValue("@isLocked", isLocked);

            int rowsAffected = 0;

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsTestAppointmentsDataAccess Class: {ex.Message}");
                rowsAffected = 0;
            }
            finally { connection.Close(); }

            return (rowsAffected > 0);
        }

        public static int AddNewTestAppointment(int testTypeID, int LDL_AppID, DateTime appointmentDate,
                                                            decimal paidFees, int createdByUserID, bool isLocked)
        {
            //this function will return the new appointment id if succeeded and -1 if not.
            int appointmentID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT TestAppointments
                                (testTypeID, LocalDrivingLicenseApplicationID, appointmentDate, paidFees, createdByUserID, isLocked)
                                VALUES (@testTypeID, @LDL_AppID, @appointmentDate, @paidFees, @createdByUserID, @isLocked);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@testTypeID", testTypeID);
            command.Parameters.AddWithValue("@LDL_AppID", LDL_AppID);
            command.Parameters.AddWithValue("@paidFees", paidFees);
            command.Parameters.AddWithValue("@appointmentDate", appointmentDate);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);
            command.Parameters.AddWithValue("@isLocked", isLocked);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    appointmentID = insertedID;
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsTestAppointmentsDataAccess Class: {ex.Message}");
                appointmentID = -1;
            }
            finally { connection.Close(); }

            return appointmentID;
        }

        public static DataTable GetTestAppointmentsListPerTest(int LDL_AppID, int testTypeID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            DataTable dt = new DataTable();

            string query = @"SELECT 
                                TestAppointmentID,
                                AppointmentDate,
                                PaidFees,
                                IsLocked 
                            FROM TestAppointments WHERE LocalDrivingLicenseApplicationID = @LDL_AppID AND TestTypeID = @testTypeID
                            ORDER BY AppointmentDate DESC";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@LDL_AppID", LDL_AppID);
            cmd.Parameters.AddWithValue("@testTypeID", testTypeID);

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
                clsErrorsLogger.LogError($"An error occurred in clsTestAppointmentsDataAccess Class: {ex.Message}");
                dt = null;
            }
            finally { connection.Close(); }

            return dt;
        }

        public static bool HasActiveAppointment(int LDL_AppID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT HasActiveAppointment = 1 FROM TestAppointments WHERE (LocalDrivingLicenseApplicationID = @LDL_AppID AND IsLocked = 0)";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LDL_AppID", LDL_AppID);

            bool isExist = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isExist = reader.Read();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsTestAppointmentsDataAccess Class: {ex.Message}");
                isExist = false;
            }
            finally { connection.Close(); }

            return isExist;
        
        }

        public static bool HasPassedTest(int LDL_AppID, int testTypeID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT HasPassedTest = 1
                                FROM TestAppointments INNER JOIN
                                    Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID INNER JOIN
                                    LocalDrivingLicenseApplications ON TestAppointments.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID INNER JOIN
                                    TestTypes ON TestAppointments.TestTypeID = TestTypes.TestTypeID
	                            WHERE Tests.TestResult = 1 AND
                                    TestTypes.TestTypeID = @testTypeID AND 
                                    LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LDL_AppID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LDL_AppID", LDL_AppID);
            command.Parameters.AddWithValue("@testTypeID", testTypeID);

            bool isPassed = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isPassed = reader.Read();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsTestAppointmentsDataAccess Class: {ex.Message}");
                isPassed = false;
            }
            finally { connection.Close(); }

            return isPassed;

        }

        public static bool HasFailedTest(int LDL_AppID, int testTypeID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT HasFailedTest = 1
                                FROM TestAppointments INNER JOIN
                                    Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID INNER JOIN
                                    LocalDrivingLicenseApplications ON TestAppointments.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID INNER JOIN
                                    TestTypes ON TestAppointments.TestTypeID = TestTypes.TestTypeID
	                            WHERE Tests.TestResult = 0 AND
                                    TestTypes.TestTypeID = @testTypeID AND 
                                    LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LDL_AppID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LDL_AppID", LDL_AppID);
            command.Parameters.AddWithValue("@testTypeID", testTypeID);

            bool isFailed = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isFailed = reader.Read();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsTestAppointmentsDataAccess Class: {ex.Message}");
                isFailed = false;
            }
            finally { connection.Close(); }

            return isFailed;
        }

    }
}
