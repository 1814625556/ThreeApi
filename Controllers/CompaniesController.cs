﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using ThreeApi.DtoParameters;
using ThreeApi.Entities;
using ThreeApi.Helpers;
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
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckerService _propertyCheckerService;

        public CompaniesController(
            ICompanyRepository companyRepository,
            IMapper mapper,
            IPropertyMappingService propertyMappingService,
            IPropertyCheckerService propertyCheckerService)
        {
            _companyRepository = companyRepository ??
                                 throw new ArgumentNullException(nameof(companyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
            _propertyCheckerService = propertyCheckerService ?? throw new ArgumentNullException(nameof(propertyCheckerService)); ;
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
        public async Task<IActionResult> GetCompanies([FromQuery] CompanyDtoParameters parameters)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<CompanyDto, Company>(parameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_propertyCheckerService.TypeHasProperties<CompanyDto>(parameters.Fields))
            {
                return BadRequest();
            }

            var companies = await _companyRepository.GetCompaniesAsync(parameters);

            var paginationMetadata = new
            {
                totalCount = companies.TotalCount,
                pageSize = companies.PageSize,
                currentPage = companies.CurrentPage,
                totalPages = companies.TotalPages
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata,
                new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                }));

            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            var shapedData = companyDtos.ShapeData(parameters.Fields);
            //var links = CreateLinksForCompany(parameters, companies.HasPrevious, companies.HasNext);

            // { value: [xxx], links }
            //var shapedCompaniesWithLinks = shapedData.Select(c =>
            //{
            //    var companyDict = c as IDictionary<string, object>;
            //    var companyLinks = CreateLinksForCompany((Guid)companyDict["Id"], null);
            //    companyDict.Add("links", companyLinks);
            //    return companyDict;
            //});

            //var linkedCollectionResource = new
            //{
            //    value = shapedCompaniesWithLinks,
            //    //links
            //};

            return Ok(shapedData);
            //var result = await _companyRepository.GetCompaniesAsync();
            //return Ok(result);
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

        //private IEnumerable<LinkDto> CreateLinksForCompany(Guid companyId, string fields)
        //{
        //    var links = new List<LinkDto>();

        //    if (string.IsNullOrWhiteSpace(fields))
        //    {
        //        links.Add(
        //            new LinkDto(Url.Link(nameof(GetCompany), new { companyId }),
        //                "self",
        //                "GET"));
        //    }
        //    else
        //    {
        //        links.Add(
        //            new LinkDto(Url.Link(nameof(GetCompany), new { companyId, fields }),
        //                "self",
        //                "GET"));
        //    }


        //    links.Add(
        //        new LinkDto(Url.Link(nameof(DeleteCompany), new { companyId }),
        //            "delete_company",
        //            "DELETE"));

        //    links.Add(
        //        new LinkDto(Url.Link(nameof(EmployeesController.CreateEmployeeForCompany), new { companyId }),
        //            "create_employee_for_company",
        //            "POST"));

        //    links.Add(
        //        new LinkDto(Url.Link(nameof(EmployeesController.GetEmployeesForCompany), new { companyId }),
        //            "employees",
        //            "GET"));

        //    return links;
        //}

        //private IEnumerable<LinkDto> CreateLinksForCompany(CompanyDtoParameters parameters, bool hasPrevious, bool hasNext)
        //{
        //    var links = new List<LinkDto>();


        //    links.Add(new LinkDto(CreateCompaniesResourceUri(parameters, ResourceUriType.CurrentPage),
        //        "self", "GET"));

        //    if (hasPrevious)
        //    {
        //        links.Add(new LinkDto(CreateCompaniesResourceUri(parameters, ResourceUriType.PreviousPage),
        //            "previous_page", "GET"));
        //    }

        //    if (hasNext)
        //    {
        //        links.Add(new LinkDto(CreateCompaniesResourceUri(parameters, ResourceUriType.NextPage),
        //            "next_page", "GET"));
        //    }

        //    return links;
        //}

    }
}
