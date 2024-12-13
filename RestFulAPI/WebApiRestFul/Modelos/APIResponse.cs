using System.Net;

namespace WebApiRestFul.Modelos
{
    public class APIResponse
    {
        public HttpStatusCode statusCode { get; set; }

        public bool IsExistoso { get; set; } = true;

        public List<string> ErrorMesages { get; set; }

        public object Resultado { get; set; }
    }
}
