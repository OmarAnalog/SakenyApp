using System.Net;

namespace SakennyProject.Erorrs
{
    public class ApiExceptionResponse : ApiResponse
    {
        private HttpStatusCode internalServerError;
        private string message;
        public string? Details { get; set; }
        public ApiExceptionResponse(int StatusCode, string? Message = null, string? details = null) : base(StatusCode, Message)
        {
            Details = details;
        }
    }
}
