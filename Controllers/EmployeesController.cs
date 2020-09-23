using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ThreeApi.DtoParameters;
using ThreeApi.Models;
using ThreeApi.Services;

namespace ThreeApi.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;

        public EmployeesController(IMapper mapper, ICompanyRepository companyRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        }

        //[HttpGet(Name = nameof(GetEmployeesForCompany))]
        //public async Task<ActionResult<IEnumerable<EmployeeDto>>>
        //    GetEmployeesForCompany(Guid companyId,
        //        [FromQuery] EmployeeDtoParameters parameters)
        //{
        //    if (!await _companyRepository.CompanyExistsAsync(companyId))
        //    {
        //        return NotFound();
        //    }

        //    var employees = await _companyRepository
        //        .GetEmployeesAsync(companyId, parameters);

        //    var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

        //    return Ok(employeeDtos);
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>>
            GetEmployeesForCompanyDemo1(Guid companyId,
                string Q)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employees = await _companyRepository
                .GetEmployeesAsync(companyId, new EmployeeDtoParameters() { Q = Q});

            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return Ok(employeeDtos);
        }
    }
}
