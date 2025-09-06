using System;
using System.Data.SqlClient;
using System.Data;

namespace DVLD_DataAccess
{
    public static class clsDetainedLicensesDataAccess
    {
        public static bool GetDetainedLicenseInfoByDetainID(int detainID, ref int licenseID, ref DateTime detainDate, ref decimal fineFees,
                                                                ref int detainedByUserID, ref bool isReleased, ref DateTime releaseDate,
                                                                ref int releasedByUserID, ref int releaseApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM DetainedLicenses WHERE DetainID = @detainID";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@detainID", detainID);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    licenseID = (int)reader["licenseID"];
                    detainDate = (DateTime)reader["detainDate"];
                    fineFees = (decimal)reader["fineFees"];
                    detainedByUserID = (int)reader["detainedByUserID"];
                    isReleased = (bool)reader["isReleased"];

                    // Handling the fields that accept null 
                    if (reader["releaseDate"] != DBNull.Value)
                        releaseDate = (DateTime)reader["releaseDate"];
                    else
                        releaseDate = DateTime.Now;

                    if (reader["releasedByUserID"] != DBNull.Value)
                        releasedByUserID = (int)reader["releasedByUserID"];
                    else
                        releasedByUserID = -1;

                    if (reader["releaseApplicationID"] != DBNull.Value)
                        releaseApplicationID = (int)reader["releaseApplicationID"];
                    else
                        releaseApplicationID = -1;

                    isFound = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsDetainedLicensesDataAccess Class: {ex.Message}");
                isFound = false;
            }
            finally { connection.Close(); }

            return isFound;
        }

        public static bool GetDetainedLicenseInfoByLicenseID(ref int detainID, int licenseID, ref DateTime detainDate, ref decimal fineFees,
                                                                ref int detainedByUserID, ref bool isReleased, ref DateTime releaseDate,
                                                                ref int releasedByUserID, ref int releaseApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM DetainedLicenses WHERE LicenseID = @licenseID AND IsReleased = 'False'";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@licenseID", licenseID);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    detainID = (int)reader["detainID"];
                    detainDate = (DateTime)reader["detainDate"];
                    fineFees = (decimal)reader["fineFees"];
                    detainedByUserID = (int)reader["detainedByUserID"];
                    isReleased = (bool)reader["isReleased"];

                    // Handling the fields that accept null 
                    if (reader["releaseDate"] != DBNull.Value)
                        releaseDate = (DateTime)reader["releaseDate"];
                    else
                        releaseDate = DateTime.Now;

                    if (reader["releasedByUserID"] != DBNull.Value)
                        releasedByUserID = (int)reader["releasedByUserID"];
                    else
                        releasedByUserID = -1;

                    if (reader["releaseApplicationID"] != DBNull.Value)
                        releaseApplicationID = (int)reader["releaseApplicationID"];
                    else
                        releaseApplicationID = -1;

                    isFound = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsDetainedLicensesDataAccess Class: {ex.Message}");
                isFound = false;
            }
            finally { connection.Close(); }

            return isFound;
        }

        public static int DetainLicense(int licenseID, DateTime detainDate, decimal fineFees, int detainedByUserID)
        {
            int detainID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO 
                                DetainedLicenses (LicenseID, DetainDate, FineFees, DetainedByUserID, IsReleased,
                                                    ReleaseDate, ReleasedByUserID, ReleaseApplicationID) 
                                Values (@licenseID, @detainDate, @fineFees, @detainedByUserID, 'False',
                                            @releaseDate, @releasedByUserID, @releaseApplicationID);
                                SELECT SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@licenseID", licenseID);
            cmd.Parameters.AddWithValue("@detainDate", detainDate);
            cmd.Parameters.AddWithValue("@fineFees", fineFees);
            cmd.Parameters.AddWithValue("@detainedByUserID", detainedByUserID);
            cmd.Parameters.AddWithValue("@releaseDate", DBNull.Value);
            cmd.Parameters.AddWithValue("@releasedByUserID", DBNull.Value);
            cmd.Parameters.AddWithValue("@releaseApplicationID", DBNull.Value);

            try
            {
                connection.Open();

                object result = cmd.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    detainID = insertedID;
                }
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsDetainedLicensesDataAccess Class: {ex.Message}");
                detainID = -1;
            }
            finally { connection.Close(); }

            return detainID;
        }

        public static bool ReleaseLicense(int licenseID, DateTime releaseDate, int releasedByUserID, int releaseApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE DetainedLicenses
                                SET isReleased = 'True',
                                releaseDate = @releaseDate,
                                releasedByUserID = @releasedByUserID,
                                releaseApplicationID = @releaseApplicationID
                                WHERE licenseID = @licenseID AND isReleased = 'False'
                            ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@licenseID", licenseID);
            command.Parameters.AddWithValue("@releaseDate", releaseDate);
            command.Parameters.AddWithValue("@releasedByUserID", releasedByUserID);
            command.Parameters.AddWithValue("@releaseApplicationID", releaseApplicationID);

            int rowsAffected = 0;

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsDetainedLicensesDataAccess Class: {ex.Message}");
                rowsAffected = 0;
            }
            finally { connection.Close(); }

            return (rowsAffected > 0);
        }

        public static bool isLicenseDetained(int licenseID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Select isDetained = 1 From DetainedLicenses WHERE LicenseID = @licenseID AND IsReleased = 'False'";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@licenseID", licenseID);

            bool isDetained = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isDetained = reader.Read();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsDetainedLicensesDataAccess Class: {ex.Message}");
                isDetained = false;
            }
            finally { connection.Close(); }

            return isDetained;
        }

        public static DataTable GetDetainedLicensesList()
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            DataTable dt = new DataTable();

            string query = "SELECT * FROM DetainedLicenses_View ORDER BY DetainID";

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
                clsErrorsLogger.LogError($"An error occurred in clsDetainedLicensesDataAccess Class: {ex.Message}");
                dt = null;
            }
            finally { connection.Close(); }

            return dt;
        }

    }
}
