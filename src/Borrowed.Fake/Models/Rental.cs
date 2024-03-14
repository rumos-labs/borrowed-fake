namespace Borrowed.Fake.Models;

public class Rental
{
    public int Id { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public virtual Borrower? Borrower { get; set; } = default!;
}