using MediatR;
using StudentSvc.Api.DTO;

namespace StudentSvc.Api.CQRS.Query
{
    public class GetStudentFromCosmosByEmailQuery : IRequest<StudentDto>
    {
        public string Email { get; set; }
    }
}
