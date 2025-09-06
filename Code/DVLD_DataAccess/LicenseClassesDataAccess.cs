using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public static class clsLicenseClassesDataAccess
    {
        public static DataTable GetLicenseClassesList()
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM LicenseClasses";

            SqlCommand cmd = new SqlCommand(query, connection);

            DataTable countries = new DataTable();

            try
            {
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                    countries.Load(reader);
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsLicenseClassesDataAccess Class: {ex.Message}");  
                countries = null;
            }
            finally { connection.Close(); }

            return countries;
        }

        public static bool FindLicenseClassByID(int licenseClassID, ref string licenseClassName, ref string classDescription, 
                                                   ref byte minimumAllowedAge, ref byte defaultValidityLength, ref decimal classFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;
                    licenseClassName = (string)reader["ClassName"];
                    classDescription = (string)reader["classDescription"];
                    minimumAllowedAge = (byte)reader["minimumAllowedAge"];
                    defaultValidityLength = (byte)reader["defaultValidityLength"];
                    classFees = (decimal)reader["classFees"];
                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsLicenseClassesDataAccess Class: {ex.Message}");
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
        
        public static bool FindLicenseClassByName(ref int licenseClassID, string licenseClassName, ref string classDescription,
                                                   ref byte minimumAllowedAge, ref byte defaultValidityLength, ref decimal classFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM LicenseClasses WHERE ClassName = @licenseClassName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@licenseClassName", licenseClassName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;
                    licenseClassID = (int)reader["licenseClassID"];
                    classDescription = (string)reader["classDescription"];
                    minimumAllowedAge = (byte)reader["minimumAllowedAge"];
                    defaultValidityLength = (byte)reader["defaultValidityLength"];
                    classFees = (decimal)reader["classFees"];
                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsLicenseClassesDataAccess Class: {ex.Message}");
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

    }
}
