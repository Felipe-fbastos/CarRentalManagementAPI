using CarRentalManagementAPI.Models.Enum;

namespace CarRentalManagementAPI.DTO.Client
{
    public class ClientSignUpDTO
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string CPF { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Password { get; set; }
        public TypeUser TypeUser { get; set; }
    }
}
