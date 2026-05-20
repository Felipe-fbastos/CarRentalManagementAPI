using CarLocationManagementAPI.Models;
using CarRentalManagementAPI.DTO.Car;
using CarRentalManagementAPI.DTO.Rental;
using CarRentalManagementAPI.Models;
using Mapster;

namespace CarRentalManagementAPI.Mapper
{
    public static class MappingConfiguration
    {
        public static IServiceCollection RegisterMaps(this IServiceCollection services)
        {

            services.AddMapster();

            // Faz o mapster aceitar valores nulos para fazer updates parciais
            TypeAdapterConfig<CarUpdateDTO, Car>
                .NewConfig()
                .IgnoreNullValues(true);

            
            TypeAdapterConfig<Rental, RentalGetReponseDTO>
                .NewConfig()
                .Map(destino => destino.NameClient, scr => scr.Client.Name)
                .Map(destino => destino.NameCar, scr => scr.Car.Name);

            return services;
        }
    }
}
