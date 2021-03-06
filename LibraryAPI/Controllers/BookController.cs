﻿using AutoMapper;
using Library.Contracts.Request.Book;
using Library.Contracts.Response.Book;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using LibraryAPI.Helpers;
using LibraryAPI.Models.Validations;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace LibraryAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/authors/{authorId}/Books")]
    public class BookController : Controller
    {
        private const string GetBookForAuthorRoute = "GetBookForAuthor";
        private readonly ILogger<BookController> logger;
        private readonly ILibraryRepository libraryRepository;
        private readonly IBookValidation bookValidation;

        public BookController(ILogger<BookController> logger, ILibraryRepository libraryRepository, IBookValidation bookValidation)
        {
            this.logger = logger;
            this.libraryRepository = libraryRepository;
            this.bookValidation = bookValidation;
        }

        [HttpGet()]
        public IActionResult GetBooksForAuthor(Guid authorId)
        {
            if (!libraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var booksForAuthorFromRepo = libraryRepository.GetBooksForAuthor(authorId);

            var books = Mapper.Map<IEnumerable<BookDto>>(booksForAuthorFromRepo);

            return Ok(books);
        }

        [HttpGet("{id}", Name = GetBookForAuthorRoute)]
        public IActionResult GetBookForAuthor(Guid authorId, Guid id)
        {
            if(!libraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var bookForAuthorFromRepo = libraryRepository.GetBookForAuthor(authorId, id);
            if (bookForAuthorFromRepo == null)
            {
                return NotFound();
            }

            var book = Mapper.Map<BookDto>(bookForAuthorFromRepo);

            return Ok(book);
        }

        [HttpPost]
        public IActionResult CreateBookForAuthor(Guid authorId, [FromBody] BookForCreationDto book)
        {
            if (book == null)
            {
                return BadRequest();
            }

            bookValidation.Validate(book, ModelState);

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            if (!libraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var bookEntity = Mapper.Map<Book>(book);

            libraryRepository.AddBookForAuthor(authorId, bookEntity);

            if (!libraryRepository.Save())
            {
                throw new Exception($"creating a book for author {authorId} failed to save");
            }

            var book2Return = Mapper.Map<BookDto>(bookEntity);

            return CreatedAtRoute(GetBookForAuthorRoute, new { id = book2Return.Id }, book2Return);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBookForAuthor(Guid authorId, Guid id)
        {
            if (!libraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var bookForAuthorFromRepo = libraryRepository.GetBookForAuthor(authorId, id);
            if (bookForAuthorFromRepo == null)
            {
                return NotFound();
            }

            libraryRepository.DeleteBook(bookForAuthorFromRepo);

            if (libraryRepository.Save())
            {
                logger.LogInformation(1000, $"Deleted book {id} for author {authorId}");
            }
            else
            {
                throw new Exception($"Deleting book {id} for author {authorId} failed on save.");
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBookForAuthor(Guid authorId, Guid id,
            [FromBody] BookForUpdateDto book)
        {
            if (book == null)
            {
                return BadRequest();
            }

            if (!libraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var bookForAuthorFromRepo = libraryRepository.GetBookForAuthor(authorId, id);
            if (bookForAuthorFromRepo == null)
            {
                var bookEntity = Mapper.Map<Book>(book);

                libraryRepository.AddBookForAuthor(authorId, bookEntity);

                if (!libraryRepository.Save())
                {
                    throw new Exception($"creating a book for author {authorId} failed to save");
                }

                var book2Return = Mapper.Map<BookDto>(bookEntity);

                return CreatedAtRoute(GetBookForAuthorRoute, new { id = book2Return.Id }, book2Return);
            }

            Mapper.Map(book, bookForAuthorFromRepo);

            bookValidation.Validate(book, ModelState);

            TryValidateModel(book);
            
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            libraryRepository.UpdateBookForAuthor(bookForAuthorFromRepo);

            if (!libraryRepository.Save())
            {
                throw new Exception($"Updating book {id} for author {authorId} failed on save.");
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PatchBookForAuthor(Guid authorId, Guid id,
            [FromBody] JsonPatchDocument<BookForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!libraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var bookForAuthorFromRepo = libraryRepository.GetBookForAuthor(authorId, id);
            if (bookForAuthorFromRepo == null)
            {
                var bookDto = new BookForUpdateDto();

                patchDoc.ApplyTo(bookDto, ModelState);

                bookValidation.Validate(bookDto, ModelState);

                TryValidateModel(bookDto);

                if (!ModelState.IsValid)
                {
                    return new UnprocessableEntityObjectResult(ModelState);
                }

                var bookToAdd = Mapper.Map<Book>(bookDto);
                bookToAdd.Id = id;

                libraryRepository.AddBookForAuthor(authorId, bookToAdd);

                if (!libraryRepository.Save())
                {
                    throw new Exception($"creating a book for author {authorId} failed to save");
                }

                var book2Return = Mapper.Map<BookDto>(bookToAdd);

                return CreatedAtRoute(GetBookForAuthorRoute, new { id = book2Return.Id }, book2Return);
            }

            var bookToPatch = Mapper.Map<BookForUpdateDto>(bookForAuthorFromRepo);

            patchDoc.ApplyTo(bookToPatch, ModelState);

            bookValidation.Validate(bookToPatch, ModelState);

            TryValidateModel(bookToPatch);
            
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            Mapper.Map(bookToPatch, bookForAuthorFromRepo);

            libraryRepository.UpdateBookForAuthor(bookForAuthorFromRepo);

            if (!libraryRepository.Save())
            {
                throw new Exception($"Updating book {id} for author {authorId} failed on save.");
            }

            return NoContent();
        }
    }
}