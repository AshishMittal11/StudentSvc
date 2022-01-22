using System;

namespace StudentSvc.Api.DTO
{
    public class StudentDto
    {
        public int Id { get; set; } 
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhotoBase64 { get; set; }
        public string CurrentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string Dob { get; set; }
        public bool IsActive { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
    }
}
