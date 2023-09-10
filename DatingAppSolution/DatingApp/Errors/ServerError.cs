namespace DatingApp.Errors
{
    public class ServerError:ErrorResponse
    {
        public string Detail { get; set; }
        public ServerError():base(500)
        {

        }
    }
}
