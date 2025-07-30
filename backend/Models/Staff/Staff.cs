using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassroomManagement.Models.Staff
{
    public class Staff
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string BloodGroup { get; set; }
        public string AadharNumber { get; set; }

      
        public string Mobile { get; set; }
        public string AltMobile { get; set; }
        public string Email { get; set; }

       
        public string DoorNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Taluk { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string Country { get; set; }

       
        [NotMapped] 
        public List<StaffQualification> Qualifications { get; set; }

        
        public string Department { get; set; }
        public string Role { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string StaffType { get; set; }

       
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string PANCardNumber { get; set; }

       
        public string PhotoPath { get; set; }

        [NotMapped] 
        public IFormFile Photo { get; set; }

       
        public string Username { get; set; }
        public string Password { get; set; }
        public string LoginRole { get; set; }
    }

    [Table("StaffQualifications")] 
    public class StaffQualification
    {
        public int Id { get; set; } 
        public string StaffId { get; set; } 
        public string Degree { get; set; }
        public string Specialization { get; set; }
        public string Institution { get; set; }
        public string University { get; set; }
        public string YearOfPassing { get; set; }
    }
}
