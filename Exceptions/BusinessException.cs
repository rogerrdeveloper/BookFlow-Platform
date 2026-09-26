namespace EmprestimoLibrary.Exceptions
{
    public class BusinessException : Exception
    {
        //config erro especifico para regras de negocios
        public BusinessException(string message) : base(message)
        {
        }
    }
}
