using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using StudentSvc.Api.Database;
using StudentSvc.Api.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StudentSvc.Api.CQRS.Command.CommandHandler
{
    public class StudentRegisterCommandHandler : IRequestHandler<StudentRegisterCommand, bool>
    {
        private readonly ILogger<StudentRegisterCommandHandler> _logger;
        private readonly StudentContext _context;

        public StudentRegisterCommandHandler(ILogger<StudentRegisterCommandHandler> logger, StudentContext context)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> Handle(StudentRegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbStudent = request.Student.Adapt<Student>();
                await _context.StudentSet.AddAsync(dbStudent).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch(Exception ex)
            {
                this._logger.LogError(ex.Message, request);
                throw;
            } 
        }
    }
}
