
using WalletApp.API.Middlewares;

namespace WalletApp.API
{
    public static class ProgramPipelineApp
    {
        public static void ConfigurarPipeline(this WebApplication app) 
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            //----------> Uso de Middlewares
            app.UseMiddleware<ExceptionMiddleware>();

            //----------> Configuración de autenticación y autorización
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

        }
    }
}
