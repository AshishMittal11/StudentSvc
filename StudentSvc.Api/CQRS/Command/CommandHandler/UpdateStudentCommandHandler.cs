using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentSvc.Api.Database;
using StudentSvc.Api.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StudentSvc.Api.CQRS.Command.CommandHandler
{
    public class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, bool>
    {
        private readonly ILogger<UpdateStudentCommandHandler> _logger;
        private readonly StudentContext _context;

        public UpdateStudentCommandHandler(ILogger<UpdateStudentCommandHandler> logger, StudentContext context)
        {
            this._logger = logger;
            this._context = context;
        }

        public async Task<bool> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbStudent = await this._context.StudentSet.FirstOrDefaultAsync(x => x.Id == request.Student.Id).ConfigureAwait(false);
                if (dbStudent != null)
                {
                    var entry = this._context.Entry<Student>(dbStudent);
                    entry.CurrentValues.SetValues(request.Student);
                    await this._context.SaveChangesAsync().ConfigureAwait(false);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message, request);
                throw;
            }
        }
    }
}
