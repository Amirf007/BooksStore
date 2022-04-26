using BookStore.Entities;
using BookStore.Infrastructure.Application;
using BookStore.Services.Books.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Books
{
    public class BookAppService : BookService
    {
        private BookRepository _repository;
        private UnitOfWork _unitOfWork;

        public BookAppService(BookRepository repository, UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddBookDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Description = dto.Description,
                Pages = dto.Pages,
                CategoryId = dto.CategoryId,
            };

            _repository.Add(book);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var book = _repository.GetbyId(id);
            PreventDeleteBookWhenNotExistBook(book);

            _repository.Remove(book);
            _unitOfWork.Commit();
        }

        private static void PreventDeleteBookWhenNotExistBook(Book book)
        {
            if (book == null)
            {
                throw new BookNotFoundException();
            }
        }

        public void Update(int id, UpdateBookDto dto)
        {
            var book = _repository.GetbyId(id);
            PreventUpdateBookWhenNotExistBook(book);

            book.Title = dto.Title;
            book.Description = dto.Description;
            book.Pages = dto.Pages;
            book.Author = dto.Author;
            book.CategoryId = dto.CategoryId;

            _unitOfWork.Commit();
        }

        private static void PreventUpdateBookWhenNotExistBook(Book book)
        {
            if (book == null)
            {
                throw new BookNotFoundException();
            }
        }
    }
}
