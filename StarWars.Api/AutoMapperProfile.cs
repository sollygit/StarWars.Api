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
            CreateMap<Movie, MovieViewModel>()
                .ForMember(o => o.ID, map => map.MapFrom(o => o.ID))
                .ForMember(o => o.Title, map => map.MapFrom(o => o.Title))
                .ForMember(o => o.Year, map => map.MapFrom(o => o.Year))
                .ForMember(o => o.Type, map => map.MapFrom(o => o.Type))
                .ForMember(o => o.Poster, map => map.MapFrom(o => o.Poster))
                .ForMember(o => o.Price, map => map.MapFrom(o => decimal.Parse(string.Format("{0:0.##}", new Random().NextDouble() * 1000))))
                .ReverseMap();

            CreateMap<MovieDetails, MovieDetailsViewModel>()
                .ForMember(o => o.Rated, map => map.MapFrom(o => o.Rated))
                .ForMember(o => o.Released, map => map.MapFrom(o => o.Released))
                .ForMember(o => o.Runtime, map => map.MapFrom(o => o.Runtime))
                .ForMember(o => o.Genre, map => map.MapFrom(o => o.Genre))
                .ForMember(o => o.Director, map => map.MapFrom(o => o.Director))
                .ForMember(o => o.Writer, map => map.MapFrom(o => o.Writer))
                .ForMember(o => o.Language, map => map.MapFrom(o => o.Language))
                .ForMember(o => o.Country, map => map.MapFrom(o => o.Country))
                .ForMember(o => o.Awards, map => map.MapFrom(o => o.Awards))
                .ForMember(o => o.Metascore, map => map.MapFrom(o => o.Metascore))
                .ForMember(o => o.Rating, map => map.MapFrom(o => o.Rating))
                .ForMember(o => o.Votes, map => map.MapFrom(o => o.Votes))
                .ReverseMap();
        }
    }
}