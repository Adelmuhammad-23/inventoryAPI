using AutoMapper;
using IMS.Application.DTOs.CategoryDTOs;
using IMS.Domain.Entities;
using IMS.Domain.Interfaces;
using IMS.Domain.UnitOfWorkInterface;

namespace IMS.Application.Services
{
    public class CategoryServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;


        public CategoryServices(IUnitOfWork unitOfWork, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CategoryListDTO>> GetCategoriesAsync()
        {
            var categories = await _unitOfWork.CategoryUOF.GetAllAsync(p => p.Products);
            var categoriesMapping = _mapper.Map<List<CategoryListDTO>>(categories);

            return categoriesMapping;

        }
        public async Task<AddCategoryDTO> GetCategoryByIdAsync(int id)
        {
            var categories = await _unitOfWork.CategoryUOF.GetByIdAsync(id);
            if (categories == null)
                return null;
            var categoriesMapping = _mapper.Map<AddCategoryDTO>(categories);

            return categoriesMapping;

        }
        public async Task<AddCategoryDTO> AddCategoriesAsync(AddCategoryDTO addCategory)
        {
            var AddCategory = _mapper.Map<Category>(addCategory);
            var addCategoryDB = await _categoryRepository.AddAsync(AddCategory);

            return addCategory;

        }
        public async Task<string> UpdateCategoriesAsync(int id, UpdateCategoryDTO updateCategory)
        {
            var category = await _unitOfWork.CategoryUOF.GetByIdAsync(id);
            if (category == null)
                return null;

            var categoriesMapping = _mapper.Map<Category>(updateCategory);

            var categories = await _unitOfWork.CategoryUOF.UpdatAsync(categoriesMapping);

            return "Update category is successfully";

        }
        public async Task<string> DeleteCategoriesAsync(int id)
        {
            var category = await _unitOfWork.CategoryUOF.GetByIdAsync(id);
            if (category == null)
                return null;

            var categories = await _unitOfWork.CategoryUOF.DeleteAsync(id);

            return "Delete category is successfully";

        }
    }
}
