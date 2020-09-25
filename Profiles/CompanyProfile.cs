using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThreeApi.Entities;
using ThreeApi.Models;

namespace ThreeApi.Profiles
{
    public class CompanyProfile: Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(
                    dest => dest.CompanyName,
                    opt => opt.MapFrom(src => src.Name));
            CreateMap<CompanyAddDto, Company>();
        }
    }
}
