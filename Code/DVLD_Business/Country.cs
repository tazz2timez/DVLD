using DVLD_DataAccess;
using System.Data;

namespace DVLD_Business
{
    public class clsCountry
    {
        private clsCountry(int countryID, string countryName)
        {
            this.CountryID = countryID;
            this.CountryName = countryName;
        }
        public int CountryID { get; set; }
        public string CountryName { get; set; }

        public static DataTable GetCountriesList()
        {
            return clsCountriesDataAccess.GetCountriesList();
        }

        public static clsCountry Find(int countryID)
        {
            string countryName = "";

            if(clsCountriesDataAccess.FindCountryByID(countryID, ref countryName))
                return new clsCountry(countryID, countryName);
            else
                return null;
        }

        public static clsCountry Find(string countryName)
        {
            int countryID = -1;

            if (clsCountriesDataAccess.FindCountryByName(ref countryID, countryName))
                return new clsCountry(countryID, countryName);
            else
                return null;
        }

    }
}
