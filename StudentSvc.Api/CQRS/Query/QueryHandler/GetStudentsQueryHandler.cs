using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentSvc.Api.Database;
using StudentSvc.Api.DTO;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StudentSvc.Api.CQRS.Query.QueryHandler
{
    public class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, List<StudentDto>>
    {
        private readonly ILogger<GetStudentsQueryHandler> _logger;
        private readonly StudentContext _context;

        public GetStudentsQueryHandler(ILogger<GetStudentsQueryHandler> logger, StudentContext context)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<List<StudentDto>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dbStudents = await _context.StudentSet.ToListAsync().ConfigureAwait(false);
                return dbStudents?.Adapt<List<StudentDto>>() ?? new List<StudentDto>(); 
            }
            catch(Exception ex)
            {
                this._logger.LogError(ex.Message, request);
                throw;
            } 
        }
    }
}
