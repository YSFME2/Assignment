using Application.Models;
using Application.Models.Identity;
using Web.Contracts.v1.Responses.Identity;

namespace Web.Api.Profiles
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<UpsertCategoryRequest, Category>();
            CreateMap<Category, CategoryResponse>();
            CreateMap<Product, ProductResponse>();
            CreateMap<UpsertProductRequest, Product>();
            CreateMap<CartItem, CartItemResponse>();

            CreateMap<AuthenticationResult, AuthenticationResponse>();
            CreateMap<AuthenticationResult, AuthenticationResponse>();
            CreateMap<ChangeUserRoleResult, ChangeUserRoleResponse>();

            CreateMap<Error, ErrorResponse>();
        }
    }
}
