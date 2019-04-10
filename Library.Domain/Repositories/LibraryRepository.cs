namespace Library.Domain.Repositories
{
    using Library.Contracts.Response.Author;
    using Library.Domain.Entities;
    using Library.Domain.Helpers;
    using Library.Domain.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class LibraryRepository : ILibraryRepository
    {
        private LibraryContext _context;

        private readonly IPropertyMappingService propertyMappingService;

        public LibraryRepository(LibraryContext context, IPropertyMappingService propertyMappingService)
        {
            _context = context;
            this.propertyMappingService = propertyMappingService;
        }

        public void AddAuthor(Author author)
        {
            author.Id = Guid.NewGuid();
            _context.Authors.Add(author);

            // the repository fills the id (instead of using identity columns)
            if (author?.Books != null && author.Books.Any())
            {
                foreach (var book in author.Books)
                {
                    book.Id = Guid.NewGuid();
                }
            }
        }

        public void AddBookForAuthor(Guid authorId, Book book)
        {
            var author = GetAuthor(authorId);
            if (author != null)
            {
                // if there isn't an id filled out (ie: we're not upserting),
                // we should generate one
                if (book.Id == Guid.Empty)
                {
                    book.Id = Guid.NewGuid();
                }
                author.Books.Add(book);
            }
        }

        public bool AuthorExists(Guid authorId)
        {
            return _context.Authors.Any(a => a.Id == authorId);
        }

        public void DeleteAuthor(Author author)
        {
            _context.Authors.Remove(author);
        }

        public void DeleteBook(Book book)
        {
            _context.Books.Remove(book);
        }

        public Author GetAuthor(Guid authorId)
        {
            return _context.Authors.FirstOrDefault(a => a.Id == authorId);
        }

        public PagedList<Author> GetAuthors(IAuthorsRepositoryParameters parameters)
        {
            var collectionBeforePaging = _context.Authors.ApplySort(parameters.OrderBy, propertyMappingService.GetPropertyMapping<AuthorDto, Author>());

            if (!string.IsNullOrEmpty(parameters.Genre))
            {
                collectionBeforePaging = collectionBeforePaging
                    .Where(collection => collection.Genre.Equals(parameters.Genre.Trim(), StringComparison.InvariantCultureIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var searchLowerInvariant = parameters.Search.Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(search => search.Genre.ToLowerInvariant().Contains(searchLowerInvariant)
                    || search.FirstName.ToLowerInvariant().Contains(searchLowerInvariant)
                    || search.LastName.ToLowerInvariant().Contains(searchLowerInvariant));
            }

            return PagedList<Author>.Create(collectionBeforePaging, parameters.PageNumber, parameters.PageSize);
        }

        public IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds)
        {
            return _context.Authors.Where(a => authorIds.Contains(a.Id))
                .OrderBy(a => a.FirstName)
                .OrderBy(a => a.LastName)
                .ToList();
        }

        public void UpdateAuthor(Author author)
        {
            // no code in this implementation
            throw new NotImplementedException();
        }

        public Book GetBookForAuthor(Guid authorId, Guid bookId)
        {
            return _context.Books.Where(b => b.AuthorId == authorId && b.Id == bookId).FirstOrDefault();
        }

        public IEnumerable<Book> GetBooksForAuthor(Guid authorId)
        {
            return _context.Books.Where(b => b.AuthorId == authorId).OrderBy(b => b.Title).ToList();
        }

        public void UpdateBookForAuthor(Book book)
        {
            // no code in this implementation
            throw new NotImplementedException();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
