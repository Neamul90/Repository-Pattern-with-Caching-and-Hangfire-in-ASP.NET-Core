using Caching_Hangfire.Interface;
using Caching_Hangfire.Model;
using Microsoft.AspNetCore.Mvc;

namespace Caching_Hangfire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _repository;
        public CustomerController(ICustomerRepository repository)
        {
            this._repository = repository;
        }
        [HttpGet]
        public async Task<IReadOnlyList<Customer>> Get()
        {
            return await _repository.GetAllAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Get(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }
            await _repository.UpdateAsync(customer);
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<Customer>> Post(Customer customer)
        {
            await _repository.AddAsync(customer);
            return CreatedAtAction("Get", new { id = customer.Id }, customer);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> Delete(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            await _repository.DeleteAsync(customer);
            return customer;
        }
    }
}
