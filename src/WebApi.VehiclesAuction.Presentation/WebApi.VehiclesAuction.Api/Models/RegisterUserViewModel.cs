namespace WebApi.VehiclesAuction.Api.Models
{
    public class RegisterUserViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserType { get; set; }
        public string Cep { get; set; }
    }
}
