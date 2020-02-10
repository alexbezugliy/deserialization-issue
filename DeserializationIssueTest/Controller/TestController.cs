using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DeserializationIssueTest.Controller
{
    [Route("endpoint")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class TestController
    {
        [HttpPost]
        public async Task<IActionResult> PostEndpoint([FromBody] Dto.Dto dto)
        {
            Console.Out.Write(dto.Name);
            return new OkResult();
        }
    }
}
