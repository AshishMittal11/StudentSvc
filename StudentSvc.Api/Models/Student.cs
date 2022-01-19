using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentSvc.Api.Models
{
    [Table("Student", Schema = "dbo")]
    public class Student : StudentBase
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }  

        [StringLength(50)]
        public string LastName { get; set; }    

        [StringLength(1)]
        public string Gender { get; set; }

        [Required, StringLength(100)]
        public string Email { get; set; }

        public byte[] Photo { get; set; }

        [StringLength(100)]
        public string CurrentAddress { get; set; }

        [StringLength(100)]
        public string PermanentAddress { get; set; }

        [Required]
        public DateTimeOffset Dob { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
