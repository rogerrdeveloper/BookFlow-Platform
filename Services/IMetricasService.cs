using EmprestimoLibrary.DTOs;

namespace EmprestimoLibrary.Services
{
    public interface IMetricasService
    {
        Task<MetricasResponseDto> ObterMetricasAsync(int idEditora);
    }
}
