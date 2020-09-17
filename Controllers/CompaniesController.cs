using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace ThreeApi.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        [HttpGet(Name = "hellocc")]
        public string HelloCC()
        {
            return "hello cc";
        }

        //[HttpGet(Name = "helloccAsync")]
        //public async Task<string> HelloCCAsync()
        //{
        //    var result = await Task.Run(() => "hello cc async");
        //    return result;
        //}
    }
}
