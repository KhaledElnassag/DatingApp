namespace DatingApp.Errors
{
   
    public class ApiValidationErrors : ErrorResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ApiValidationErrors() : base(400)
        {
            Errors = new List<string>();
        }
    }
}
