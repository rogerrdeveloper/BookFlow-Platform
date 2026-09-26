using System.Text.Json;

namespace EmprestimoLibrary.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                //significa continue no caminho da requisição

                //se tudo der ok, segue normal
                await _next(context);
            }
            // mas se der throw new businessexceeption
            //ela vai nesse catch
            catch(BusinessException e)
            {
                //responde com status 409
                //e a mensagem
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = e.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }

            //esse catch é para erros inesperados
            catch (Exception)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = "Ocorreu um erro interno no servidor"
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
