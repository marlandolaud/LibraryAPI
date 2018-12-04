using AutoMapper;
using LibraryAPI.Entities;
using LibraryAPI.Helpers;
using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LibraryAPI.Controllers
{
    [Route("api/authorcollections")]
    public class AuthorCollectionController: Controller
    {
        private readonly ILibraryRepository libraryRepository;

        public AuthorCollectionController(ILibraryRepository libraryRepository)
        {
            this.libraryRepository = libraryRepository;
        }

        [HttpPost]
        public IActionResult CreateAuthorCollection([FromBody] IEnumerable<AuthorForCreationDto> authorCollection)
        {
            if (authorCollection == null)
            {
                return BadRequest();
            }

            var authorEntities = Mapper.Map<IEnumerable<Author>>(authorCollection);

            foreach (var author in authorEntities)
            {
                libraryRepository.AddAuthor(author);
            }

            if (!libraryRepository.Save())
            {
                throw new Exception("Creating an author failed to save");
            }

            var authorsToReturn = Mapper.Map<IEnumerable<AuthorDto>>(authorEntities);

            var ids = string.Join(",", authorsToReturn.Select(item => item.Id));

            return CreatedAtRoute("GetAuthorCollection", new { ids = ids }, authorsToReturn);
        }

        [HttpGet("({ids})", Name = "GetAuthorCollection")]
        public IActionResult GetAuthorCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var authorEntities = libraryRepository.GetAuthors(ids);

            if (ids.Count() != authorEntities.Count())
            {
                return NotFound();
            }

            var authorsToReturn = Mapper.Map<IEnumerable<AuthorDto>>(authorEntities);

            return Ok(authorsToReturn);
        }
    }
}
