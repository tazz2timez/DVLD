using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsTestAppointment
    {
        public clsTestAppointment()
        {
            this.TestAppointmentID = -1;
            this.TestTypeID = -1;
            this.LDL_ApplicationID = -1;
            this.CreatedByUserID = -1;
            this.isLocked = false;
            this.PaidFees = 0;
            this.AppointmentDate = DateTime.Now;

            this.Mode = enMode.add_new_mode;
        }
        private clsTestAppointment(int testAppointmentID, int testTypeID, int lDL_ApplicationID, DateTime appointmentDate, decimal paidFees, int createdByUserID, bool isLocked)
        {
            TestAppointmentID = testAppointmentID;
            CreatedByUserID = createdByUserID;
            TestTypeID = testTypeID;
            LDL_ApplicationID = lDL_ApplicationID;
            PaidFees = paidFees;
            AppointmentDate = appointmentDate;
            this.isLocked = isLocked;

            this.Mode = enMode.update_mode;
        }
        public int TestAppointmentID { get; set; }
        public int CreatedByUserID { get; set; }    
        public int TestTypeID { get; set; } 
        public int LDL_ApplicationID { get; set; }
        public decimal PaidFees { get; set; }   
        public DateTime AppointmentDate { get; set; }
        public bool isLocked { get; set; }
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

        public static clsTestAppointment Find(int testAppointmentID)
        {
            int testTypeID = -1, createdByUserID = -1, LDL_AppID = -1;
            bool isLocked = false;
            decimal paidFees = 0;
            DateTime appointmentDate = DateTime.Now;

            if (clsTestAppointmentsDataAccess.GetTestAppointmentInfoByID(testAppointmentID, ref testTypeID, ref LDL_AppID, ref appointmentDate, ref paidFees
                , ref createdByUserID, ref isLocked))
                return new clsTestAppointment(testAppointmentID, testTypeID, LDL_AppID, appointmentDate, paidFees
                , createdByUserID, isLocked);
            else
                return null;
        }

        public static int GetTotalPassedTests(int LDL_AppID)
        {
            return clsTestsDataAccess.GetTotalPassedTests(LDL_AppID);
        }

        public static DataTable GetTestAppointmentsListPerTest(int LDL_AppID, int testTypeID)
        {
            return clsTestAppointmentsDataAccess.GetTestAppointmentsListPerTest(LDL_AppID, testTypeID);
        }

        public static bool HasActiveAppointment(int LDL_AppID) 
        {
            return clsTestAppointmentsDataAccess.HasActiveAppointment(LDL_AppID);
        }

        public static bool HasPassedTest(int LDL_AppID, int testTypeID)
        {
            return clsTestAppointmentsDataAccess.HasPassedTest(LDL_AppID, testTypeID);
        }

        public static bool HasFailedTest(int LDL_AppID, int testTypeID)
        {
            return clsTestAppointmentsDataAccess.HasFailedTest(LDL_AppID, testTypeID);
        }

        private bool _New()
        {
            this.TestAppointmentID = clsTestAppointmentsDataAccess.AddNewTestAppointment(this.TestTypeID, this.LDL_ApplicationID, this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.isLocked);
            
            return this.TestAppointmentID != -1;
        }

        private bool _Update()
        {
            return clsTestAppointmentsDataAccess.UpdateTestAppointmentInfo(TestAppointmentID, TestTypeID, LDL_ApplicationID, AppointmentDate, PaidFees, CreatedByUserID, isLocked);
        }

    }
}
