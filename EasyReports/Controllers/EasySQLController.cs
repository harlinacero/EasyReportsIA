using EaseyReportsApi.Dtos;
using EaseyReportsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyReports.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EasySQLController : Controller
    {
        private readonly IGenerateSQLService _service;

        public EasySQLController(IGenerateSQLService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> GetData(ApiRequest naturalQuery)
        {
            return Ok(await _service.GenerateSQLData(naturalQuery.NaturalQuery));
        }
        
    }


}