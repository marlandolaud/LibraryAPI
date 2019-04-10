﻿namespace Library.Contracts.Request.Author
{
    using Library.Contracts.Request.Book;
    using System;
    using System.Collections.Generic;

    public class AuthorForCreationDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTimeOffset DateOfBirth { get; set; }

        public string Genre { get; set; }

        public ICollection<BookForCreationDto> Books { get; set; }
            = new List<BookForCreationDto>();
    }
}
