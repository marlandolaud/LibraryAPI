namespace Library.Contracts.Request.Book
{
    using System.ComponentModel.DataAnnotations;

    public class BookForUpdateDto: BookForManipulationDto
    {
        [Required]
        public override string Description { get => base.Description; set => base.Description = value; }
    }
}
