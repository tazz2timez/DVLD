using DVLD_DataAccess;
using System;
using System.Data;

namespace DVLD_Business
{
    public class clsPerson
    {
        public clsPerson()
        {
            this.PersonID = -1;
            this.CountryID = -1;
            this.FirstName = string.Empty;
            this.SecondName = string.Empty;
            this.ThirdName = string.Empty;
            this.LastName = string.Empty;
            this.Address = string.Empty;
            this.Email = string.Empty;
            this.Phone = string.Empty;
            this.ImagePath = string.Empty;
            this.NationalNumber = string.Empty;
            this.Gender = '\0';
            this.DateOfBirth = DateTime.Now;

            this.Mode = enMode.add_new_mode;
        }

        private clsPerson(int ID, string nationalNumber, string firstName, string secondName, string thirdName,
                            string lastName, DateTime dateOfBirth, char gender, string address,
                            string phone, string email, int countryID, string imagePath)
        {
            this.PersonID = ID;
            this.NationalNumber = nationalNumber;
            this.FirstName = firstName;
            this.SecondName = secondName;
            this.ThirdName = thirdName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Gender = gender;
            this.Address = address;
            this.Phone = phone;
            this.Email = email;
            this.CountryID = countryID;
            this.ImagePath = imagePath;

            this.Mode = enMode.update_mode;
        }

        public int PersonID { get; set; }
        public string NationalNumber { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + SecondName + " " + ThirdName + " " + LastName;
            }
        }
        public DateTime DateOfBirth { get; set; }
        public char Gender { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int CountryID { get; set; }
        public string ImagePath { get; set; }
        public enMode Mode { get; set; }
        public enum enMode { update_mode, add_new_mode }

        public static clsPerson Find(int  personID)
        {
            string firstName = "", secondName = "", thirdName = "", lastName = "", nationalNumber = "", email = "", phone = "",
                       imagePath = "", address = "";
            int countryID = -1;
            char gender = '\0';
            DateTime dateOfBirth = DateTime.Now;

            if(clsPeopleDataAccess.GetPersonInfoByID(personID, ref nationalNumber, ref firstName, ref secondName, ref thirdName,
                 ref lastName, ref dateOfBirth, ref gender, ref address, ref phone, ref email, ref countryID, ref imagePath))
                return new clsPerson(personID, nationalNumber, firstName, secondName, thirdName,lastName , dateOfBirth, gender, address, phone, email, countryID, imagePath);
            else
                return null;
        }

        public static clsPerson Find(string nationalNumber)
        {
            string firstName = "", secondName = "", thirdName = "", lastName = "", email = "", phone = "",
                       imagePath = "", address = "";
            int countryID = -1, ID = -1;
            char gender = '\0';
            DateTime dateOfBirth = DateTime.Now;

            if (clsPeopleDataAccess.GetPersonInfoByNationalNo(ref ID, nationalNumber, ref firstName, ref secondName, ref thirdName,
                 ref lastName, ref dateOfBirth, ref gender, ref address, ref phone, ref email, ref countryID, ref imagePath))
                return new clsPerson(ID, nationalNumber, firstName, secondName, thirdName, lastName, dateOfBirth, gender, address, phone, email, countryID, imagePath);
            else
                return null;
        }

        public static bool DeletePerson(string nationalNumber)
        {
            return clsPeopleDataAccess.DeletePerson(nationalNumber);
        }

        public static bool DeletePerson(int personID)
        {
            return clsPeopleDataAccess.DeletePerson(personID);
        }

        private bool _AddNewPerson()
        {
            this.PersonID = clsPeopleDataAccess.AddNewPerson(this.NationalNumber, this.FirstName, this.SecondName, this.ThirdName,
                                                        this.LastName, this.DateOfBirth, this.Gender, this.Address, this.Phone,
                                                        this.Email, this.CountryID, this.ImagePath);

            return this.PersonID != -1;
        }

        private bool _UpdatePerson()
        {
            return clsPeopleDataAccess.UpdatePersonInfo(this.PersonID ,this.NationalNumber, this.FirstName, this.SecondName, this.ThirdName,
                                                        this.LastName, this.DateOfBirth, this.Gender, this.Address, this.Phone,
                                                        this.Email, this.CountryID, this.ImagePath);
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.add_new_mode:
                    if(_AddNewPerson())
                    {
                        this.Mode = enMode.update_mode;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.update_mode:
                    return _UpdatePerson();
                default:
                    return false;
            }
        }

        public static bool isExist(int personID)
        {
            return clsPeopleDataAccess.IsPersonExist(personID);
        }

        public static bool isExist(string nationalNo)
        {
            return clsPeopleDataAccess.IsPersonExist(nationalNo);
        }

        public static DataTable GetPeopleList()
        {
            return clsPeopleDataAccess.GetPeopleList();
        }


    }
}
