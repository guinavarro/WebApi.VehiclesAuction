using System.Text;

namespace WebApi.VehiclesAuction.Domain.Models
{
    public sealed class Response<T>
    {
        public Response(bool success, T data)
        {
            Success = success;
            Object = data;
        }
        public Response(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public Response(bool success, List<string> errors)
        {
            Success = success;
            Errors = errors;
        }
        public Response(bool success, string message, List<string> errors)
        {
            Success = success;
            Message = message;
            Errors = errors;
        }

        public Response(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Object = data;
        }

        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Object { get; set; }
        public List<string>? Errors { get; set; }
        public string GetErrorMessage()
        {
            return Errors.FirstOrDefault();
        }
        public string GetAllErrorsMessage()
        {
            var errorMessage = new StringBuilder();

            if (Errors != null && Errors.Any())
            {
                foreach (var error in Errors)
                {
                    errorMessage.Append($" {error.ToString()}");
                }
            }

            return errorMessage.ToString(); 
        }
    }
}
