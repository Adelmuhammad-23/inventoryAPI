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
        public async Task<GetCategoryByIdDTO> GetCategoryByIdAsync(int id)
        {
            var categories = await _unitOfWork.CategoryUOF.GetByIdAsync(id, p => p.Products);
            if (categories == null)
                return null;


            var categoriesMapping = _mapper.Map<GetCategoryByIdDTO>(categories);


            return categoriesMapping;

        }
        public async Task<string> AddProductToCategoryAsync(int categoryId, int productId)
        {
            var category = await _unitOfWork.CategoryUOF.GetByIdAsync(categoryId, p => p.Products);
            if (category == null)
                return null;

            var product = await _unitOfWork.ProductsUOF.GetByIdAsync(productId);
            if (product == null)
                return null;


            category.Products.Add(product);
            await _unitOfWork.Complete();


            return $"Add product with iD: {productId} in category with Id: {categoryId} is successfully";

        }
        public async Task<string> RemoveProductToCategoryAsync(int categoryId, int productId)
        {
            var category = await _unitOfWork.CategoryUOF.GetByIdAsync(categoryId, p => p.Products);
            if (category == null)
                return null;

            var product = await _unitOfWork.ProductsUOF.GetByIdAsync(productId);
            if (product == null)
                return null;


            category.Products.Remove(product);
            await _unitOfWork.Complete();


            return $"Remove product with iD: {productId} in category with Id: {categoryId} is successfully";

        }
        public async Task<AddCategoryDTO> AddCategoriesAsync(AddCategoryDTO addCategory)
        {
            var AddCategory = _mapper.Map<Category>(addCategory);
            var addCategoryDB = await _unitOfWork.CategoryUOF.AddAsync(AddCategory);
            await _unitOfWork.Complete();



            return addCategory;

        }
        public async Task<string> UpdateCategoriesAsync(int id, UpdateCategoryDTO updateCategory)
        {
            var category = await _unitOfWork.CategoryUOF.GetByIdAsync(id);
            if (category == null)
                return null;

            var categoriesMapping = _mapper.Map(updateCategory, category);

            var categories = await _unitOfWork.CategoryUOF.UpdatAsync(categoriesMapping);
            await _unitOfWork.Complete();



            return "Update category is successfully";

        }
        public async Task<string> DeleteCategoriesAsync(int id)
        {
            var category = await _unitOfWork.CategoryUOF.GetByIdAsync(id);
            if (category == null)
                return null;
            if (category.Products is not null)
                return "Can't Delete this category because with many products !";

            var categories = await _unitOfWork.CategoryUOF.DeleteAsync(id);
            await _unitOfWork.Complete();


            return "Delete category is successfully";

        }
    }
}
