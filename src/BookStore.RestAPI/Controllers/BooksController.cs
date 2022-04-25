using BookStore.Services.Books.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.RestAPI.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _service;
        public BooksController(BookService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddBookDto dto)
        {
            _service.Add(dto);
        }

        [HttpPut("{id}")]
        public void Update(int id , UpdateBookDto dto)
        {
            _service.Update(id, dto);
        }
    }
}
