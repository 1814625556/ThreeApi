using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using ThreeApi.Entities;
using ThreeApi.Services;

namespace ThreeApi.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly IOptions<StudentOptions> _studentOption;
        private readonly ICompanyRepository _companyRepository;

        public CompaniesController(IOptions<StudentOptions> studentOption, ICompanyRepository companyRepository)
        {
            _studentOption = studentOption;
            _companyRepository = companyRepository;
        }
        
        /// <summary>
        /// 指定请求action
        /// </summary>
        /// <returns></returns>
        [HttpGet("hellocc")]
        public string HelloCC()
        {
            return $"hello cc{_studentOption.Value.Grades}";
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var result = await _companyRepository.GetCompaniesAsync();
            return Ok(result);
        }
        [HttpGet("{companyId}", Name = nameof(GetCompany))]
        public async Task<IActionResult> GetCompany(Guid companyId)
        {
            var company = await _companyRepository.GetCompanyAsync(companyId);
            if (company == null)
                return NotFound();
            return Ok(company);
        }
    }
}
