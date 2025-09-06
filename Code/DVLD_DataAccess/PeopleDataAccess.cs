using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public static class clsPeopleDataAccess
    {
        public static bool GetPersonInfoByID(int personID, ref string nationalNumber, ref string firstName, ref string secondName, ref string thirdName,
                                                ref string lastName, ref DateTime dateOfBirth, ref char gender, ref string address,
                                                ref string phone, ref string email, ref int countryID, ref string imagePath)
        {
            bool isFound = false;

            try
            {
                SqlConnection connection;

                using (connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = $"SELECT * FROM People WHERE PersonID = @PersonID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", personID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                countryID = (int)reader["NationalityCountryID"];
                                nationalNumber = (string)reader["NationalNo"];
                                firstName = (string)reader["firstName"];
                                secondName = (string)reader["secondName"];
                                lastName = (string)reader["lastName"];
                                dateOfBirth = (DateTime)reader["dateOfBirth"];
                                address = (string)reader["address"];
                                address = (string)reader["address"];
                                phone = (string)reader["phone"];

                                gender = Convert.ToChar(reader["gender"]);

                                // Handling the fields that accept null 
                                if (reader["thirdName"] != DBNull.Value)
                                    thirdName = (string)reader["thirdName"];
                                else
                                    thirdName = string.Empty;

                                if (reader["email"] != DBNull.Value)
                                    email = (string)reader["email"];
                                else
                                    email = string.Empty;

                                if (reader["imagePath"] != DBNull.Value)
                                    imagePath = (string)reader["imagePath"];
                                else
                                    imagePath = string.Empty;


                                isFound = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsPeopleDataAccess Class: {ex.Message}");
                isFound = false;
            }

            return isFound;
        }

        public static bool GetPersonInfoByNationalNo(ref int personID, string nationalNumber, ref string firstName, ref string secondName, ref string thirdName,
                                               ref string lastName, ref DateTime dateOfBirth, ref char gender, ref string address,
                                               ref string phone, ref string email, ref int countryID, ref string imagePath)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = $"SELECT * FROM People WHERE NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", nationalNumber);

            bool isFound = false;

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    countryID = (int)reader["NationalityCountryID"];
                    personID = (int)reader["PersonID"];
                    firstName = (string)reader["firstName"];
                    secondName = (string)reader["secondName"];
                    lastName = (string)reader["lastName"];
                    dateOfBirth = (DateTime)reader["dateOfBirth"];
                    address = (string)reader["address"];
                    address = (string)reader["address"];
                    phone = (string)reader["phone"];

                    gender = Convert.ToChar(reader["gender"]);

                    // Handling the fields that accept null 
                    if (reader["thirdName"] != DBNull.Value)
                        thirdName = (string)reader["thirdName"];
                    else
                        thirdName = string.Empty;

                    if (reader["email"] != DBNull.Value)
                        email = (string)reader["email"];
                    else
                        email = string.Empty;

                    if (reader["imagePath"] != DBNull.Value)
                        imagePath = (string)reader["imagePath"];
                    else
                        imagePath = string.Empty;


                    isFound = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsPeopleDataAccess Class: {ex.Message}");
                isFound = false;
            }
            finally { connection.Close(); }

            return isFound;
        }

        public static bool UpdatePersonInfo(int personID, string nationalNumber, string firstName, string secondName, string thirdName,
                                               string lastName, DateTime dateOfBirth, char gender, string address,
                                               string phone, string email, int countryID, string imagePath)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE People
                                SET firstName = @firstName,
                                nationalNo = @nationalNo,
                                secondName = @secondName,
                                thirdName = @thirdName,
                                lastName = @lastName,
                                email = @email,
                                phone = @phone,
                                address = @address,
                                gender = @gender,
                                dateOfBirth = @dateOfBirth,
                                nationalityCountryID = @countryID,
                                imagePath = @imagePath
                                WHERE PersonID = @PersonID
                          ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", personID);
            command.Parameters.AddWithValue("@firstName", firstName);
            command.Parameters.AddWithValue("@secondName", secondName);
            command.Parameters.AddWithValue("@lastName", lastName);
            command.Parameters.AddWithValue("@address", address);
            command.Parameters.AddWithValue("@phone", phone);
            command.Parameters.AddWithValue("@gender", gender);
            command.Parameters.AddWithValue("@nationalNo", nationalNumber);
            command.Parameters.AddWithValue("@countryID", countryID);
            command.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);

            // Handling the fields that accept null
            if(string.IsNullOrEmpty(thirdName))
                command.Parameters.AddWithValue("@thirdName", DBNull.Value);
            else
                command.Parameters.AddWithValue("@thirdName", thirdName);

            if (string.IsNullOrEmpty(email))
                command.Parameters.AddWithValue("@email", DBNull.Value);
            else
                command.Parameters.AddWithValue("@email", email);

            if (string.IsNullOrEmpty(imagePath))
                command.Parameters.AddWithValue("@imagePath", DBNull.Value);
            else
                command.Parameters.AddWithValue("@imagePath", imagePath);

            int rowsAffected = 0;

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsPeopleDataAccess Class: {ex.Message}");
                rowsAffected = 0;
            }
            finally { connection.Close(); }

            return (rowsAffected > 0);
        }

        public static int AddNewPerson(string nationalNumber, string firstName, string secondName, string thirdName,
                                               string lastName, DateTime dateOfBirth, char gender, string address,
                                               string phone, string email, int countryID, string imagePath)
        {
            //this function will return the new contact id if succeeded and -1 if not.
            int personID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO
                                People(nationalNo, firstName, secondName, thirdName,
                                    lastName, dateOfBirth, gender, address,
                                    phone, email, NationalityCountryID, imagePath)

                                VALUES (@nationalNo, @firstName, @secondName, @thirdName,
                                    @lastName, @dateOfBirth, @gender, @address,
                                    @phone, @email, @countryID, @imagePath);

                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@firstName", firstName);
            command.Parameters.AddWithValue("@secondName", secondName);
            command.Parameters.AddWithValue("@lastName", lastName);
            command.Parameters.AddWithValue("@address", address);
            command.Parameters.AddWithValue("@phone", phone);
            command.Parameters.AddWithValue("@gender", gender);
            command.Parameters.AddWithValue("@nationalNo", nationalNumber);
            command.Parameters.AddWithValue("@countryID", countryID);
            command.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);

            // Handling the fields that accept null
            if (string.IsNullOrEmpty(thirdName))
                command.Parameters.AddWithValue("@thirdName", DBNull.Value);
            else
                command.Parameters.AddWithValue("@thirdName", thirdName);

            if (string.IsNullOrEmpty(email))
                command.Parameters.AddWithValue("@email", DBNull.Value);
            else
                command.Parameters.AddWithValue("@email", email);

            if (string.IsNullOrEmpty(imagePath))
                command.Parameters.AddWithValue("@imagePath", DBNull.Value);
            else
                command.Parameters.AddWithValue("@imagePath", imagePath);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    personID = insertedID;
                }
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsPeopleDataAccess Class: {ex.Message}");
                personID = -1;
            }
            finally { connection.Close(); }

            return personID;
        }

        public static bool DeletePerson(int PersonID)
        {


            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "Delete People where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            int rowsAffected = 0;

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsPeopleDataAccess Class: {ex.Message}");
                rowsAffected = 0;
            }
            finally
            {

                connection.Close();

            }

            return (rowsAffected > 0);

        }

        public static bool DeletePerson(string NationalNo)
        {


            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "Delete People where NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            int rowsAffected = 0;

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsPeopleDataAccess Class: {ex.Message}");
                rowsAffected = 0;
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        public static bool IsPersonExist(string nationalNo)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT Found = 1 WHERE EXISTS (SELECT Found = 1 FROM People WHERE nationalNo = @nationalNo)";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@nationalNo", nationalNo);

            bool isExist = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isExist = reader.Read();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsPeopleDataAccess Class: {ex.Message}");
                isExist = false;
            }
            finally { connection.Close(); }

            return isExist;
        }

        public static bool IsPersonExist(int PersonID)
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT Found = 1 WHERE EXISTS (SELECT Found = 1 FROM People WHERE PersonID = @PersonID)";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            bool isExist = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isExist = reader.Read();
            }
            catch (Exception ex)
            {
                clsErrorsLogger.LogError($"An error occurred in clsPeopleDataAccess Class: {ex.Message}");
                isExist = false;
            }
            finally { connection.Close(); }

            return isExist;
        }

        public static DataTable GetPeopleList()
        {
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            DataTable dt = new DataTable();

            string query = "SELECT * FROM People_View";

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
                clsErrorsLogger.LogError($"An error occurred in clsPeopleDataAccess Class: {ex.Message}");
                dt = null;
            }
            finally { connection.Close(); }

            return dt;
        }

    }
}
