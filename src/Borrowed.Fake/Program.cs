using Bogus;
using Borrowed.Fake.Models;
using System.Net.Http.Json;

var endpoint = "http://localhost:5258/";

var publishers = GeneratePublishers();
var borrowers = GenerateBorrowers();
var rentals = GenerateRentals();
var books = GenerateBooks();

var httpClient = new HttpClient();

httpClient.BaseAddress = new Uri(endpoint);

foreach (var book in books)
{
  await httpClient.PostAsJsonAsync("/api/v1/books", book);
}

foreach (var borrower in borrowers)
{
  await httpClient.PostAsJsonAsync("/api/v1/borrowers", borrower);
}

// --------------------------------------------------------------------------------------------------------------------------------
// Methods
// --------------------------------------------------------------------------------------------------------------------------------
List<Publisher> GeneratePublishers(int number = 100)
{
  var publisherNames = GetPublisherNames();

  var publisherFaker = new Faker<Publisher>()
    .RuleFor(p => p.Name, f => f.PickRandom(publisherNames));

  return publisherFaker.Generate(number);
}

List<Borrower> GenerateBorrowers(int number = 100)
{
  var borrowerFaker = new Faker<Borrower>()
    .RuleFor(b => b.Name, f => f.Name.FullName());

  return borrowerFaker.Generate(number);
}

List<Rental> GenerateRentals(int number = 10000)
{
  var yesterday = DateTime.Today.AddDays(-1);

  var rentalFaker = new Faker<Rental>()
    .RuleFor(r => r.StartDate, f => f.Date.Past(5, yesterday))
    .RuleFor(r => r.EndDate, (f, r) => r.StartDate?.AddDays(7))
    .RuleFor(r => r.ReturnDate, (f, r) => r.StartDate?.AddDays(f.Random.Int(1, 10)))
    .RuleFor(r => r.Borrower, f => f.PickRandom(borrowers));

  return rentalFaker.Generate(number);
}

List<Book> GenerateBooks(int number = 100)
{
  var yesterday = DateTime.Today.AddDays(-1);
  var genres = GetGenres();

  var bookFaker = new Faker<Book>()
    .RuleFor(b => b.Title, f => f.Lorem.Sentence(2, 5).TrimEnd('.'))
    .RuleFor(b => b.Author, f => f.Name.FullName())
    .RuleFor(b => b.PublishedYear, f => f.Date.Past(50, yesterday).Year)
    .RuleFor(b => b.Genre, f => f.PickRandom(genres))
    .RuleFor(b => b.Publisher, f => f.PickRandom(publishers))
    .RuleFor(b => b.Rentals, f => f.PickRandom(rentals, f.Random.Int(0, 5)).ToList());

  return bookFaker.Generate(number);
}

string[] GetPublisherNames()
  => ["Penguin Random House", "HarperCollins", "Simon & Schuster", "Hachette Book Group", "Macmillan Publishers", "Scholastic", "McGraw-Hill Education", "Pearson", "Wiley", "Oxford University Press", "Cambridge University Press", "Elsevier", "SAGE Publications", "Springer Nature", "Taylor & Francis", "Cengage Learning", "Routledge", "Bloomsbury", "John Wiley & Sons", "Thomson Reuters", "W. W. Norton & Company", "Kodansha", "Shueisha", "Kadokawa Corporation", "Bonnier Books", "Bertelsmann", "Houghton Mifflin Harcourt", "Holtzbrinck Publishing Group", "Informa", "Zondervan", "Chronicle Books", "Quarto Group", "Abrams Books", "Algonquin Books", "Hay House", "Workman Publishing", "Grove Atlantic", "Harlequin Enterprises", "Dover Publications", "Peachpit Press", "Da Capo Press", "O'Reilly Media", "Packt Publishing", "Rowman & Littlefield", "Arcadia Publishing", "Chelsea Green Publishing", "Tyndale House", "VIZ Media", "Dark Horse Comics", "IDW Publishing"];

string[] GetGenres()
  => ["Literary Fiction", "Mystery", "Thriller", "Horror", "Historical Fiction", "Romance", "Western", "Dystopian", "Fantasy", "Science Fiction", "Speculative Fiction", "Magical Realism", "Realist Literature", "Biography", "Autobiography", "Memoir", "Essay", "History", "True Crime", "Self-help", "Health and Wellness", "Science", "Business and Economics", "Philosophy", "Travel", "Cookbooks", "Art and Photography", "Personal Development", "Picture Books", "Early Readers", "Chapter Books", "Middle Grade", "Young Adult (YA)", "Poetry", "Drama", "Anthology", "Comics and Graphic Novels"];
