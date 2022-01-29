using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StudentSvc.Api.Azure;
using StudentSvc.Api.Database;
using StudentSvc.Api.DTO;
using StudentSvc.Api.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using MyPayload = StudentSvc.Api.Azure.MyPayload;

namespace StudentSvc.Api.CQRS.Command.CommandHandler
{
    public class StudentRegisterCommandHandler : IRequestHandler<StudentRegisterCommand, bool>
    {
        private readonly ILogger<StudentRegisterCommandHandler> _logger;
        private readonly StudentContext _context;
        private readonly ServiceBusTopicSender _topicSender;

        public StudentRegisterCommandHandler(
            ILogger<StudentRegisterCommandHandler> logger,
            StudentContext context,
            ServiceBusTopicSender topicSender)
        {
            this._logger = logger;
            this._context = context;
            this._topicSender = topicSender;
        }

        public async Task<bool> Handle(StudentRegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbStudent = request.Student.Adapt<Student>();
                await _context.StudentSet.AddAsync(dbStudent).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);

                //raise the event to send the mail
                var mail = new MailDto();
                mail.MailTo = request.Student.Email;
                mail.Subject = "major";
                mail.Body = $"Hello";

                var payload = new MyPayload
                {
                    Url = "http://localhost:5228/api/email",
                    Message = JsonConvert.SerializeObject(mail)
                };

                // send message to the topic.
                await this._topicSender.SendMessageAsync(payload).ConfigureAwait(false);
                
                return true;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message, request);
                throw;
            }
        }
    }
}
