using Microsoft.EntityFrameworkCore;
using StudentSvc.Api.Models;

namespace StudentSvc.Api.Database
{
    public class StudentContext : DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options)
        {

        }

        public DbSet<Student> StudentSet { get; set; }
    }
}
