using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsTest
    {
        public clsTest()
        {
            this.TestID = -1;
            this.CreatedByUserID = -1;
            this.AppointmentID = -1;
            this.Notes = string.Empty;
            this.TestResult = false;

            this.Mode = enMode.add_new_mode;
        }
        private clsTest(int testID, int appointmentID, bool testResult, string notes, int createdByUserID)
        {
            this.TestID = testID;
            this.AppointmentID = appointmentID;
            this.CreatedByUserID = createdByUserID;
            this.TestResult = testResult;
            this.Notes = notes;

            this.Mode = enMode.update_mode;
        }
        public int TestID { get; set; } 
        public int AppointmentID { get; set; }  
        public int CreatedByUserID { get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }
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

        public static clsTest Find(int testID)
        {
            int appointmentID = -1, createdByUserID = -1;
            bool testResult = false;
            string notes = string.Empty;

            if(clsTestsDataAccess.GetTestInfoByID(testID, ref appointmentID, ref testResult, ref notes, ref createdByUserID))
                return new clsTest(testID, appointmentID, testResult, notes, createdByUserID);
            else
                return null;
        }

        public static int GetTotalPassedTests(int LDL_AppID)
        {
            return clsTestsDataAccess.GetTotalPassedTests(LDL_AppID);
        }

        public static int CountFailedTests(int testTypeID, int LDL_AppID)
        {
            return clsTestsDataAccess.CountFailedTests(testTypeID, LDL_AppID);
        }

        private bool _New()
        {
            this.TestID = clsTestsDataAccess.AddNewTest(this.AppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);

            return this.TestID != -1;
        }

        private bool _Update()
        {
            return clsTestsDataAccess.UpdateTestInfo(this.TestID, this.AppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
        }

    }
}
