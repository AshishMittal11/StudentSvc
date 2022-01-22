﻿using Mapster;
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
                .AfterMapping((src, dest) =>
                {
                    if (src?.Photo?.Length > 0)
                    {
                        dest.PhotoBase64 = Convert.ToBase64String(src.Photo);
                    }
                });
        }
    }
}
