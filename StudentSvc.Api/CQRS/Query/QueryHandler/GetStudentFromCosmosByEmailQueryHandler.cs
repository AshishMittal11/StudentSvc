using MediatR;
using Microsoft.Extensions.Logging;
using StudentSvc.Api.DTO;
using StudentSvc.Api.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StudentSvc.Api.CQRS.Query.QueryHandler
{
    public class GetStudentFromCosmosByEmailQueryHandler
        : IRequestHandler<GetStudentFromCosmosByEmailQuery, StudentDto>
    {
        private readonly ILogger<GetStudentFromCosmosByEmailQueryHandler> _logger;
        private readonly IStudentRepository _studentRepository;

        public GetStudentFromCosmosByEmailQueryHandler(ILogger<GetStudentFromCosmosByEmailQueryHandler> logger, IStudentRepository studentRepository)
        {
            this._logger = logger;
            this._studentRepository = studentRepository;
        }
        
        public async Task<StudentDto> Handle(GetStudentFromCosmosByEmailQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Email)) throw new ArgumentNullException(nameof(request.Email));

            try
            {
                var student = await this._studentRepository.GetStudentFromCosmosByEmailAsync(request.Email).ConfigureAwait(false);
                return student;
            }
            catch(Exception ex)
            {
                this._logger.LogError(ex.Message, request);
                throw;
            }
        }
    }
}
