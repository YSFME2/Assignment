namespace Web.Api.Profiles
{
    public class DomainResponseProfile : Profile
    {
        public DomainResponseProfile()
        {
            CreateMap<Category, CategoryResponse>();
        }
    }
}
