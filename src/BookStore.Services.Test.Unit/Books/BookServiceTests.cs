using BookStore.Infrastructure.Application;
using BookStore.Infrastructure.Test;
using BookStore.Persistence.EF;
using BookStore.Persistence.EF.Books;
using BookStore.Services.Books;
using BookStore.Services.Books.Contracts;
using BookStore.Test.Tools.Categories;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Services.Test.Unit.Books
{
    public class BookServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly BookService _sut;
        private readonly BookRepository _repository;

        public BookServiceTests()
        {
            _dataContext =
                new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFBookRepository(_dataContext);
            _sut = new BookAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_Adds_Book_properly()
        {
            var category = CategoryFactory.CreateCategory("categorytitle");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            AddBookDto dto = GenerateAddBookDto(category);

            _sut.Add(dto);

            var Expected = _dataContext.Books.FirstOrDefault();
            Expected.Title.Should().Be(dto.Title);
            Expected.Description.Should().Be(dto.Description);
            Expected.Author.Should().Be(dto.Author);
            Expected.Pages.Should().Be(dto.Pages);
            Expected.CategoryId.Should().Be(dto.CategoryId);
        }

        private static AddBookDto GenerateAddBookDto(Entities.Category category)
        {
            return new AddBookDto
            {
                Title = "dummy",
                Author = "dummyauthor",
                Description = "dummypeople",
                Pages = 17,
                CategoryId = category.Id,
            };
        }
    }
}
