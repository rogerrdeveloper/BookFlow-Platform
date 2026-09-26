using EmprestimoLibrary.DTOs;
using EmprestimoLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmprestimoLibrary.Controllers
{
    [Authorize(Roles = "Editora")]
    [Route("api/[controller]")]
    [ApiController]
    public class MetricasController : ControllerBase
    {
        private readonly IMetricasService _metricasService;

        public MetricasController(IMetricasService metricasService)
        {
            _metricasService = metricasService;
        }

        [HttpGet]
        public async Task<ActionResult<MetricasResponseDto>> ObterMetricas()
        {
            try
            {
                var idEditora = int.Parse(User.FindFirst("idEditora")!.Value);

                var metricas = await _metricasService.ObterMetricasAsync(idEditora);

                return Ok(metricas);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}