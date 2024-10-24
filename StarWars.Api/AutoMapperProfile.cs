using AutoMapper;
using StarWars.Model;
using StarWars.Model.ViewModels;
using System;

namespace StarWars.Api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Movie, MovieView>()
                .ForMember(o => o.ID, map => map.MapFrom(o => o.ID))
                .ForMember(o => o.Title, map => map.MapFrom(o => o.Title))
                .ForMember(o => o.Year, map => map.MapFrom(o => o.Year))
                .ForMember(o => o.Type, map => map.MapFrom(o => o.Type))
                .ForMember(o => o.Poster, map => map.MapFrom(o => o.Poster))
                .ForMember(o => o.Price, map => map.MapFrom(o => decimal.Parse(string.Format("{0:0.##}", new Random().NextDouble() * 1000))))
                .ReverseMap();
        }
    }
}