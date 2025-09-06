using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public static class clsLicensesDataAccess
    {
        public static bool GetLicenseInfoByLicenseID(int licenseID, ref int applicationID, ref int driverID, ref int licenseClassID,
                                                ref DateTime issueDate, ref DateTime expiryDate, ref string notes, ref decimal paidFees,
                                                ref bool isActive, ref byte issueReason, ref int createdByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Licenses WHERE licenseID = @licenseID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@licenseID", licenseID);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    applicationID = (int)reader["applicationID"];
                    driverID = (int)reader["driverID"];
                    licenseClassID = (int)reader["licenseClassID"];
                    createdByUserID = (int)reader["createdByUserID"];
                    issueReason = (byte)reader["issueReason"];
                    issueDate = (DateTime)reader["issueDate"];
                    expiryDate = (DateTime)reader["expiryDate"];
                    isActive = (bool)reader["isActive"];
                    paidFees = (decimal)reader["paidFees"];

                    // Handling the fields that accept null 
                    if (reader["notes"] != DBNull.Value)
                        notes = (string)reader["notes"];
                    else
                        notes = string.Empty;

                    isFound = true;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsLicensesDataAccess Class: {ex.Message}");
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetLicenseInfoByApplicationID(ref int licenseID, int applicationID, ref int driverID, ref int licenseClassID,
                                               ref DateTime issueDate, ref DateTime expiryDate, ref string notes, ref decimal paidFees,
                                               ref bool isActive, ref byte issueReason, ref int createdByUserID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Licenses WHERE applicationID = @applicationID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@applicationID", applicationID);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    licenseID = (int)reader["licenseID"];
                    driverID = (int)reader["driverID"];
                    licenseClassID = (int)reader["licenseClassID"];
                    createdByUserID = (int)reader["createdByUserID"];
                    issueReason = (byte)reader["issueReason"];
                    issueDate = (DateTime)reader["issueDate"];
                    expiryDate = (DateTime)reader["expiryDate"];
                    isActive = (bool)reader["isActive"];
                    paidFees = (decimal)reader["paidFees"];

                    // Handling the fields that accept null 
                    if (reader["notes"] != DBNull.Value)
                        notes = (string)reader["notes"];
                    else
                        notes = string.Empty;

                    isFound = true;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsLicensesDataAccess Class: {ex.Message}");
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int CreateNewLicense(int applicationID, int driverID, int licenseClassID, DateTime issueDate, DateTime expiryDate,
                                            string notes, decimal paidFees, bool isActive, byte issueReason, int createdByUserID)

        {
            //this function will return the new contact id if succeeded and -1 if not.
            int licenseID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO
                                  Licenses(applicationID, driverID, licenseClassID, issueDate, expiryDate,
                                            notes, paidFees, isActive, issueReason, createdByUserID)
                                  VALUES (@applicationID, @driverID, @licenseClassID, @issueDate, @expiryDate,
                                            @notes, @paidFees, @isActive, @issueReason, @createdByUserID);
                             SELECT SCOPE_IDENTITY();";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@applicationID", applicationID);
            command.Parameters.AddWithValue("@driverID", driverID);
            command.Parameters.AddWithValue("@licenseClassID", licenseClassID);
            command.Parameters.AddWithValue("@issueDate", issueDate);
            command.Parameters.AddWithValue("@expiryDate", expiryDate);
            command.Parameters.AddWithValue("@paidFees", paidFees);
            command.Parameters.AddWithValue("@isActive", isActive);
            command.Parameters.AddWithValue("@issueReason", issueReason);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);

            // Handling the fields that accept null
            if (string.IsNullOrEmpty(notes))
                command.Parameters.AddWithValue("@notes", DBNull.Value);
            else
                command.Parameters.AddWithValue("@notes", notes);

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
                clsErrorsLogger.LogError($"An error occurred in clsLicensesDataAccess Class: {ex.Message}");
                licenseID = -1;
            }
            finally { connection.Close(); }

            return licenseID;
        }
    
        public static bool UpdateLicense(int licenseID, int applicationID, int driverID, int licenseClassID, DateTime issueDate, DateTime expiryDate,
                                            string notes, decimal paidFees, bool isActive, byte issueReason, int createdByUserID)

        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE Licenses
                                SET applicationID = @applicationID,
                                driverID = @driverID,
                                licenseClassID = @licenseClassID,
                                issueDate = @issueDate,
                                expiryDate = @expiryDate,
                                notes = @notes,
                                paidFees = @paidFees,
                                isActive = @isActive,
                                issueReason = @issueReason,
                                createdByUserID = @createdByUserID
                                WHERE licenseID = @licenseID
                            ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@licenseID", licenseID);
            command.Parameters.AddWithValue("@applicationID", applicationID);
            command.Parameters.AddWithValue("@driverID", driverID);
            command.Parameters.AddWithValue("@licenseClassID", licenseClassID);
            command.Parameters.AddWithValue("@issueDate", issueDate);
            command.Parameters.AddWithValue("@expiryDate", expiryDate);
            command.Parameters.AddWithValue("@paidFees", paidFees);
            command.Parameters.AddWithValue("@isActive", isActive);
            command.Parameters.AddWithValue("@issueReason", issueReason);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);

            // Handling the fields that accept null
            if (string.IsNullOrEmpty(notes))
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
                clsErrorsLogger.LogError($"An error occurred in clsLicensesDataAccess Class: {ex.Message}");
                rowsAffected = 0;
            }
            finally { connection.Close(); }

            return (rowsAffected > 0);
        }

        public static bool isApplicationConnectedToLicense(int applicationID)
        {
            bool isExist = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT 1 AS Found FROM Licenses WHERE ApplicationID = @applicationID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@applicationID", applicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                isExist = reader.HasRows;
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsLicensesDataAccess Class: {ex.Message}");
                isExist = false;
            }
            finally { connection.Close(); }
             
            return isExist;
        }

        public static bool isPersonHaveLicenseWithSameClass(int licenseClassID, int driverID)
        {
            bool isExist = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT 1 AS Found 
                             FROM Licenses 
                             WHERE driverID = @driverID AND licenseClassID = @licenseClassID AND GetDate() between IssueDate and ExpiryDate AND IsActive = 1";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@driverID", driverID);
            command.Parameters.AddWithValue("@licenseClassID", licenseClassID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                isExist = reader.HasRows;
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsLicensesDataAccess Class: {ex.Message}");
                isExist = false;
            }
            finally { connection.Close(); }

            return isExist;
        }

        public static DataTable GetLocalLicenses(int driverID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            DataTable dt = new DataTable();

            string query = @"SELECT Licenses.LicenseID,
                                    Licenses.ApplicationID, 
                                    LicenseClasses.ClassName, 
                                    FORMAT(Licenses.IssueDate, 'dd,MM,yyyy') AS IssueDate,
                                    FORMAT(Licenses.ExpiryDate, 'dd,MM,yyyy') AS ExpiryDate,
                                    Licenses.IsActive
                            FROM Licenses INNER JOIN 
	                                LicenseClasses ON Licenses.LicenseClassID = LicenseClasses.LicenseClassID
	                                WHERE Licenses.DriverID = @driverID
                                    ORDER BY Licenses.IssueDate DESC";

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
                clsErrorsLogger.LogError($"An error occurred in clsLicensesDataAccess Class: {ex.Message}");
                dt = null;
            }
            finally { connection.Close(); }

            return dt;
        }

    }
}
