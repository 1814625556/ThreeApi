using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using ThreeApi.Entities;
using ThreeApi.Models;
using ThreeApi.Services;

namespace ThreeApi.Controllers
{
    [ApiController]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly IOptions<StudentOptions> _studentOption;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompaniesController(
            IOptions<StudentOptions> studentOption,
            IMapper mapper,
            ICompanyRepository companyRepository)
        {
            _studentOption = studentOption;
            _companyRepository = companyRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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

        [HttpPost(Name = nameof(CreateCompany))]
        public async Task<IActionResult> CreateCompany(CompanyAddDto company)
        {
            var entity = _mapper.Map<Company>(company);
            _companyRepository.AddCompany(entity);
            await _companyRepository.SaveAsync();

            var returnDto = _mapper.Map<CompanyDto>(entity);

            //var links = CreateLinksForCompany(returnDto.Id, null);
            //var linkedDict = returnDto.ShapeData(null)
            //    as IDictionary<string, object>;

            //linkedDict.Add("links", links);

            return CreatedAtRoute(nameof(GetCompany), new { companyId = returnDto.Id },returnDto);
            //return Ok(returnDto);
        }
    }
}
