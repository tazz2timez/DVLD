using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsInternationalLicense
    {
        public clsInternationalLicense()
        {
            this.InternationalLicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.CreatedByUserID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this.ExpiryDate = DateTime.Now;
            this.IssueDate = DateTime.Now;
            this.IsActive = false;
            this.Mode = enMode.add_new_mode;
        }
        private clsInternationalLicense(int internationalLicenseID, int applicationID, int issuedUsingLocalLicenseID, int driverID, int createdByUserID, DateTime issueDate, DateTime expiryDate, bool isActive)
        {
            this.InternationalLicenseID = internationalLicenseID;
            this.ApplicationID = applicationID;
            this.IssuedUsingLocalLicenseID = issuedUsingLocalLicenseID;
            this.DriverID = driverID;
            this.CreatedByUserID = createdByUserID;
            this.IssueDate = issueDate;
            this.ExpiryDate = expiryDate;
            this.IsActive = isActive;
            this.Mode = enMode.update_mode;
        }
        public int InternationalLicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }
        public int DriverID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public enum enMode { update_mode, add_new_mode };
        public enMode Mode { get; set; }

        public static clsInternationalLicense FindByInternationalLicenseID(int internationalLicenseID)
        {
            int applicationID = -1, driverID = -1, createdByUserID = -1, issuedUsingLocalLicenseID = -1;
            DateTime expiryDate = DateTime.Now, issueDate = DateTime.Now;
            bool isActive = false;

            if (clsInternationalLicensesDataAccess.GetLicenseInfoByLicenseID(internationalLicenseID, ref applicationID, ref driverID, ref issuedUsingLocalLicenseID, ref issueDate, ref expiryDate,
                                                                                ref isActive, ref createdByUserID))
                return new clsInternationalLicense(internationalLicenseID, applicationID, issuedUsingLocalLicenseID, driverID, createdByUserID, issueDate, expiryDate, isActive);
            else
                return null;
        }

        public static clsInternationalLicense FindByApplicationID(int applicationID)
        {
            int internationalLicenseID = -1, driverID = -1, createdByUserID = -1, issuedUsingLocalLicenseID = -1;
            DateTime expiryDate = DateTime.Now, issueDate = DateTime.Now;
            bool isActive = false;

            if (clsInternationalLicensesDataAccess.GetLicenseInfoByApplicationID(ref internationalLicenseID, applicationID, ref driverID, ref issuedUsingLocalLicenseID, ref issueDate, ref expiryDate,
                                                                                ref isActive, ref createdByUserID))
                return new clsInternationalLicense(internationalLicenseID, applicationID, issuedUsingLocalLicenseID, driverID, createdByUserID, issueDate, expiryDate, isActive);
            else
                return null;
        }

        public static clsInternationalLicense FindByLocalLicenseID(int issuedUsingLocalLicenseID)
        {
            int internationalLicenseID = -1, driverID = -1, createdByUserID = -1, applicationID = -1;
            DateTime expiryDate = DateTime.Now, issueDate = DateTime.Now;
            bool isActive = false;

            if (clsInternationalLicensesDataAccess.GetLicenseInfoByLocalLicenseID(ref internationalLicenseID, ref applicationID, ref driverID, issuedUsingLocalLicenseID, 
                                                                                    ref issueDate, ref expiryDate, ref isActive, ref createdByUserID))
                return new clsInternationalLicense(internationalLicenseID, applicationID, issuedUsingLocalLicenseID, driverID, createdByUserID, issueDate, expiryDate, isActive);
            else
                return null;
        }

        public static DataTable GetInternationalLicenses(int driverID)
        {
            return clsInternationalLicensesDataAccess.GetInternationalLicenses(driverID);
        }

        public static DataTable GetInternationalLicensesList()
        {
            return clsInternationalLicensesDataAccess.GetInternationalLicensesList();
        }

        public static int GetActiveInternationalLicenseIDByDriverID(int driverID)
        {
            return clsInternationalLicensesDataAccess.GetActiveInternationalLicenseIDByDriverID(driverID);
        }

        public bool Save()
        {
            switch (this.Mode)
            {
                case enMode.add_new_mode:
                    if (_New())
                    {
                        this.Mode = enMode.update_mode;
                        return true;
                    }
                    else
                        return false;
                case enMode.update_mode:
                    return _Update();
                default:
                    return false;
            }
        }

        private bool _New()
        {
            this.InternationalLicenseID = clsInternationalLicensesDataAccess.CreateNewLicense(ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpiryDate, IsActive, CreatedByUserID);

            return this.InternationalLicenseID != -1;
        }

        private bool _Update()
        {
            return clsInternationalLicensesDataAccess.UpdateLicense(InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpiryDate, IsActive, CreatedByUserID);
        }

    }
}
