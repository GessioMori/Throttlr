using AutoMapper;
using Throttlr.Core.DTOs;
using Throttlr.Core.Entities;

namespace Throttlr.Core.Mappers;
public class RouteConfigProfile : Profile
{
    public RouteConfigProfile()
    {
        this.CreateMap<RouteConfigDto, RouteConfig>();
        this.CreateMap<RouteConfig, RouteConfigReadDto>();
    }
}