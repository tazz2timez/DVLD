using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsLDL_Application
    {
        public clsLDL_Application()
        {
            this.LDL_AppID = -1;
            this.ApplicationID = -1;
            this.LicenseClassID = -1;

            this.Mode = enMode.add_new_mode;
        }
        private clsLDL_Application(int lDL_AppID, int applicationID, int licenseClassID)
        {
            this.LDL_AppID = lDL_AppID;
            this.ApplicationID = applicationID;
            this.LicenseClassID = licenseClassID;

            this.Mode = enMode.update_mode;
        }
        public int LDL_AppID { get; set; }
        public int ApplicationID {  get; set; }
        public int LicenseClassID { get; set; }
        public enum enMode { update_mode, add_new_mode };
        public enMode Mode { get; set; }

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

        public static bool Delete_LDL_Application(int LDL_AppID)
        {
            return clsLDL_ApplicationsDataAccess.DeleteApplication(LDL_AppID);
        }

        public static clsLDL_Application Find(int LDL_AppID)
        {
            int applicationID = -1, licenseClassID = -1;

            if(clsLDL_ApplicationsDataAccess.Get_LDL_ApplicationInfoB_LDL_AppID(LDL_AppID, ref applicationID, ref licenseClassID))
                return new clsLDL_Application(LDL_AppID, applicationID, licenseClassID);
            else
                return null;
        }

        public static int isPersonHasActiveApplicationWithSameClass(int applicantPersonID, int licenseClassID)
        {
            return clsLDL_ApplicationsDataAccess.isPersonHasActiveApplicationWithSameClass(applicantPersonID, licenseClassID);
        }

        public static DataTable Get_LDL_ApplicationsList()
        {
            return clsLDL_ApplicationsDataAccess.Get_LDL_ApplicationsList();
        }

        private bool _New()
        {
            this.LDL_AppID = clsLDL_ApplicationsDataAccess.New_LDL_Application(this.ApplicationID, this.LicenseClassID);

            return this.LDL_AppID != -1;
        }

        private bool _Update()
        {
            return clsLDL_ApplicationsDataAccess.Update_LDL_ApplicationInfo(this.LDL_AppID ,this.ApplicationID, this.LicenseClassID);
        }

    }
}
