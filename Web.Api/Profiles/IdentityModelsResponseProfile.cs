using Application.Models;
using Application.Models.Identity;
using Web.Contracts.v1.Responses.Identity;

namespace Web.Api.Profiles
{
    public class IdentityModelsResponseProfile : Profile
    {
        public IdentityModelsResponseProfile()
        {
            CreateMap<AuthenticationResult, AuthenticationResponse>();
            CreateMap<AuthenticationResult, AuthenticationResponse>();
            CreateMap<Error, ErrorResponse>();
            CreateMap<ChangeUserRoleResult, ChangeUserRoleResponse>();
        }
    }
}
