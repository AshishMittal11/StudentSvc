using MediatR;

namespace StudentSvc.Api.CQRS.Command
{
    public class DeleteStudentCommand : IRequest<bool>
    {
        public int StudentId { get; set; }
    }
}
