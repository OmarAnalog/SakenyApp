namespace SakennyProject.Erorrs
{
    public class ApiResponse
    {
        public int? StatusCodee { get; set; }
        public string? Msg { get; set; }
        public ApiResponse(int? statusCodee, string? msg = null)
        {
            StatusCodee = statusCodee;
            Msg = msg ?? GetDefaultMessageForStatusCode(statusCodee);
        }

        private string? GetDefaultMessageForStatusCode(int? statusCodee)
        {
            return statusCodee switch
            {
                400 => "BadRequest",
                401 => "UnAuthorized",
                403 => "Forbidden",
                404 => "Not Found",
                500 => "internal Server Error",
                _ => null
            };
        }

    }
}