using DVLD_DataAccess;
using System;
using System.Data;

namespace DVLD_Business
{
    public class clsLicense
    {
        public clsLicense()
        {
            this.LicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.CreatedByUserID = -1;
            this.IssueReason = 0;
            this.LicenseClassID = -1;
            this.ExpiryDate = DateTime.Now;
            this.IssueDate = DateTime.Now;
            this.Notes = string.Empty;
            this.PaidFees = 0;
            this.IsActive = false;
            this.Mode = enMode.add_new_mode;
        }
        private clsLicense(int licenseID, int applicationID, int licenseClassID, int driverID, int createdByUserID, byte issueReason, DateTime issueDate, DateTime expiryDate, string notes, decimal paidFees, bool isActive)
        {
            this.LicenseID = licenseID;
            this.ApplicationID = applicationID;
            this.LicenseClassID = licenseClassID;
            this.DriverID = driverID;
            this.CreatedByUserID = createdByUserID;
            this.IssueReason = issueReason;
            this.IssueDate = issueDate;
            this.ExpiryDate = expiryDate;
            this.Notes = notes;
            this.PaidFees = paidFees;
            this.IsActive = isActive;
            this.Mode = enMode.update_mode;
        }
        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int LicenseClassID { get; set; }
        public int DriverID { get; set; }
        public int CreatedByUserID { get; set; }
        public byte IssueReason { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Notes { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsActive { get; set; }
        public enum enMode { update_mode, add_new_mode };
        public enMode Mode { get; set; }

        public static clsLicense FindByLicenseID(int licenseID)
        {
            int applicationID = -1, driverID = -1, createdByUserID = -1, licenseClassID = -1;
            byte issueReason = 0;
            DateTime expiryDate = DateTime.Now, issueDate = DateTime.Now;
            string notes = string.Empty;
            decimal paidFees = 0;
            bool isActive = false;

            if (clsLicensesDataAccess.GetLicenseInfoByLicenseID(licenseID, ref applicationID, ref driverID, ref licenseClassID, ref issueDate, ref expiryDate,
                                                            ref notes, ref paidFees, ref isActive, ref issueReason, ref createdByUserID))
                return new clsLicense(licenseID, applicationID, licenseClassID, driverID, createdByUserID, issueReason, issueDate, expiryDate, notes, paidFees, isActive);
            else
                return null;
        }

        public static clsLicense FindByApplicationID(int applicationID)
        {
            int licenseID = -1, driverID = -1, createdByUserID = -1, licenseClassID = -1;
            byte issueReason = 0;
            DateTime expiryDate = DateTime.Now, issueDate = DateTime.Now;
            string notes = string.Empty;
            decimal paidFees = 0;
            bool isActive = false;

            if (clsLicensesDataAccess.GetLicenseInfoByApplicationID(ref licenseID, applicationID, ref driverID, ref licenseClassID, ref issueDate, ref expiryDate,
                                                            ref notes, ref paidFees, ref isActive, ref issueReason, ref createdByUserID))
                return new clsLicense(licenseID, applicationID, licenseClassID, driverID, createdByUserID, issueReason, issueDate, expiryDate, notes, paidFees, isActive);
            else
                return null;
        }

        public static bool isApplicationConnectedToLicense(int applicationID)
        {
            return clsLicensesDataAccess.isApplicationConnectedToLicense(applicationID);
        }

        public static bool isPersonHaveLicenseWithSameClass(int licenseClassID, int driverID)
        {
            return clsLicensesDataAccess.isPersonHaveLicenseWithSameClass(licenseClassID, driverID);
        }

        public static DataTable GetLocalLicenses(int driverID)
        {
            return clsLicensesDataAccess.GetLocalLicenses(driverID);
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
            this.LicenseID = clsLicensesDataAccess.CreateNewLicense(ApplicationID, DriverID, LicenseClassID, IssueDate, ExpiryDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID);

            return this.LicenseID != -1;
        }

        private bool _Update()
        {
            return clsLicensesDataAccess.UpdateLicense(LicenseID ,ApplicationID, DriverID, LicenseClassID, IssueDate, ExpiryDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID);
        }

    }
}
