using DVLD_DataAccess;
using System.Data;

namespace DVLD_Business
{
    public class clsManageAppTypes
    {
        private clsManageAppTypes(int appID, string appTitle, decimal appFees)
        { 
            this.ApplicationID = appID;
            this.ApplicationTitle = appTitle;
            this.ApplicationFees = appFees;
        }
        public int ApplicationID { get; set; }
        public string ApplicationTitle { get; set; }
        public decimal ApplicationFees { get; set; }

        public static clsManageAppTypes Find(int appID)
        {
            string appTitle = string.Empty;
            decimal appFees = 0;

            if(clsManageAppsTypesDataAccess.GetAppTypeInfoByID(appID, ref appTitle, ref appFees))
                return new clsManageAppTypes(appID, appTitle, appFees);
            else
                return null;
        }

        public bool Update()
        {
            if(clsManageAppsTypesDataAccess.UpdateAppTypeInfo(this.ApplicationID, this.ApplicationTitle, this.ApplicationFees))
                return true;
            else
                return false;
        }

        public static DataTable GetAppTypesList()
        {
            return clsManageAppsTypesDataAccess.GetAppTypesList();
        }

    }
}
