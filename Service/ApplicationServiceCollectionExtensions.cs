using Microsoft.Extensions.DependencyInjection;
using Service.Service;

namespace Service;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddBookingApplication(this IServiceCollection services)
    {
        services.AddSingleton<IBookingService, BookingService>();
        return services;
    } 
}