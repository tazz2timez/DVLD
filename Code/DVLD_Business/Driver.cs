using DVLD_DataAccess;
using System;
using System.Data;

namespace DVLD_Business
{
    public class clsDriver
    {
        public clsDriver()
        {
            this.DriverID = -1;
            this.PersonID = -1;
            this.CreatedByUserID = -1;
            this.CreationDate = DateTime.Now;

            this.Mode = enMode.add_new_mode;
        }
        private clsDriver(int driverID, int personID, int createdByUserID, DateTime creationDate)
        {
            this.DriverID = driverID;
            this.PersonID = personID;
            this.CreatedByUserID = createdByUserID;
            this.CreationDate = creationDate;
            this.Mode = enMode.update_mode;
        }
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreationDate {  get; set; }
        public enum enMode { update_mode, add_new_mode };
        public enMode Mode { get; set; }

        public static clsDriver FindByDriverID(int driverID)
        {
            int personID = -1, createdByUserID = -1;
            DateTime creationDate = DateTime.MinValue;

            if(clsDriversDataAccess.GetDriverInfoByDriverID(driverID, ref personID, ref createdByUserID, ref creationDate))
                return new clsDriver(driverID, personID, createdByUserID, creationDate);
            else
                return null;
        }

        public static clsDriver FindByPersonID(int personID)
        {
            int driverID = -1, createdByUserID = -1;
            DateTime creationDate = DateTime.MinValue;

            if (clsDriversDataAccess.GetDriverInfoByPersonID(ref driverID, personID, ref createdByUserID, ref creationDate))
                return new clsDriver(driverID, personID, createdByUserID, creationDate);
            else
                return null;

        }

        public static bool isPersonAlreadyDriver(int  personID)
        {
            return clsDriversDataAccess.isPersonAlreadyDriver(personID);
        }

        public static bool DeleteDriver(int driverID)
        {
            return clsDriversDataAccess.DeleteDriver(driverID);
        }

        public static DataTable GetDriversList()
        {
            return clsDriversDataAccess.GetDriversList();
        }

        public bool Save()
        {
            switch (this.Mode)
            {
                case enMode.add_new_mode:
                    if (_AddNew())
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

        private bool _AddNew()
        {
            this.DriverID = clsDriversDataAccess.AddNewDriver(this.PersonID, this.CreatedByUserID, this.CreationDate);

            return this.DriverID != -1;
        }

        private bool _Update()
        {
            return clsDriversDataAccess.UpdateDriverInfo(this.DriverID, this.PersonID, this.CreatedByUserID, this.CreationDate);
        }

    }
}
