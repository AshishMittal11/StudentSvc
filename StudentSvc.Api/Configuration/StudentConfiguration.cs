using Mapster;
using StudentSvc.Api.Cosmos;
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
                .ForType()
                .Map(dest => dest.CreatedDate, src => DateTimeOffset.UtcNow)
                .Map(dest => dest.ModifiedDate, src => DateTimeOffset.UtcNow)
                .Map(dest => dest.AdmittedClassId, src => int.Parse(src.AdmittedClassId))
                .AfterMapping((src, dest) =>
                {
                    if (!string.IsNullOrWhiteSpace(src.PhotoBase64))
                    {
                        dest.Photo = Convert.FromBase64String(src.PhotoBase64);
                    }
                });

            TypeAdapterConfig<Student, StudentDto>
                .ForType()
                .Map(dest => dest.CreatedDate, src => src.CreatedDate.ToString("MM/dd/yyyy"))
                .Map(dest => dest.ModifiedDate, src => src.ModifiedDate.ToString("MM/dd/yyyy"))
                .Map(dest => dest.Dob, src => src.Dob.ToString("MM/dd/yyyy"))
                .Map(dest => dest.AdmittedClassId, src => src.AdmittedClassId.ToString())
                .AfterMapping((src, dest) =>
                {
                    if (src?.Photo?.Length > 0)
                    {
                        dest.PhotoBase64 = Convert.ToBase64String(src.Photo);
                    }
                });

            TypeAdapterConfig<Student, StudentCosmos>
                .ForType()
                .Map(dest => dest.StudentId, src => src.Id)
                .Map(dest => dest.Id, src => Guid.NewGuid())
                .Map(dest => dest.CreatedOn, src => DateTimeOffset.UtcNow)
                .Map(dest => dest.ModifiedOn, src => DateTimeOffset.UtcNow);

            TypeAdapterConfig<StudentCosmos, Student>
                .ForType()
                .Map(dest => dest.Id, src => src.StudentId);
        }
    }
}