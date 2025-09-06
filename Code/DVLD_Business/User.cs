using DVLD_DataAccess;
using System.Data;

namespace DVLD_Business
{
    public class clsUser
    {
        public clsUser() 
        {
            this.UserID = -1;
            this.PersonID = -1;
            this.UserName = string.Empty;
            this.Password = string.Empty;
            this.isActive = false;

            this.Mode = enMode.add_new_mode;
        }
        private clsUser(int userID, int personID, string username, string password, bool isActive)
        { 
            this.UserID = userID;
            this.PersonID = personID;
            this.PersonInfo = clsPerson.Find(personID);
            this.UserName = username;
            this.Password = password;
            this.isActive = isActive;

            this.Mode = enMode.update_mode;
        }

        public int UserID { get; set; }
        public int PersonID { get; set; }

        public clsPerson PersonInfo;
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool isActive { get; set; }
        public enum enMode { update_mode, add_new_mode};
        public enMode Mode { get; set; }


        public static clsUser Find(int userID)
        {
            int personID = -1;
            string username = string.Empty, password = string.Empty;
            bool isActive = false;

            if(clsUsersDataAccess.GetUserInfoByUserID(userID, ref personID, ref username, ref password, ref isActive))
                return new clsUser(userID, personID, username, password, isActive);
            else
                return null;
        }


        public static clsUser Find(string username)
        {
            int personID = -1, userID = -1;
            string password = string.Empty;
            bool isActive = false;

            if (clsUsersDataAccess.GetUserInfoByUserName(ref userID, ref personID,  username, ref password, ref isActive))
                return new clsUser(userID, personID, username, password, isActive);
            else
                return null;
        }


        public static bool isExist(string username, string password)
        { 
            return clsUsersDataAccess.isUserExist(username, password);
        }


        public static bool isExist(string username)
        {
            return clsUsersDataAccess.isUserExist(username);
        }


        public static bool isExist(int userID)
        {
            return clsUsersDataAccess.isUserExist(userID);
        }

        public static bool isPersonAlreadyUser(int personID)
        {
            return clsUsersDataAccess.isPersonAlreadyUser(personID);
        }


        public static bool DeleteUser(int userID)
        {
            if(isExist(userID))
                return clsUsersDataAccess.DeleteUser(userID);
            else 
                return false;
        }


        public static bool DeleteUser(string username)
        {
            if (isExist(username))
                return clsUsersDataAccess.DeleteUser(username);
            else
                return false;
        }

        public bool Save()
        {
            switch(this.Mode)
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
            this.UserID = clsUsersDataAccess.AddNewUser(this.PersonID, this.UserName, this.Password, this.isActive);

            return this.UserID != -1;
        }

        private bool _Update()
        {
            if (isExist(this.UserID))
                return clsUsersDataAccess.UpdateUserInfo(this.UserID, this.PersonID, this.UserName, this.Password, this.isActive);
            else
                return false;
        }

        public static DataTable GetUsersList()
        {
            return clsUsersDataAccess.GetUsersList();
        }

    }
}