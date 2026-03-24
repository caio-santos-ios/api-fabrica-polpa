using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PulpaAPI.src.Handlers;
using PulpaAPI.src.Interfaces;
using PulpaAPI.src.Repository;
using PulpaAPI.src.Services;

namespace PulpaAPI.src.Configuration
{
    public static class Build
    {
        public static void AddBuilderConfiguration(this WebApplicationBuilder builder)
        {
            AppDbContext.ConnectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? "mongodb://localhost:27017";
            AppDbContext.DatabaseName = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "pulpa_db";
            bool IsSSL;
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("IS_SSL")))
                IsSSL = Convert.ToBoolean(Environment.GetEnvironmentVariable("IS_SSL"));
            else
                IsSSL = false;
            AppDbContext.IsSSL = IsSSL;
        }

        public static void AddBuilderAuthentication(this WebApplicationBuilder builder)
        {
            string? SecretKey = Environment.GetEnvironmentVariable("SECRET_KEY") ?? "pulpa_secret_key_2024_very_secure";
            string? Issuer = Environment.GetEnvironmentVariable("ISSUER") ?? "pulpa-api";
            string? Audience = Environment.GetEnvironmentVariable("AUDIENCE") ?? "pulpa-front";

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Issuer,
                    ValidAudience = Audience,
                    ClockSkew = TimeSpan.FromMinutes(5),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey))
                };
            });
        }

        public static void AddContext(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<AppDbContext>();
        }

        public static void AddBuilderServices(this WebApplicationBuilder builder)
        {
            // AUTH
            builder.Services.AddTransient<IAuthService, AuthService>();

            // MASTER DATA
            builder.Services.AddTransient<IProductService, ProductService>();
            builder.Services.AddTransient<IProductRepository, ProductRepository>();

            builder.Services.AddTransient<ICustomerService, CustomerService>();
            builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();

            builder.Services.AddTransient<ISupplierService, SupplierService>();
            builder.Services.AddTransient<ISupplierRepository, SupplierRepository>();

            // ESTOQUE
            builder.Services.AddTransient<IStockService, StockService>();
            builder.Services.AddTransient<IStockRepository, StockRepository>();

            // VENDAS
            builder.Services.AddTransient<ISaleService, SaleService>();
            builder.Services.AddTransient<ISaleRepository, SaleRepository>();

            // RELATÓRIOS
            builder.Services.AddTransient<IReportService, ReportService>();

            // HANDLERS
            builder.Services.AddTransient<MailHandler>();
            builder.Services.AddTransient<UploadHandler>();

            // builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
