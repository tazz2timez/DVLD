using DVLD_DataAccess;
using System.Data;

namespace DVLD_Business
{
    public class clsLicenseClass
    {
        private clsLicenseClass(int licenseClassID, string licenseClassName, string licenseClassDescription, byte minimumAllowedAge, byte defaultValidityLength, decimal classFees)
        {
            this.LicenseClassID = licenseClassID;
            this.LicenseClassName = licenseClassName;
            this.LicenseClassDescription = licenseClassDescription;
            this.MinimumAllowedAge = minimumAllowedAge;
            this.DefaultValidityLength = defaultValidityLength;
            this.LicenseClassFees = classFees;
        }
        public int LicenseClassID { get; set; }
        public string LicenseClassName { get; set; }
        public string LicenseClassDescription { get; set; }
        public decimal LicenseClassFees { get; set; }
        public byte MinimumAllowedAge { get; set; }
        public byte DefaultValidityLength { get; set; }

        public static DataTable GetLicenseClassesList()
        {
            return clsLicenseClassesDataAccess.GetLicenseClassesList();
        }

        public static clsLicenseClass Find(int licenseClassID)
        {
            string licenseClassName = string.Empty, classDescription = string.Empty;
            byte minimumAllowedAge = 0, defaultValidityLength = 0;
            decimal classFees = 0;

            if (clsLicenseClassesDataAccess.FindLicenseClassByID(licenseClassID, ref licenseClassName, ref classDescription,
                                                   ref minimumAllowedAge, ref defaultValidityLength, ref classFees))
                return new clsLicenseClass(licenseClassID, licenseClassName, classDescription, minimumAllowedAge, defaultValidityLength, classFees);
            else
                return null;
        }

        public static clsLicenseClass Find(string licenseClassName)
        {
            int licenseClassID = -1;
            string classDescription = string.Empty;
            byte minimumAllowedAge = 0, defaultValidityLength = 0;
            decimal classFees = 0;

            if (clsLicenseClassesDataAccess.FindLicenseClassByName(ref licenseClassID, licenseClassName, ref classDescription,
                                                   ref minimumAllowedAge, ref defaultValidityLength, ref classFees))
                return new clsLicenseClass(licenseClassID, licenseClassName, classDescription, minimumAllowedAge, defaultValidityLength, classFees);
            else
                return null;
        }

    }
}

