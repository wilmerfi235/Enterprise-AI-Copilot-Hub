namespace EnterpriseAI.API.Extensions;

/// <summary>
/// 
/// Comprueba que el archivo esté creado en la carpeta Extensions del proyecto API y contenga 
/// el método estático AddApiServices.
/// 
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAngularClient", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        return services;
    }
}
