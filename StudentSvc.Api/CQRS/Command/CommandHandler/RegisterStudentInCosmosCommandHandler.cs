using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using StudentSvc.Api.Models;
using StudentSvc.Api.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StudentSvc.Api.CQRS.Command.CommandHandler
{
    public class RegisterStudentInCosmosCommandHandler
        : IRequestHandler<RegisterStudentInCosmosCommand, bool>
    {
        private readonly ILogger<RegisterStudentInCosmosCommand> _logger;
        private readonly IStudentRepository _studentRepository;

        public RegisterStudentInCosmosCommandHandler(ILogger<RegisterStudentInCosmosCommand> logger, IStudentRepository studentRepository)
        {
            this._logger = logger;
            this._studentRepository = studentRepository;
        }

        public async Task<bool> Handle(RegisterStudentInCosmosCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbStudent = request.Student.Adapt<Student>();
                bool status = await this._studentRepository.SaveStudentToCosmosAsync(dbStudent).ConfigureAwait(false);
                return status;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, request);
                throw;
            }
        }
    }
}
