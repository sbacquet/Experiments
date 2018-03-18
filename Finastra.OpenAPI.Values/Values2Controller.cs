using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Finastra.OpenAPI.Values.Model;

namespace Sbacquet.Controllers.V2
{
    [Route("api/values")]
    [ApiVersion("2.0")]
    public class ValuesController : Controller
    {
        ILogger _logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Seb2> Get()
        {
            _logger.LogInformation("Received a GET request");
            return new [] { new Seb2(1), new Seb2(2) };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Seb2 Get(int id)
        {
            _logger.LogInformation("Received a GET request (id = {id})", id);
            return new Seb2(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Seb2 value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
