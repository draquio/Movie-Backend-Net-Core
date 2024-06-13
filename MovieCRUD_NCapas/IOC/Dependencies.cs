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
            services.AddAutoMapper(cfg =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var mapperFunctions = serviceProvider.GetService<MapperFunctions>();

                cfg.AddProfile(new AutoMapperProfile(mapperFunctions));
            });


            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IActorRepository, ActorRepository>();
            services.AddScoped<IMovieActorRepository, MovieActorRepository>();


            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IActorService, ActorService>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IReviewService, ReviewService>();
        }
    }
}
