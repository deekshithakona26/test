namespace GetECINo.Models
{
    public class ErrorResponse
    {
        public ErrorResponse()
        {
        }

        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }
    }
}