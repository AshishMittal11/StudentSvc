using System;

namespace StudentSvc.Api.Cosmos
{
    public class StudentCosmos : CosmosBase
    {
        public int StudentId { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

        public byte[] Photo { get; set; }

        public string CurrentAddress { get; set; }

        public string PermanentAddress { get; set; }

        public DateTimeOffset Dob { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset ModifiedOn { get; set; }
    }
}
