using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StudentSvc.Api.Azure;
using StudentSvc.Api.CQRS.Command;
using StudentSvc.Api.CQRS.Query;
using StudentSvc.Api.DTO;
using StudentSvc.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentSvc.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StudentController> _logger;

        public StudentController(IMediator mediator, ILogger<StudentController> logger)
        {
            this._mediator = mediator;
            this._logger = logger;
        }

        [HttpGet("view")]
        public async Task<List<StudentDto>> GetStudents()
        {
            var students = await this._mediator.Send(new GetStudentsQuery()).ConfigureAwait(false);
            return students;
        }

        // GET api/<StudentController>/5
        [HttpGet("view/{id}")]
        public async Task<StudentDto> GetStudentById(int id)
        {
            var student = await this._mediator.Send(new GetStudentByIdQuery { Id = id }).ConfigureAwait(false);
            return student;
        }

        [HttpGet("viewbyfirstname")]
        public async Task<List<StudentDto>> GetStudentsByFirstName([FromQuery] string firstNameChar)
        {
            var students = await this._mediator.Send(new GetStudentsByFirstNameCharQuery { FirstNameChar = firstNameChar }).ConfigureAwait(false);
            return students;
        }

        // POST api/<StudentController>
        [HttpPost("register")]
        public async Task<bool> Post([FromBody] StudentDto student)
        {
            bool status = await this._mediator.Send(new StudentRegisterCommand { Student = student }).ConfigureAwait(false);
            return status;
        }

        // POST api/<StudentController>
        [HttpPost("cosmos/register")]
        public async Task<bool> CosmosRegister([FromBody] StudentDto student)
        {
            bool status = await this._mediator.Send(new RegisterStudentInCosmosCommand { Student = student }).ConfigureAwait(false);
            return status;
        }

        [HttpGet("view/cosmos/student")]
        public async Task<StudentDto> GetStudentFromCosmos([FromQuery] string email)
        {
            var student = await this._mediator.Send(new GetStudentFromCosmosByEmailQuery { Email = email }).ConfigureAwait(false);
            return student;
        }

        // PUT api/<StudentController>/5
        [HttpPut("{id}")]
        public async Task<bool> UpdateStudent (int id, [FromBody] StudentDto student)
        {
            student.Id = id;
            var result = await this._mediator.Send(new UpdateStudentCommand { Student = student }).ConfigureAwait(false);
            return result;
        }

        // Delete api/<studentcontroller>/5
        [HttpDelete("{id}")]
        public async Task<bool> DeleteStudent(int id)
        {
            bool status = await this._mediator.Send(new DeleteStudentCommand { StudentId = id }).ConfigureAwait(false);
            return status;
        }
    }
}
