using BookStore.Entities;
using BookStore.Infrastructure.Application;
using BookStore.Services.Categories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Categories
{
    public class CategoryAppService : CategoryService
    {
        private readonly CategoryRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        
            public CategoryAppService(
                CategoryRepository repository ,
                UnitOfWork unitOfWork)
            {
            _repository = repository;
            _unitOfWork = unitOfWork;
            }
        public void Add(AddCategoryDto dto)
        {
            var category = new Category
            {
                Title = dto.Title,
            };

            _repository.Add(category);

            _unitOfWork.Commit();
        }

        public IList<GetCategoryDto> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(int id, UpdateCategoryDto dto)
        {
            var category = _repository.Getbyid(id);
            PreventUpdateWhenNotExistCategory(category);

            category.Title = dto.Title;
            _unitOfWork.Commit();
        }

        private static void PreventUpdateWhenNotExistCategory(Category category)
        {
            if (category == null)
            {
                throw new CategoryNotFoundException();
            }
        }
    }
}
