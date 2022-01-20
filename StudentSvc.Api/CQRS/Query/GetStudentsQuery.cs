using MediatR;
using StudentSvc.Api.DTO;
using System.Collections.Generic;

namespace StudentSvc.Api.CQRS.Query
{
    public class GetStudentsQuery : IRequest<List<StudentDto>>
    {
    }
}
