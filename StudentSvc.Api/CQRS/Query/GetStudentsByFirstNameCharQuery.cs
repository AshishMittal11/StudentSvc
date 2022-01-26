using MediatR;
using StudentSvc.Api.DTO;
using System.Collections.Generic;

namespace StudentSvc.Api.CQRS.Query
{
    public class GetStudentsByFirstNameCharQuery : IRequest<List<StudentDto>>
    {
        public string FirstNameChar { get; set; }
    }
}
