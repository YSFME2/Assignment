namespace Web.Api.Profiles
{
    public class DomainRequestProfile : Profile
    {
        public DomainRequestProfile()
        {
            CreateMap<UpsertCategoryRequest, Category>();
        }
    }
}
