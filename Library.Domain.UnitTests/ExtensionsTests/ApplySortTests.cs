using Library.Contracts.Response.Author;
using Library.Domain.Entities;
using Library.Domain.Extensions;
using Library.Domain.Services;
using Library.Domain.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Library.Domain.UnitTests.Extensions
{
    public class ApplySortTests
    {
        private IPropertyMappingService PropertyMappingService { get; set; } = new PropertyMappingService();

        [Fact]
        public void ShouldGetAuthorsOrderedByNameDescending()
        {
            const string OrderBy = "name desc";

            var expectedResult = new Author()
            {
                FirstName = "Zzzzzzzzzzz",
                LastName = "Zzzzzzzzzzz"
            };

            var authors = LibraryTestHelper.GetAuthors();

            authors.Add(expectedResult);

            var sortedList = authors.AsQueryable().ApplySort(OrderBy, PropertyMappingService.GetPropertyMapping<AuthorDto, Author>());

            Assert.Equal(expectedResult.FirstName, sortedList.FirstOrDefault().FirstName);
        }

        [Fact]
        public void ShouldGetAuthorsOrderedByNameDescendingExtraSpace()
        {
            const string OrderBy = "name      desc";

            var expectedResult = new Author()
            {
                FirstName = "Zzzzzzzzzzz",
                LastName = "Zzzzzzzzzzz"
            };

            var authors = LibraryTestHelper.GetAuthors();

            authors.Add(expectedResult);

            var sortedList = authors.AsQueryable().ApplySort(OrderBy, PropertyMappingService.GetPropertyMapping<AuthorDto, Author>());

            Assert.Equal(expectedResult.FirstName, sortedList.FirstOrDefault().FirstName);
        }

        [Fact]
        public void ShouldGetAuthorsOrderedByNameDescending2()
        {
            const string OrderBy = "name desc";

            var expectedResult = new Author()
            {
                FirstName = "Zzzzzzzzzzz",
                LastName = "Zzzzzzzzzzz"
            };

            var expectedSecond = new Author()
            {
                FirstName = "Zzzzzzzzzzz",
                LastName = "Zack"
            };

            var authors = LibraryTestHelper.GetAuthors();

            authors.Add(expectedResult);
            authors.Add(expectedSecond);

            var sortedList = authors.AsQueryable().ApplySort(OrderBy, PropertyMappingService.GetPropertyMapping<AuthorDto, Author>());

            Assert.Equal(expectedResult.FirstName, sortedList.FirstOrDefault().FirstName);
            Assert.Equal(expectedSecond.FirstName, sortedList.ElementAt(1).FirstName);
        }

        [Fact]
        public void ShouldGetAuthorsOrderedByNameAscending()
        {
            const string OrderBy = "name ";

            var expectedResult = new Author()
            {
                FirstName = "Aaaaaaaaaaa",
                LastName = "Aaaaaaaaaaa"
            };

            var authors = LibraryTestHelper.GetAuthors();

            authors.Add(expectedResult);

            var sortedList = authors.AsQueryable().ApplySort(OrderBy, PropertyMappingService.GetPropertyMapping<AuthorDto, Author>());

            Assert.Equal(expectedResult.FirstName, sortedList.FirstOrDefault().FirstName);
        }

        [Fact]
        public void ShouldGetAuthorsOrderedByNameAscending2()
        {
            const string OrderBy = "name ";

            var expectedResult = new Author()
            {
                FirstName = "Aaaaaaaaaaa",
                LastName = "Aaaaaaaaaaa"
            };

            var expectedSecond = new Author()
            {
                FirstName = "Aaaaaaaaaaa",
                LastName = "Zack"
            };

            var authors = LibraryTestHelper.GetAuthors();

            authors.Add(expectedResult);
            authors.Add(expectedSecond);

            var sortedList = authors.AsQueryable().ApplySort(OrderBy, PropertyMappingService.GetPropertyMapping<AuthorDto, Author>());

            Assert.Equal(expectedResult.FirstName, sortedList.FirstOrDefault().FirstName);
            Assert.Equal(expectedSecond.FirstName, sortedList.ElementAt(1).FirstName);
        }

        [Fact]
        public void ShouldGetAuthorsOrderedByAgeAscending()
        {
            const string OrderBy = "age";

            var expectedResult = new Author()
            {
                DateOfBirth = new DateTimeOffset(DateTime.MinValue)
            };

            var authors = LibraryTestHelper.GetAuthors();

            authors.Add(expectedResult);

            var sortedList = authors.AsQueryable().ApplySort(OrderBy, PropertyMappingService.GetPropertyMapping<AuthorDto, Author>());

            Assert.Equal(expectedResult.DateOfBirth, sortedList.FirstOrDefault().DateOfBirth);
        }

        [Fact]
        public void ShouldGetAuthorsOrderedByAgeDescending()
        {
            const string OrderBy = "age desc";

            var expectedResult = new Author()
            {
                DateOfBirth = new DateTimeOffset(new DateTime(5022,1,1))
            };

            var authors = LibraryTestHelper.GetAuthors();

            authors.Add(expectedResult);

            var sortedList = authors.AsQueryable().ApplySort(OrderBy, PropertyMappingService.GetPropertyMapping<AuthorDto, Author>());

            Assert.Equal(expectedResult.DateOfBirth, sortedList.FirstOrDefault().DateOfBirth);
        }

        [Fact]
        public void ShouldGetAuthorsOrderedByGenreDescending()
        {
            const string OrderBy = "genre desc";

            var expectedResult = new Author()
            {
                Genre = "ZZZZZZZZZZZ"
            };

            var authors = LibraryTestHelper.GetAuthors();

            authors.Add(expectedResult);

            var sortedList = authors.AsQueryable().ApplySort(OrderBy, PropertyMappingService.GetPropertyMapping<AuthorDto, Author>());

            Assert.Equal(expectedResult.Genre, sortedList.FirstOrDefault().Genre);
        }

        [Fact]
        public void ShouldGetAuthorsOrderedByGenreAscending()
        {
            const string OrderBy = "genre ";

            var expectedResult = new Author()
            {
                Genre = "001AAAA"
            };

            var authors = LibraryTestHelper.GetAuthors();

            authors.Add(expectedResult);

            var sortedList = authors.AsQueryable().ApplySort(OrderBy, PropertyMappingService.GetPropertyMapping<AuthorDto, Author>());

            Assert.Equal(expectedResult.Genre, sortedList.FirstOrDefault().Genre);
        }

        [Fact]
        public void ShouldGetAuthorsOrderedByIdAscending()
        {
            const string OrderBy = "id ";

            var expectedResult = new Author()
            {
                Id = Guid.Empty
            };

            var authors = LibraryTestHelper.GetAuthors();

            authors.Add(expectedResult);

            var sortedList = authors.AsQueryable().ApplySort(OrderBy, PropertyMappingService.GetPropertyMapping<AuthorDto, Author>());

            Assert.Equal(expectedResult.Id, sortedList.FirstOrDefault().Id);
        }

        [Fact]
        public void ShouldGetAuthorsOrderedByNameAndAgeDescending()
        {
            const string OrderBy = "name desc, age desc";

            var expectedResult = new Author()
            {
                FirstName = "Zzzzzzzzzzz",
                LastName = "Zzzzzzzzzzz",
                DateOfBirth = new DateTimeOffset(new DateTime(5022, 1, 2))
            };

            var expectedSecond = new Author()
            {
                FirstName = "Zzzzzzzzzzz",
                LastName = "Zzzzzzzzzzz",
                DateOfBirth = new DateTimeOffset(new DateTime(5022, 1, 1))
            };

            var authors = LibraryTestHelper.GetAuthors();

            authors.Add(expectedResult);
            authors.Add(expectedSecond);

            var sortedList = authors.AsQueryable().ApplySort(OrderBy, PropertyMappingService.GetPropertyMapping<AuthorDto, Author>());

            Assert.Equal(expectedResult.FirstName, sortedList.FirstOrDefault().FirstName);
            Assert.Equal(expectedSecond.FirstName, sortedList.ElementAt(1).FirstName);
        }

        [Fact]
        public void InvalidPropertyThrowsException()
        {
            const string OrderBy = "madeuppropertyname ";

            var authors = LibraryTestHelper.GetAuthors();

            Assert.Throws<ArgumentException>(() => authors.AsQueryable().ApplySort(OrderBy, PropertyMappingService.GetPropertyMapping<AuthorDto, Author>()));
        }     

        [Fact]
        public void InvalidDelimiterThrowsException()
        {
            const string OrderBy = "name; ";

            var authors = LibraryTestHelper.GetAuthors();

            Assert.Throws<ArgumentException>(() => authors.AsQueryable().ApplySort(OrderBy, PropertyMappingService.GetPropertyMapping<AuthorDto, Author>()));
        }

        [Fact]
        public void NullMappingDictionaryThrowsException()
        {
            const string OrderBy = "name";

            var authors = LibraryTestHelper.GetAuthors();

            Assert.Throws<ArgumentNullException>(() => authors.AsQueryable().ApplySort(OrderBy, null));
        }

        [Fact]
        public void ShouldReturnValidIfFirsWordInKeyList()
        {
            const string OrderBy = "name ";

            var expectedResult = true;

            var result = PropertyMappingService.ValidMappingExistsForFields<AuthorDto, Author>(OrderBy);

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ShouldReturnValidIfFirsWordInKeyListWithDesc()
        {
            const string OrderBy = "name desc";

            var expectedResult = true;

            var result = PropertyMappingService.ValidMappingExistsForFields<AuthorDto, Author>(OrderBy);

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ShouldReturnValidIfFirsWordInKeyListEvenWhenNextWordInvalid()
        {
            const string OrderBy = "name barr ";

            var expectedResult = true;

            var result = PropertyMappingService.ValidMappingExistsForFields<AuthorDto, Author>(OrderBy);

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ShouldReturnInvalidIfPropertyNameNotInKeyList()
        {
            const string OrderBy = "madeuppropertyname ";

            var expectedResult = false;

            var result = PropertyMappingService.ValidMappingExistsForFields<AuthorDto, Author>(OrderBy);

            Assert.Equal(expectedResult, result);
        }       
    }
}
