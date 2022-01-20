using MediatR;
using StudentSvc.Api.DTO;

namespace StudentSvc.Api.CQRS.Command
{
    public class StudentRegisterCommand : IRequest<bool>
    {
        public StudentDto Student { get; set; }
    }
}
