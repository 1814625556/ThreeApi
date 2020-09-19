using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using ThreeApi.Entities;

namespace ThreeApi.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private IOptions<StudentOptions> _studentOption { get; }
        public CompaniesController(IOptions<StudentOptions> studentOption)
        {
            _studentOption = studentOption;
        }

        [HttpGet("hellocc")]
        public string HelloCC()
        {
            return $"hello cc{_studentOption.Value.Grades}";
        }
    }
}
