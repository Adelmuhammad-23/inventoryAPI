using IMS.Application.DTOs.ProductDTOs;
using IMS.Domain.Entities;

namespace IMS.Application.Mapping.ProductMapping
{
    public partial class ProductMappingProfile
    {
        public void UpdateProductCommandMapping()
        {
            CreateMap<UpdateProductDTO, Product>();

        }
    }
}
