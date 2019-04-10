using AutoMapper;
using Library.Contracts.Request.Author;
using Library.Contracts.Response.Author;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using LibraryAPI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LibraryAPI.Controllers
{
    [Route("api/authors")]
    public class AuthorsController : Controller
    {
        private const string GetAuthorRoute = "GetAuthor";

        private const string GetAuthorsRoute = "GetAuthors";

        private const string PaginationMetadata = "X-PaginationMetadata";

        private readonly ILogger<AuthorsController> logger;

        private readonly ILibraryRepository libraryRepository;

        private readonly IUrlHelper urlHelper;

        public AuthorsController(ILogger<AuthorsController> logger, ILibraryRepository libraryRepository, IUrlHelper urlHelper)
        {
            this.logger = logger;
            this.libraryRepository = libraryRepository;
            this.urlHelper = urlHelper;
        }

        [HttpGet(Name = GetAuthorsRoute)]
        public IActionResult GetAuthors(AuthorResouceParameter authorsResourceParameters)
        {
            var authorsFromRepo = libraryRepository.GetAuthors(authorsResourceParameters);

            var paginationMetadata = new
            {
                totalCount = authorsFromRepo.TotalCount,
                pageSize = authorsFromRepo.PageSize,
                currentPage = authorsFromRepo.CurrentPage,
                totalPages = authorsFromRepo.TotalPages,
                previousPageLink = authorsFromRepo.HasPrevioius ? CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.PreviousPage) : null,
                nextPageLink = authorsFromRepo.HasNext ? CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.NextPage) : null
            };

            Response.Headers.Add(PaginationMetadata, JsonConvert.SerializeObject(paginationMetadata));

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

        private string CreateAuthorsResourceUri(AuthorResouceParameter authorsResourceParameters, ResourceUriType type)
        {
            if (type == ResourceUriType.PreviousPage)
            {
                return urlHelper.Link(GetAuthorsRoute,
                        new
                        {
                            pageNumber = authorsResourceParameters.PageNumber - 1,
                            pageSize = authorsResourceParameters.PageSize,
                            genre = authorsResourceParameters.Genre,
                            search = authorsResourceParameters.Search,
                            orderBy = authorsResourceParameters.OrderBy
                        });
            }
            else
            {
                return urlHelper.Link(GetAuthorsRoute,
                        new
                        {
                            pageNumber = authorsResourceParameters.PageNumber + 1,
                            pageSize = authorsResourceParameters.PageSize,
                            genre = authorsResourceParameters.Genre,
                            search = authorsResourceParameters.Search,
                            orderBy = authorsResourceParameters.OrderBy
                        });
            }            
        }
    }
}