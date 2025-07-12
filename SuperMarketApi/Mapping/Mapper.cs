namespace SuperMarketApi.Mapping
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<CreateProductDto, Product>();
        }
    }
}   