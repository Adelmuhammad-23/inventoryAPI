using IMS.Application.DTOs;
using IMS.Domain.Entities;

namespace IMS.Application.Mapping.ProductMapping
{
    public partial class ProductMappingProfile
    {

        public void GetProductDetailsMapping()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.CategoryName, src => src.MapFrom(cn => cn.Category.Name));
        }

    }
}
