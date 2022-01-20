using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentSvc.Api.Database;
using StudentSvc.Api.DTO;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StudentSvc.Api.CQRS.Query.QueryHandler
{
    public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, StudentDto>
    {
        private readonly ILogger<GetStudentByIdQueryHandler> _logger;
        private readonly StudentContext _context;

        public GetStudentByIdQueryHandler(ILogger<GetStudentByIdQueryHandler> logger, StudentContext context)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<StudentDto> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dbStudent = await this._context.StudentSet.FirstOrDefaultAsync(x => x.Id == request.Id).ConfigureAwait(false);
                return dbStudent?.Adapt<StudentDto>() ?? new StudentDto();
            }
            catch(Exception ex)
            {
                this._logger.LogError(ex.Message, request);
                throw;
            } 
        }
    }
}
