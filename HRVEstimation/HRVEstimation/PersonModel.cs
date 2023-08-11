using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace HRVEstimation
{
    public class PersonModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string MedicalCardNumber { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Department { get; set; }
        public DateTime DateOfAppointment { get; set; }

        public PersonModel()
        {
        }

        public PersonModel(int id, string medicalCardNumber, string fullName, DateTime dateOfBirth, string department, DateTime dateOfAppointment)
        {
            Id = id;
            MedicalCardNumber = medicalCardNumber;
            FullName = fullName;
            DateOfBirth = dateOfBirth;
            Department = department;
            DateOfAppointment = dateOfAppointment;  
        }
    }
}
