using Mapster;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StudentSvc.Api.Cosmos;
using StudentSvc.Api.Models;
using System;
using System.Threading.Tasks;

namespace StudentSvc.Api.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private const string DatabaseId = "TestDB";
        private const string ContainerId = "TestContainer";
        private readonly ILogger<StudentRepository> _logger;
        private readonly CosmosSettings _cosmosSettings;

        public StudentRepository(ILogger<StudentRepository> logger, IOptions<CosmosSettings> options)
        {
            this._logger = logger;
            this._cosmosSettings = options.Value;
        }

        public Task<bool> IsStudentPresentInCosmosAsync(Student student)
        {
            //using (var client = new CosmosClient(_cosmosSettings.PrimaryConnectionString))
            //{
            //    var database = client.GetDatabase(DatabaseId);
            //    var container = database.GetContainer(ContainerId);
            //    string query = $"SELECT * FROM c where lower(c.FirstName) = '{student.FirstName.ToLower()}' and lower(c.LastName) = '{student.LastName.ToLower()}' and lower(c.MiddleName) = '{student.MiddleName.ToLower()}' and lower(c.Email) = '{student.Email.ToLower()}'";   
            //    var iterator = container.GetItemQueryIterator<StudentCosmos>(queryText: query);
            //}
            
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves student information to the cosmos....
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public async Task<bool> SaveStudentToCosmosAsync(Student student)
        {
            try
            {
                using (CosmosClient client = new CosmosClient(_cosmosSettings.PrimaryConnectionString))
                {
                    var cosmosItem = student.Adapt<StudentCosmos>();
                    var database = client.GetDatabase(DatabaseId);
                    var container = database.GetContainer(ContainerId);
                    var response = await container.CreateItemAsync(cosmosItem).ConfigureAwait(false);

                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, student);
                throw;
            }
        }
    }
}
