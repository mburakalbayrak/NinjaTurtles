using Microsoft.AspNetCore.Mvc;
using NinjaTurtles.Business.Abstract;

namespace NinjaTurtles.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParamController : Controller
    {
        private IParamService _paramService;

        public ParamController(IParamService paramService)
        {
            _paramService = paramService;
        }

        [HttpGet("GetParamList")]
        public IActionResult GetParamList(int id)
        {
            var list =_paramService.List(id);
            return Ok(list);
        }
    }
}
