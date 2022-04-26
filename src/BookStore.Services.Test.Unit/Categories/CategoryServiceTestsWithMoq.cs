using BookStore.Entities;
using BookStore.Infrastructure.Application;
using BookStore.Services.Categories;
using BookStore.Services.Categories.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Services.Test.Unit.Categories
{
    public class CategoryServiceTestsWithMoq
    {
        [Fact]
        public void Add_add_category_properly()
        {
            var dto = new AddCategoryDto
            {
                Title = "kmjnhujh"
            };

            Mock<CategoryRepository> repository = new Mock<CategoryRepository>();
            Mock<UnitOfWork> unitofwork = new Mock<UnitOfWork>();

            var sut = new CategoryAppService(repository.Object, unitofwork.Object);
            sut.Add(dto);

            repository.Verify(_ => _.Add(It.Is<Category>(_ => _.Title == dto.Title)));
            unitofwork.Verify(_ => _.Commit());
        }
    }
}
