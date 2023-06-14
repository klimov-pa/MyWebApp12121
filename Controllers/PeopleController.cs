using Microsoft.AspNetCore.Mvc;
using webapi;

namespace webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeopleController : ControllerBase
{
    PeopleRepository _peopleRepository;
    ILogger<PeopleController> _logger;

    public PeopleController(ILogger<PeopleController> logger, PeopleRepository peopleRepository)
    {
        _logger = logger;
        _peopleRepository = peopleRepository;
    }

    [HttpGet(Name = "GetPeople")]
    public IActionResult GetPeople()
    {
        //return Ok(_peopleRepository.People);
        return Ok(Request.Headers.Accept);
    }

    [HttpPost(Name = "AddPerson")]
    public IActionResult Add([FromBody] Person person)
    {
        if (_peopleRepository.People.Count() > 0)
            person.Id = (from person1 in _peopleRepository.People select person1.Id).Max() + 1;
        else
            person.Id = 1;
        _peopleRepository.People.Add(person);
        return Ok();
    }

    [HttpPut(Name = "EditRange")]
    public IActionResult EditRange([FromBody] Person[] people)
    {
        Person[] peopleInRepo = new Person[people.Length];
        for (int index = 0; index < people.Length; ++index)
        {
            Person? foundPerson = _peopleRepository.People
                .FirstOrDefault(p => p.Id == people[index].Id);

            if (foundPerson is null)
                return NotFound(new {Error = $"person with id = {people[index].Id} is not found"});

            peopleInRepo[index] = foundPerson!;
        }

        for (int index = 0; index < people.Length; ++index)
        {
            var personInRepo = peopleInRepo[index];
            personInRepo.FirstName = people[index].FirstName;
            personInRepo.LastName = people[index].LastName;
        }

        return Ok();
    }

    [HttpDelete(Name = "DeletePeople")]
    public IActionResult Delete()
    {
        _peopleRepository.People.Clear();
        return Ok();
    }

    [HttpGet("{id}", Name = "GetPerson")]
    public IActionResult Get(int? id)
    {
        if (id is null)
            return NotFound();

        Person? personFromRepo = _peopleRepository.People.FirstOrDefault(p => p.Id == id);
        if (personFromRepo is null)
            return NotFound();

        return Ok(personFromRepo);
    }

    [HttpPut("{id}", Name = "EditPerson")]
    public IActionResult Edit(int? id, Person person)
    {
        if (id is null)
            return NotFound();

        Person? personFromRepo = _peopleRepository.People.FirstOrDefault(p => p.Id == id);
        if (personFromRepo is null)
            return NotFound();

        personFromRepo.FirstName = person.FirstName;
        personFromRepo.LastName = person.LastName;
        return Ok();
    }

    [HttpDelete("{id}", Name = "DeletePerson")]
    public IActionResult Delete(int? id)
    {
        if (id is null)
            return NotFound();

        Person? personFromRepo = _peopleRepository.People.FirstOrDefault(p => p.Id == id);
        if (personFromRepo is null)
            return NotFound();

        _peopleRepository.People.Remove(personFromRepo);
        return Ok();
    }
}