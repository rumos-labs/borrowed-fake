namespace Borrowed.Fake.Models;

public class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = default!;

    public string? Author { get; set; }

    public int? PublishedYear { get; set; }

    public string? Genre { get; set; }

    public virtual Publisher? Publisher { get; set; } = default!;

    public virtual List<Rental>? Rentals { get; set; } = [];
}