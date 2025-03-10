using IMS.Application.DTOs.CategoryDTOs;
using IMS.Domain.Entities;

namespace IMS.Application.Mapping.CategoryMapping
{
    public partial class CategoryProfile
    {
        public void GetCategoryByIdMapping()
        {
            CreateMap<Category, GetCategoryByIdDTO>()
                    .ForMember(dest => dest.Productslist, opt => opt.MapFrom(src => src.Products));

            CreateMap<Product, Productslist>();
        }
    }
}