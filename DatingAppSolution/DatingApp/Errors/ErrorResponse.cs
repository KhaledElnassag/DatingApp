namespace DatingApp.Errors
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ErrorResponse(int statusCode, string? message="")
        {
            StatusCode = statusCode;
            if (string.IsNullOrEmpty(message))
                Message = statusCode switch
                {
                    400 => "A Bad Request, You have made!",
                    401=>"You Are Un Authorized!",
                    404=> "Not Found Resources!",
                    500=> "Errors are the path to success!",
                    _=>"Un Expected Error!"
                };
            else Message= message;
        }
    }
}
