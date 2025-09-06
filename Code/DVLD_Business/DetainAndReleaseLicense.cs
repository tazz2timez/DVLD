using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsDetainAndReleaseLicense
    {
        private clsDetainAndReleaseLicense(int detainID, int licenseID, DateTime detainDate, decimal fineFees,int detainedByUserID,
                                              bool isReleased, DateTime releaseDate, int releasedByUserID, int releaseApplicationID)
        { 
            this.DetainID = detainID;
            this.LicenseID = licenseID;
            this.ReleaseDate = releaseDate;
            this.DetainDate = detainDate;
            this.FineFees = fineFees;
            this.isReleased = isReleased;
            this.DetainedByUserID = detainedByUserID;
            this.ReleaseApplicationID = releaseApplicationID;
            this.ReleasedByUserID = releasedByUserID;
        }
        public int DetainID { get; set; }
        public int LicenseID { get; set; }  
        public DateTime DetainDate { get; set; }
        public decimal FineFees { get; set; }
        public int DetainedByUserID { get; set; }
        public bool isReleased {  get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ReleasedByUserID { get; set; }   
        public int ReleaseApplicationID { get; set; }

        public static int DetainLicense(int licenseID, DateTime detainDate, decimal fineFees, int detainedByUserID)
        {
            return clsDetainedLicensesDataAccess.DetainLicense(licenseID, detainDate, fineFees, detainedByUserID);
        }

        public static bool ReleaseLicense(int licenseID, DateTime releaseDate, int releasedByUserID, int releaseApplicationID)
        {
            return clsDetainedLicensesDataAccess.ReleaseLicense(licenseID, releaseDate, releasedByUserID, releaseApplicationID);
        }

        public static bool isLicenseDetained(int licenseID)
        {
            return clsDetainedLicensesDataAccess.isLicenseDetained(licenseID);
        }

        public static clsDetainAndReleaseLicense FindByDetainID(int detainID)
        {
            int licenseID = -1, detainedByUserID = -1, releasedByUserID = -1, releaseApplicationID = -1;
            DateTime detainDate = DateTime.Now, releaseDate = DateTime.Now;
            bool isreleased = false;
            decimal fineFees = 0;

            if(clsDetainedLicensesDataAccess.GetDetainedLicenseInfoByDetainID(detainID, ref licenseID, ref detainDate, ref fineFees, 
                                        ref detainedByUserID, ref isreleased, ref releaseDate , ref releasedByUserID, ref releaseApplicationID))
            {
                return new clsDetainAndReleaseLicense(detainID, licenseID, detainDate, fineFees, detainedByUserID, isreleased, releaseDate, releasedByUserID, releaseApplicationID);
            }
            else
            {
                return null;
            }

        }

        public static clsDetainAndReleaseLicense FindByLicenseID(int licenseID)
        {
            int detainID = -1, detainedByUserID = -1, releasedByUserID = -1, releaseApplicationID = -1;
            DateTime detainDate = DateTime.Now, releaseDate = DateTime.Now;
            bool isreleased = false;
            decimal fineFees = 0;

            if (clsDetainedLicensesDataAccess.GetDetainedLicenseInfoByLicenseID(ref detainID, licenseID, ref detainDate, ref fineFees,
                                        ref detainedByUserID, ref isreleased, ref releaseDate, ref releasedByUserID, ref releaseApplicationID))
            {
                return new clsDetainAndReleaseLicense(detainID, licenseID, detainDate, fineFees, detainedByUserID, isreleased, releaseDate, releasedByUserID, releaseApplicationID);
            }
            else
            {
                return null;
            }

        }

        public static DataTable GetDetainedLicensesList()
        {
            return clsDetainedLicensesDataAccess.GetDetainedLicensesList();
        }

    }
}
