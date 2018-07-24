using AutoMapper;
using Company.Api.Rest.Data;
using Company.Common.Data;

namespace Company.Api.Rest.Impl
{
    public class RestProfile
        : Profile
    {
        public RestProfile()
        {
            CreateMap<RegisterRequestDto, RegisterRequest>();
        }
    }
}
