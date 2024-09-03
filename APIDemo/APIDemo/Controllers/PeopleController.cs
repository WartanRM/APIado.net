using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using APIDemo.Data;
using APIDemo.Models;

namespace APIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly PeopleRepository _repository;

        public PeopleController(PeopleRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PeopleData>>> Get()
        {
            var people = await _repository.GetAllAsync();
            return Ok(people);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PeopleData>> Get(string id)
        {
            var person = await _repository.GetByIdAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }
        [HttpPost ]
        public async Task<ActionResult<PeopleData>> Post([FromBody] PeopleData person)
        {
            await _repository.AddAsync(person);
            return CreatedAtAction(nameof(Get), new { id = person.Id }, person);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] PeopleData person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }
            var updated = await _repository.UpdateAsync(person);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _repository.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
