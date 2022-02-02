using StudentSvc.Api.Models;
using System.Threading.Tasks;

namespace StudentSvc.Api.Repository
{
    public interface IStudentRepository
    {
        Task<bool> SaveStudentToCosmosAsync(Student student);
        Task<bool> UpdateStudentToCosmosAsync(Student student);
        bool IsStudentPresentInCosmos(Student student);
    }
}