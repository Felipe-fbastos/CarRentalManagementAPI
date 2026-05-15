using CarLocationManagementAPI.Models;
using CarRentalManagementAPI.DTO.Car;
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

            return services;
        }
    }
}
