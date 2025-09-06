using DVLD_DataAccess;
using System;

namespace DVLD_Business
{
    public class clsApplication
    {
        private clsApplication(int appID, int applicantPersonID, DateTime appDate, int appTypeID, int appStatusID,
                                                    DateTime lastStatusDate, decimal paidFees, int createdByUserID)
        {
            this.ApplicationID = appID;
            this.ApplicantPersonID = applicantPersonID;
            this.ApplicationDate = appDate;
            this.ApplicationTypeID = appTypeID;
            this.ApplicationStatusID = appStatusID;
            this.LastStatusDate = lastStatusDate;
            this.PaidFees = paidFees;   
            this.CreatedByUserID = createdByUserID;

            this.Mode = enMode.update_mode;
        }
        public clsApplication()
        {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationTypeID = -1;
            this.ApplicationStatusID = -1;
            this.CreatedByUserID = -1;
            this.ApplicationDate = DateTime.Now;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = 0;

            this.Mode = enMode.add_new_mode;
        }
        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }
        public int ApplicationStatusID { get; set; }    
        public int ApplicationTypeID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime LastStatusDate { get; set; }
        public decimal PaidFees { get; set; }
        public enum enMode { update_mode, add_new_mode };
        public enMode Mode { get; set; }
        public enum enApplicationType
        {
            NewDrivingLicense = 1,
            RenewDrivingLicense = 2,
            ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4,
            ReleaseDetainedDrivingLicense = 5,
            NewInternationalLicense = 6,
            RetakeTest = 8
        };

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

        public static bool CancelApplication(int  applicationID)
        {
            return clsApplicationsDataAccess.CancelApplication(applicationID);
        }

        public static clsApplication Find(int applicationID)
        {
            int applicantPersonID = -1, appTypeID = -1, appStatusID = -1, createdByUserID = -1;
            DateTime appDate = DateTime.Now, lastStatusDate = DateTime.Now;
            decimal paidFees = 0;

            if(clsApplicationsDataAccess.GetApplicationInfoByID(applicationID, ref applicantPersonID, ref appDate, ref appTypeID, ref appStatusID,
                                                                    ref lastStatusDate, ref paidFees, ref createdByUserID))
                return new clsApplication(applicationID, applicantPersonID, appDate, appTypeID, appStatusID, lastStatusDate, paidFees, createdByUserID);
            else
                return null;
        }

        public static bool DeleteApplication(int applicationID)
        {
            return clsApplicationsDataAccess.DeleteApplication(applicationID);
        }

        public static string GetApplicationStatus(int applicationID)
        {
            return clsApplicationsDataAccess.GetApplicationStatus(applicationID);
        }

        private bool _New()
        {
            this.ApplicationID = clsApplicationsDataAccess.NewApplication(ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatusID
                                                                            , LastStatusDate, PaidFees, CreatedByUserID);

            return this.ApplicationID != -1;
        }

        private bool _Update()
        {
            return clsApplicationsDataAccess.UpdateApplicationInfo(ApplicationID ,ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatusID
                                                                            , LastStatusDate, PaidFees, CreatedByUserID);
        }

    }
}
