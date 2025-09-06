using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccess
{
    public static class clsLDL_ApplicationsDataAccess
    {
        public static bool Get_LDL_ApplicationInfoB_LDL_AppID(int LDL_AppID, ref int applicationID, ref int licenseClassID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @LDL_AppID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LDL_AppID", LDL_AppID);

            bool isFound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    applicationID = (int)reader["ApplicationID"];
                    licenseClassID = (int)reader["LicenseClassID"];


                    isFound = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsLDL_ApplicationsDataAccess Class: {ex.Message}");
                isFound = false;
            }
            finally { connection.Close(); }

            return isFound;
        }

        public static int New_LDL_Application(int applicationID, int licenseClassID)
        {
            // this function will return the new LDL_ApplicationID if succeeded or -1 if not
            int LDL_AppID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO 
                                LocalDrivingLicenseApplications(ApplicationID, LicenseClassID)
                                VALUES (@ApplicationID, @LicenseClassID);
                                SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", applicationID);
            command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    LDL_AppID = insertedID;
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsLDL_ApplicationsDataAccess Class: {ex.Message}");
                LDL_AppID = -1;
            }
            finally { connection.Close(); }

            return LDL_AppID;
        }

        public static bool Update_LDL_ApplicationInfo(int LDL_AppID, int applicationID, int licenseClassID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE LocalDrivingLicenseApplications 
                                SET ApplicationID = @ApplicationID,
                                LicenseClassID = @LicenseClassID
                            WHERE LocalDrivingLicenseApplicationID = @LDL_AppID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", applicationID);
            command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);
            command.Parameters.AddWithValue("@LDL_AppID", LDL_AppID);

            int rowsAffected = 0;

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsLDL_ApplicationsDataAccess Class: {ex.Message}");
                rowsAffected = 0;
            }
            finally { connection.Close(); }

            return rowsAffected > 0;
        }

        public static bool DeleteApplication(int LDL_ApplicationID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"DELETE LocalDrivingLicenseApplications  WHERE LocalDrivingLicenseApplicationID = @LDL_AppID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LDL_AppID", LDL_ApplicationID);

            int rowsAffected = 0;

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsLDL_ApplicationsDataAccess Class: {ex.Message}");
                rowsAffected = 0;
            }
            finally { connection.Close(); }

            return rowsAffected > 0;
        }

        public static int isPersonHasActiveApplicationWithSameClass(int applicantPersonID, int licenseClassID)
        {
            SqlConnection connection = new SqlConnection (clsDataAccessSettings.ConnectionString);

            string query = @"SELECT Applications.ApplicationID
                                FROM People INNER JOIN
                                    Applications ON People.PersonID = Applications.ApplicantPersonID INNER JOIN
                                    LocalDrivingLicenseApplications ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID INNER JOIN
                                    ApplicationTypes ON Applications.ApplicationTypeID = ApplicationTypes.ApplicationTypeID INNER JOIN
                                    ApplicationStatuses ON Applications.ApplicationStatusID = ApplicationStatuses.StatusID INNER JOIN
                                    LicenseClasses ON LocalDrivingLicenseApplications.LicenseClassID = LicenseClasses.LicenseClassID
                                WHERE (Applications.ApplicantPersonID = @applicantPersonID AND LicenseClasses.LicenseClassID = @licenseClassID AND (ApplicationStatuses.StatusID = 1))"; // 1 == New Application
        
            SqlCommand command = new SqlCommand (query, connection);
            command.Parameters.AddWithValue("@applicantPersonID", applicantPersonID);
            command.Parameters.AddWithValue("@licenseClassID", licenseClassID);

            int ApplicationID = -1;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                
                if(reader.Read())
                    ApplicationID = (int)reader["ApplicationID"];
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsLDL_ApplicationsDataAccess Class: {ex.Message}");
                ApplicationID = -1;
            }
            finally { connection.Close(); }

            return ApplicationID;
        }

        public static DataTable Get_LDL_ApplicationsList()
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            DataTable dt = new DataTable();

            string query = "SELECT * FROM LDL_Applications_View";

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
                clsErrorsLogger.LogError($"An error occurred in clsLDL_ApplicationsDataAccess Class: {ex.Message}");
                dt = null;
            }
            finally { connection.Close(); }

            return dt;
        }

    }
}
