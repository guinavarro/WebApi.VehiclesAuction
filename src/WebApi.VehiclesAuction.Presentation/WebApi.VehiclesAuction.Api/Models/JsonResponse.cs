namespace WebApi.VehiclesAuction.Api.Models
{
    public class JsonResponse
    {
        public JsonResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
