namespace webapi;

public class PeopleRepository
{
    public List<Person> People = new List<Person>(new [] {
        new Person { Id = 1, FirstName = "John", LastName = "Wick" },
        new Person { Id = 2, FirstName = "Sarah", LastName = "Connor" },
    });
}