﻿using BookStore.Entities;
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

            CheckAddBookTest(dto);
        }

        [Fact]
        public void Update_update_book_properly()
        {
            var category = CategoryFactory.CreateCategory("categorytitle");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            AddBookDto dto = GenerateAddBookDto(category);
            _sut.Add(dto);
            var book = _dataContext.Books.FirstOrDefault();

            UpdateBookDto updateBookDto = GenerateUpdateBookDto();

            _sut.Update(book.Id, updateBookDto);

            ChechUpdateTest(updateBookDto);
        }

        [Fact]
        public void Update_update_throw_BookNotFoundExeption_when_book_with_given_id_that_not_exist()
        {
            int dummyId = 1000;

            UpdateBookDto updateBookDto = GenerateUpdateBookDto();

            Action expected = () => _sut.Update(dummyId, updateBookDto);

            expected.Should().Throw<BookNotFoundException>();
        }

        [Fact]
        public void Delete_delete_book_properly()
        {
            var category = CategoryFactory.CreateCategory("categorytitle");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            AddBookDto dto = GenerateAddBookDto(category);
            _sut.Add(dto);
            var book = _dataContext.Books.FirstOrDefault();

            _sut.Delete(book.Id);
            _dataContext.Books.Should().HaveCount(0);
        }

        [Fact]
        public void Delete_delete_throw_BookNotFoundExeption_when_book_with_given_id_that_not_exist()
        {
            int dummyId = 1000;

            Action expected = () => _sut.Delete(dummyId);

            expected.Should().Throw<BookNotFoundException>();
        }

        [Fact]
        public void GetAll_returns_all_books()
        {
            var category = CategoryFactory.CreateCategory("categorytitle");
            _dataContext.Manipulate(_ => _.Categories.Add(category));

            CreateCategoriesInDatabase(category);

            var Expected = _sut.Getall();
            Expected.Should().HaveCount(2);
        }

        private void CheckAddBookTest(AddBookDto dto)
        {
            var Expected = _dataContext.Books.FirstOrDefault();
            Expected.Title.Should().Be(dto.Title);
            Expected.Description.Should().Be(dto.Description);
            Expected.Author.Should().Be(dto.Author);
            Expected.Pages.Should().Be(dto.Pages);
            Expected.CategoryId.Should().Be(dto.CategoryId);
        }

        private void ChechUpdateTest(UpdateBookDto updateBookDto)
        {
            var Expected = _dataContext.Books.FirstOrDefault();
            Expected.Title.Should().Be(updateBookDto.Title);
            Expected.Description.Should().Be(updateBookDto.Description);
            Expected.Author.Should().Be(updateBookDto.Author);
            Expected.Pages.Should().Be(updateBookDto.Pages);
            Expected.CategoryId.Should().Be(updateBookDto.CategoryId);
        }

        private void CreateCategoriesInDatabase(Category category)
        {
            var books = new List<Book>
            {
              new Book
              {
                  Title = "dnhk",
                  Author = "dse",
                  Pages = 7,
                  Description = "sfcse",
                  CategoryId = category.Id,
              },
              new Book
              {

                  Title = "eeee",
                  Author = "eee",
                  Pages = 12,
                  Description = "eee",
                  CategoryId = category.Id,
              }
            };
            _dataContext.Manipulate(_ => _.Books.AddRange(books));
        }

        private static UpdateBookDto GenerateUpdateBookDto()
        {
            return new UpdateBookDto
            {
                Title = "dfbd",
                Pages = 20,
                Author = "dvs",
                Description = "fdvs",
                CategoryId = 1,
            };
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
