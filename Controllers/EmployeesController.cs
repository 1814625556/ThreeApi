﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ThreeApi.DtoParameters;
using ThreeApi.Entities;
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

        //http://localhost:5000/api/companies/bbdee09c-089b-4d30-bece-44df5923716c/employees?Q=Nick
        [HttpGet(Name = nameof(GetEmployeesForCompany))]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>>
            GetEmployeesForCompany(Guid companyId,
                [FromQuery] EmployeeDtoParameters parameters)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employees = await _companyRepository
                .GetEmployeesAsync(companyId, parameters);

            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return Ok(employeeDtos);
        }

        [HttpGet("{employeeId}", Name = nameof(GetEmployeeForCompany))]
        //[ResponseCache(Duration = 60)]
        //[HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 1800)]
        //[HttpCacheValidation(MustRevalidate = true)]
        public async Task<ActionResult<EmployeeDto>>
            GetEmployeeForCompany(Guid companyId, Guid employeeId)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employee = await _companyRepository.GetEmployeeAsync(companyId, employeeId);
            if (employee == null)
            {
                return NotFound();
            }

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return Ok(employeeDto);
        }

        [HttpPost(Name = nameof(CreateEmployeeForCompany))]
        public async Task<ActionResult<EmployeeDto>>
            CreateEmployeeForCompany(Guid companyId, EmployeeAddDto employee)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var entity = _mapper.Map<Employee>(employee);

            _companyRepository.AddEmployee(companyId, entity);
            await _companyRepository.SaveAsync();

            var dtoToReturn = _mapper.Map<EmployeeDto>(entity);

            return CreatedAtRoute(nameof(GetEmployeeForCompany), new
            {
                companyId,
                employeeId = dtoToReturn.Id
            }, dtoToReturn);
        }

        /// <summary>
        /// 这里的Q 来自于 url 里面的 query
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="Q"></param>
        /// <returns></returns>
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<EmployeeDto>>>
        //    GetEmployeesForCompanyDemo1(Guid companyId,
        //        string Q)
        //{
        //    if (!await _companyRepository.CompanyExistsAsync(companyId))
        //    {
        //        return NotFound();
        //    }

        //    var employees = await _companyRepository
        //        .GetEmployeesAsync(companyId, new EmployeeDtoParameters() { Q = Q});

        //    var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

        //    return Ok(employeeDtos);
        //}
    }
}
