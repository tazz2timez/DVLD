using DVLD_DataAccess;
using System.Data;

namespace DVLD_Business
{
    public class clsTestType
    {
        private clsTestType(int TestID, string TestTitle, string TestDescription, decimal TestFees)
        {
            this.TestID = TestID;
            this.TestTitle = TestTitle;
            this.TestFees = TestFees;
            this.TestDescription = TestDescription;
        }
        public int TestID { get; set; }
        public string TestTitle { get; set; }
        public string TestDescription { get; set; }
        public decimal TestFees { get; set; }

        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 }

        public static clsTestType Find(int TestID)
        {
            string TestTitle = string.Empty , TestDescription = string.Empty;
            decimal TestFees = 0;

            if (clsManageTestTypesDataAccess.GetTestTypeInfoByID(TestID, ref TestTitle, ref TestDescription, ref TestFees))
                return new clsTestType(TestID, TestTitle, TestDescription, TestFees);
            else
                return null;
        }

        public bool Update()
        {
            if (clsManageTestTypesDataAccess.UpdateTestTypeInfo(this.TestID, this.TestTitle, this.TestDescription, this.TestFees))
                return true;
            else
                return false;
        }

        public static DataTable GetTestTypesList()
        {
            return clsManageTestTypesDataAccess.GetTestTypesList();
        }

    }
}

