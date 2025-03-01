﻿using Microsoft.AspNetCore.Mvc;
using NinjaTurtles.Business.Abstract;
using NinjaTurtles.Core.Helpers.QrCode;
using NinjaTurtles.Core.NetCoreConfiguration;
using NinjaTurtles.Entities.Config;
using NinjaTurtles.Entities.Dtos;
using System.Drawing.Imaging;

namespace NinjaTurtles.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private ICompanyOrderService _companyOrderService;

        public CompanyController(ICompanyOrderService companyOrderService)
        {
            _companyOrderService = companyOrderService;
        }

        [HttpGet("getList")]
        public IActionResult GetList()
        {
            var result = _companyOrderService.GetList();
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult Add([FromBody] string name)
        {
            var result =_companyOrderService.Add(name);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public IActionResult AddCompanyOrder([FromBody] AddCompanyOrderDetailDto dto)
        {
            var currentdirectory = Directory.GetCurrentDirectory();
            var result = _companyOrderService.AddDetail(dto,currentdirectory);
            return Ok(result);
        }

      
    }
}
