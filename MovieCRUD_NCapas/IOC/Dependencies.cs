using Microsoft.EntityFrameworkCore;
using MovieCRUD_NCapas.DBContext;
using MovieCRUD_NCapas.Repository;
using MovieCRUD_NCapas.Repository.Interface;
using MovieCRUD_NCapas.Services;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;


namespace MovieCRUD_NCapas.IOC
{
    public static class Dependencies
    {
        public static void InjectDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DBMovieContext>(options => { 
                options.UseSqlServer(configuration.GetConnectionString("Connection")); 
            });
            services.AddSingleton<MapperFunctions>();
            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IActorService, ActorService>();
            services.AddScoped<IMovieService, MovieService>();
        }
    }
}
