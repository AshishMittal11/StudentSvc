using Mapster;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StudentSvc.Api.Cosmos;
using StudentSvc.Api.Models;
using System;
using System.Text;
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

        public bool IsStudentPresentInCosmos(Student student)
        {
            using (var client = new CosmosClient(_cosmosSettings.PrimaryConnectionString))
            {
                var database = client.GetDatabase(DatabaseId);
                var container = database.GetContainer(ContainerId);
                string query = GenerateQuery(student);
                var iterator = container.GetItemQueryIterator<StudentCosmos>(query);
                return iterator.HasMoreResults;
            }
        }

        private string GenerateQuery(Student student)
        {
            if (!string.IsNullOrWhiteSpace(student.FirstName)
                                || !string.IsNullOrWhiteSpace(student.MiddleName)
                                || !string.IsNullOrWhiteSpace(student.LastName))
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("Select * from c where");

                if (!string.IsNullOrWhiteSpace(student.FirstName))
                {
                    sb.AppendFormat(" lower(c.FirstName) = '{0}'", student.FirstName);
                }

                if (!string.IsNullOrWhiteSpace(student.MiddleName))
                {
                    if (sb.ToString().Contains("c.FirstName"))
                    {
                        sb.AppendFormat(" and lower(c.MiddleName) = '{0}'", student.MiddleName);
                    }
                    else
                    {
                        sb.AppendFormat(" lower(c.MiddleName) = '{0}'", student.MiddleName);
                    }
                }

                if (!string.IsNullOrWhiteSpace(student.LastName))
                {
                    if (sb.ToString().Contains("c.FirstName") || sb.ToString().Contains("c.MiddleName"))
                    {
                        sb.AppendFormat(" and lower(c.LastName) = '{0}'", student.LastName);
                    }
                    else
                    {
                        sb.AppendFormat(" lower(c.LastName) = '{0}'", student.LastName);
                    }
                }

                if (!string.IsNullOrWhiteSpace(student.Email))
                {
                    if (sb.ToString().Contains("c.FirstName") || sb.ToString().Contains("c.MiddleName") || sb.ToString().Contains("c.LastName"))
                    {
                        sb.AppendFormat(" and lower(c.Email) = '{0}'", student.Email);
                    }
                    else
                    {
                        sb.AppendFormat(" lower(c.Email) = '{0}'", student.Email);
                    }
                }

                return sb.ToString();
            }
            else
            {
                throw new Exception("Details are missing");
            }
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
                bool status = false;
                if (!IsStudentPresentInCosmos(student))
                {
                    using (CosmosClient client = new CosmosClient(_cosmosSettings.PrimaryConnectionString))
                    {
                        var cosmosItem = student.Adapt<StudentCosmos>();
                        var database = client.GetDatabase(DatabaseId);
                        var container = database.GetContainer(ContainerId);
                        var response = await container.CreateItemAsync(cosmosItem).ConfigureAwait(false);
                        status = response.StatusCode == System.Net.HttpStatusCode.Created;
                    }
                }
                else
                {
                    status = await UpdateStudentToCosmosAsync(student).ConfigureAwait(false);
                }

                return status;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, student);
                throw;
            }
        }

        public async Task<bool> UpdateStudentToCosmosAsync(Student student)
        {
            using (var client = new CosmosClient(_cosmosSettings.PrimaryConnectionString))
            {
                var cosmosItem = student.Adapt<StudentCosmos>();
                var database = client.GetDatabase(DatabaseId);
                var container = database.GetContainer(ContainerId);
                string query = GenerateQuery(student);
                var iterator = container.GetItemQueryIterator<StudentCosmos>(query);

                while (iterator.HasMoreResults)
                {
                    var item = await iterator.ReadNextAsync().ConfigureAwait(false);
                }

                return true;
            }
        }
    }
}
