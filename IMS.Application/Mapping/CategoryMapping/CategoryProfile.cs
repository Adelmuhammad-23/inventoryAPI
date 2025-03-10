using AutoMapper;

namespace IMS.Application.Mapping.CategoryMapping
{
    public partial class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CategoryListQueryMapping();
            GetCategoryByIdMapping();

            AddCategoryMapping();
            UpdateCategoryMapping();
        }
    }
}
