using AutoMapper;
using Library.Domain.Entities;
using LibraryAPI.Models;
using Library.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using LibraryAPI.Helpers;
using Library.Domain;

namespace LibraryAPI.Controllers
{
    [Route("api/authors")]
    public class AuthorsController : Controller
    {
        private const string GetAuthorRoute = "GetAuthor";
        private readonly ILogger<AuthorsController> logger;
        private readonly ILibraryRepository libraryRepository;

        public AuthorsController(ILogger<AuthorsController> logger, ILibraryRepository libraryRepository)
        {
            this.logger = logger;
            this.libraryRepository = libraryRepository;
        }

        [HttpGet]
        public IActionResult GetAuthors(AuthorResouceParameter authorsResourceParameters)
        {
            var authorsFromRepo = libraryRepository.GetAuthors(authorsResourceParameters);

            var authors = Mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);

            return Ok(authors);
        }

        [HttpGet("{id}", Name = GetAuthorRoute)]
        public IActionResult GetAuthor(Guid id)
        {
            var authorFromRepo = libraryRepository.GetAuthor(id);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            var author = Mapper.Map<AuthorDto>(authorFromRepo);

            return Ok(author);
        }

        [HttpPost]
        public IActionResult CreateAuthor([FromBody] AuthorForCreationDto author)
        {
            if (author == null)
            {
                return BadRequest();
            }

            var authorEntity = Mapper.Map<Author>(author);

            libraryRepository.AddAuthor(authorEntity);

            if (!libraryRepository.Save())
            {
                //return StatusCode(500, "");
                throw new Exception("Creating an author failed to save");
            }

            var authorToReturn = Mapper.Map<AuthorDto>(authorEntity);

            return CreatedAtRoute(GetAuthorRoute, new
            {
                id = authorToReturn.Id
            }, 
            authorToReturn);
        }

        [HttpPost("{id}")]
        public IActionResult BlockAuthorCreation(Guid id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            if (libraryRepository.AuthorExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(Guid id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var authorFromRepo = libraryRepository.GetAuthor(id);
            if (authorFromRepo == null)
            {
                return NotFound();
            }

            libraryRepository.DeleteAuthor(authorFromRepo);

            if (libraryRepository.Save())
            {
                logger.LogInformation(1000, $"Deleted author {id}");
            }
            else
            {
                throw new Exception($"Deleting author {id} failed");
            }

            return NoContent();
        }
    }
}