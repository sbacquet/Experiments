using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Finastra.OpenAPI.Values.Model;

namespace Sbacquet.Controllers
{
    [Route("api/values")]
    [ApiVersion("1.0")]
    public class ValuesController : Controller
    {
        ILogger _logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }
        
        // GET api/values
        [HttpGet]
        public IEnumerable<Seb> Get()
        {
            using (_logger.BeginScope("GET"))
            {
                _logger.LogInformation("Received a request");
                return new[] { new Seb(1), new Seb(2) };
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Seb Get(int id)
        {
            using (_logger.BeginScope("GET"))
            {
                _logger.LogInformation("Received a request with id = {0}", id);
                return new Seb(id);
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Seb value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
    }
}
