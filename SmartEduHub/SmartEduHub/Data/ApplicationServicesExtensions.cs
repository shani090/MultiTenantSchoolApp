using SmartEduHub.Interface;
using SmartEduHub.Repository;

namespace SmartEduHub.Data
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUser, AuthRepository>();
            services.AddScoped<IColleges, CollegesServices> ();
            return services;
        }
        
    }
}
