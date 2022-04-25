using BookStore.Entities;
using BookStore.Services.Books.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Persistence.EF.Books
{
    public class EFBookRepository : BookRepository
    {
        private EFDataContext _dataContext;

        public EFBookRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Book book)
        {
            _dataContext.Books.Add(book);
        }

        public Book GetbyId(int id)
        {
            return _dataContext.Books.Find(id);
        }

        public void Remove(Book book)
        {
            _dataContext.Books.Remove(book);
        }
    }
}
