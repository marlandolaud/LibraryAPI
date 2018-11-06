using AutoMapper;
using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LibraryAPI.Controllers
{
    [Route("api/authors")]
    public class AuthorsController : Controller
    {
        private readonly ILibraryRepository libraryRepository;

        public AuthorsController(ILibraryRepository libraryRepository)
        {
            this.libraryRepository = libraryRepository;
        }

        [HttpGet]
        public IActionResult GetAuthors()
        {
            var authorsFromRepo = libraryRepository.GetAuthors();

            var authors = Mapper.Map<IEnumerable<AuthorDTO>>(authorsFromRepo);

            return new JsonResult(authors);
        }
    }
}