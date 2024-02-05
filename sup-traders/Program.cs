using sup_traders.Access;
using sup_traders.Business.Helpers;
using sup_traders.Business.Repositories;

namespace sup_traders
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<GameManager>();
            builder.Services.AddSingleton<ConnectionHelper>();
            builder.Services.AddScoped<IExchangeRepository, ExchangeRepository>();
            builder.Services.AddScoped<IExchangeAccesor, ExchangeAccesor>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
