using MediatR;
using StudentSvc.Api.DTO;

namespace StudentSvc.Api.CQRS.Query
{
    public class GetStudentByIdQuery : IRequest<StudentDto>
    {
        public int Id { get; set; }
    }
}
