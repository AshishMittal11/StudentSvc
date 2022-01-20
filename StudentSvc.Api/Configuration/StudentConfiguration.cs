using Mapster;
using StudentSvc.Api.DTO;
using StudentSvc.Api.Models;
using System;

namespace StudentSvc.Api.Configuration
{
    /************************************************************************
     * All the mapter related configuration will be stored here.
     * **********************************************************************/
    public static class StudentConfiguration
    {
        public static void Configure()
        {
            TypeAdapterConfig<StudentDto, Student>
                .NewConfig()
                .Map(dest => dest.CreatedDate, src => DateTimeOffset.UtcNow)
                .Map(dest => dest.ModifiedDate, src => DateTimeOffset.UtcNow);
        }
    }
}
