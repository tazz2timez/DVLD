using System;
using System.Data.SqlClient;
using System.Data;

namespace DVLD_DataAccess
{
    public static class clsInternationalLicensesDataAccess
    {
        public static bool GetLicenseInfoByLicenseID(int internationalLicenseID, ref int applicationID, ref int driverID, ref int issuedUsingLocalLicenseID,
                                                        ref DateTime issueDate, ref DateTime expiryDate, ref bool isActive, ref int createdByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM InternationalLicenses WHERE internationalLicenseID = @internationalLicenseID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@internationalLicenseID", internationalLicenseID);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    applicationID = (int)reader["applicationID"];
                    driverID = (int)reader["driverID"];
                    issuedUsingLocalLicenseID = (int)reader["issuedUsingLocalLicenseID"];
                    createdByUserID = (int)reader["createdByUserID"];
                    issueDate = (DateTime)reader["issueDate"];
                    expiryDate = (DateTime)reader["expiryDate"];
                    isActive = (bool)reader["isActive"];

                    isFound = true;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsInternationalLicensesDataAccess Class: {ex.Message}");
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetLicenseInfoByLocalLicenseID(ref int internationalLicenseID, ref int applicationID, ref int driverID, int issuedUsingLocalLicenseID,
                                                            ref DateTime issueDate, ref DateTime expiryDate, ref bool isActive, ref int createdByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM InternationalLicenses WHERE issuedUsingLocalLicenseID = @issuedUsingLocalLicenseID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@issuedUsingLocalLicenseID", issuedUsingLocalLicenseID);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    internationalLicenseID = (int)reader["internationalLicenseID"];
                    applicationID = (int)reader["applicationID"];
                    driverID = (int)reader["driverID"];
                    createdByUserID = (int)reader["createdByUserID"];
                    issueDate = (DateTime)reader["issueDate"];
                    expiryDate = (DateTime)reader["expiryDate"];
                    isActive = (bool)reader["isActive"];

                    isFound = true;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsInternationalLicensesDataAccess Class: {ex.Message}");
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetLicenseInfoByApplicationID(ref int internationalLicenseID, int applicationID, ref int driverID, ref int issuedUsingLocalLicenseID,
                                                            ref DateTime issueDate, ref DateTime expiryDate, ref bool isActive, ref int createdByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM InternationalLicenses WHERE applicationID = @applicationID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@applicationID", applicationID);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    internationalLicenseID = (int)reader["internationalLicenseID"];
                    driverID = (int)reader["driverID"];
                    issuedUsingLocalLicenseID = (int)reader["issuedUsingLocalLicenseID"];
                    createdByUserID = (int)reader["createdByUserID"];
                    issueDate = (DateTime)reader["issueDate"];
                    expiryDate = (DateTime)reader["expiryDate"];
                    isActive = (bool)reader["isActive"];

                    isFound = true;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsInternationalLicensesDataAccess Class: {ex.Message}");
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int CreateNewLicense(int applicationID, int driverID, int issuedUsingLocalLicenseID, DateTime issueDate,
                                                DateTime expiryDate, bool isActive, int createdByUserID)

        {
            //this function will return the new contact id if succeeded and -1 if not.
            int licenseID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO
                               InternationalLicenses(applicationID, driverID, issuedUsingLocalLicenseID, issueDate, expiryDate, isActive, createdByUserID)
                               VALUES (@applicationID, @driverID, @issuedUsingLocalLicenseID, @issueDate, @expiryDate, @isActive, @createdByUserID);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@applicationID", applicationID);
            command.Parameters.AddWithValue("@driverID", driverID);
            command.Parameters.AddWithValue("@issuedUsingLocalLicenseID", issuedUsingLocalLicenseID);
            command.Parameters.AddWithValue("@issueDate", issueDate);
            command.Parameters.AddWithValue("@expiryDate", expiryDate);
            command.Parameters.AddWithValue("@isActive", isActive);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    licenseID = insertedID;
                }
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsInternationalLicensesDataAccess Class: {ex.Message}");
                licenseID = -1;
            }
            finally { connection.Close(); }

            return licenseID;
        }

        public static bool UpdateLicense(int internationalLicenseID, int applicationID, int driverID, int issuedUsingLocalLicenseID,
                                                DateTime issueDate, DateTime expiryDate, bool isActive, int createdByUserID)

        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE InternationalLicenses
                                SET applicationID = @applicationID,
                                driverID = @driverID,
                                issuedUsingLocalLicenseID = @issuedUsingLocalLicenseID,
                                issueDate = @issueDate,
                                expiryDate = @expiryDate,
                                isActive = @isActive,
                                createdByUserID = @createdByUserID
                                WHERE internationalLicenseID = @internationalLicenseID
                            ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@internationalLicenseID", internationalLicenseID);
            command.Parameters.AddWithValue("@applicationID", applicationID);
            command.Parameters.AddWithValue("@driverID", driverID);
            command.Parameters.AddWithValue("@issuedUsingLocalLicenseID", issuedUsingLocalLicenseID);
            command.Parameters.AddWithValue("@issueDate", issueDate);
            command.Parameters.AddWithValue("@expiryDate", expiryDate);
            command.Parameters.AddWithValue("@isActive", isActive);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);

            int rowsAffected = 0;

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsInternationalLicensesDataAccess Class: {ex.Message}");
                rowsAffected = 0;
            }
            finally { connection.Close(); }

            return (rowsAffected > 0);
        }

        public static DataTable GetInternationalLicenses(int driverID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            DataTable dt = new DataTable();

            string query = @"SELECT InternationalLicenseID,
                                    ApplicationID, 
                                    DriverID, 
                                    IssuedUsingLocalLicenseID, 
                                    FORMAT(IssueDate, 'dd,MM,yyyy') AS IssueDate,
                                    FORMAT(ExpiryDate, 'dd,MM,yyyy') AS ExpiryDate,
                                    IsActive
                            FROM InternationalLicenses  
	                                WHERE DriverID = @driverID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@driverID", driverID);

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
                clsErrorsLogger.LogError($"An error occurred in clsInternationalLicensesDataAccess Class: {ex.Message}");
                dt = null;
            }
            finally { connection.Close(); }

            return dt;
        }

        public static DataTable GetInternationalLicensesList()
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            DataTable dt = new DataTable();

            string query = @"SELECT InternationalLicenseID,
                                    ApplicationID, 
                                    DriverID, 
                                    IssuedUsingLocalLicenseID, 
                                    FORMAT(IssueDate, 'dd,MM,yyyy') AS IssueDate,
                                    FORMAT(ExpiryDate, 'dd,MM,yyyy') AS ExpiryDate,
                                    IsActive
                            FROM InternationalLicenses";

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
                clsErrorsLogger.LogError($"An error occurred in clsInternationalLicensesDataAccess Class: {ex.Message}");
                dt = null;
            }
            finally { connection.Close(); }

            return dt;
        }

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            int InternationalLicenseID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"  
                            SELECT Top 1 InternationalLicenseID
                            FROM InternationalLicenses 
                            where DriverID = @DriverID and GetDate() between IssueDate and ExpiryDate AND IsActive = 1
                            order by ExpiryDate Desc;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    InternationalLicenseID = insertedID;
                }
            }

            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsInternationalLicensesDataAccess Class: {ex.Message}");
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }


            return InternationalLicenseID;
        }

    }
}