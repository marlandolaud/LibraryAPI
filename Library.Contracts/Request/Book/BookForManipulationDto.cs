namespace Library.Contracts.Request.Book
{
    using System.ComponentModel.DataAnnotations;

    public abstract class BookForManipulationDto
    {
        [Required(ErrorMessage = "you should include a title")]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public virtual string Description { get; set; }
    }
}