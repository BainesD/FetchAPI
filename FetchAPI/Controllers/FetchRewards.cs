using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FetchAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FetchRewards : ControllerBase
    {
        // GET: api/<FetchRewards>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<FetchRewards>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FetchRewards>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<FetchRewards>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FetchRewards>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
