using CarRentalManagementAPI.Models;
using CarRentalManagementAPI.Models.Enum;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace CarLocationManagementAPI.Models
{
    
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string CPF { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Password { get; set; }
        public TypeUser TypeUser { get; set; }
        [JsonIgnore]
        public List<Rental> Rentals { get; set; } = new List<Rental>();

        public bool IsDelete { get; private set; }

        public void Delete()
        {
            IsDelete = true;
        }
    }
}
