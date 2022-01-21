using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentSvc.Api.Database;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StudentSvc.Api.CQRS.Command.CommandHandler
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, bool>
    {
        private readonly StudentContext _context;
        private readonly ILogger<DeleteStudentCommandHandler> _logger;

        public DeleteStudentCommandHandler(ILogger<DeleteStudentCommandHandler> logger, StudentContext context)
        {
            this._context = context;
            this._logger = logger;
        }

        public async Task<bool> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbStudent = await _context.StudentSet.FirstOrDefaultAsync(x => x.Id == request.StudentId).ConfigureAwait(false);
                if (dbStudent != null)
                {
                    _context.Remove(dbStudent);
                    await _context.SaveChangesAsync().ConfigureAwait(false);     
                    return true;
                }

                return false;   
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, request);
                throw;
            } 
        }
    }
}
