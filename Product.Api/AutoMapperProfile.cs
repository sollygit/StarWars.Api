using AutoMapper;
using Newtonsoft.Json.Linq;
using Products.Model;
using System;

namespace Products.Api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<JObject, Movie>()
                .ForMember(o => o.ID, map => map.MapFrom(o => o["ID"].Value<string>()))
                .ForMember(o => o.Title, map => map.MapFrom(o => o["Title"].Value<string>()))
                .ForMember(o => o.Year, map => map.MapFrom(o => o["Year"].Value<string>()))
                .ForMember(o => o.Type, map => map.MapFrom(o => o["Type"].Value<string>()))
                .ForMember(o => o.Poster, map => map.MapFrom(o => o["Poster"].Value<string>()))
                .ForMember(o => o.Price, map => map.MapFrom(o => GetRandomPrice()))
                .ReverseMap();
        }

        private decimal GetRandomPrice()
        {
            return decimal.Parse(string.Format("{0:0.##}", new Random().NextDouble() * 1000));
        }
    }
}