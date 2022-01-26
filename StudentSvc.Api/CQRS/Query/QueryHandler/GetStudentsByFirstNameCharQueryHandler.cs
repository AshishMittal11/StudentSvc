using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentSvc.Api.Database;
using StudentSvc.Api.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentSvc.Api.CQRS.Query.QueryHandler
{
    public class GetStudentsByFirstNameCharQueryHandler
        : IRequestHandler<GetStudentsByFirstNameCharQuery, List<StudentDto>>
    {
        private readonly ILogger<GetStudentsByFirstNameCharQueryHandler> _logger;
        private readonly StudentContext _context;

        public GetStudentsByFirstNameCharQueryHandler(ILogger<GetStudentsByFirstNameCharQueryHandler> logger, StudentContext context)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<List<StudentDto>> Handle(GetStudentsByFirstNameCharQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dbStudents = await _context.StudentSet.Where(x => x.FirstName.StartsWith(request.FirstNameChar, StringComparison.OrdinalIgnoreCase)).ToListAsync();
                return dbStudents?.Adapt<List<StudentDto>>() ?? new List<StudentDto>();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, request, cancellationToken);
                throw;
            } 
        }
    }
}
