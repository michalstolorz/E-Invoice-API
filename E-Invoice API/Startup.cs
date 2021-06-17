using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AutoMapper;
using E_Invoice_API.Common.Models;
using E_Invoice_API.Core.Helper;
using E_Invoice_API.Core.Interfaces.Services;
using E_Invoice_API.Core.MappingConfiguration;
using E_Invoice_API.Core.Repositories;
using E_Invoice_API.Core.Services;
using E_Invoice_API.Data;
using E_Invoice_API.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NetCore.AutoRegisterDi;

namespace E_Invoice_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterAssemblyPublicNonGenericClasses(typeof(InvoiceStatusService).Assembly).Where(x => x.Name.EndsWith("Service")).AsPublicImplementedInterfaces();
            services.RegisterAssemblyPublicNonGenericClasses(typeof(InvoiceStatusRepository).Assembly).Where(x => x.Name.EndsWith("Repository")).AsPublicImplementedInterfaces();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            var mappingConfiguration = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Configuration());
            });

            IMapper mapper = mappingConfiguration.CreateMapper();
            services.AddSingleton(mapper);
            services.AddHttpContextAccessor();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IUserContextProvider, UserContextProvider>();
            services.AddScoped<IChangeInvoiceStatusHelper, ChangeInvoiceStatusHelper>();
            services.AddTransient<IMailService, MailService>();
            services.Configure<DataProtectionTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromHours(3));
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

            services.AddControllers();

            IdentityBuilder builder = services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequiredLength = 3;
            }); //only for this API and development
            builder = new IdentityBuilder(builder.UserType, builder.Services);
            builder.AddEntityFrameworkStores<ApplicationDbContext>();
            builder.AddSignInManager<SignInManager<User>>();
            builder.AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
                {
                    var publicAuthorizationKey = Configuration.GetSection("AppSettings:PublicKey").Value;
                    var key = TokenHelper.BuildRsaSigningKey(publicAuthorizationKey);
                    options.TokenValidationParameters = TokenHelper.GetTokenValidationParameters(key);
                })
                .AddCookie(IdentityConstants.ApplicationScheme, options =>
                {
                    options.SlidingExpiration = true;
                })
                .AddCookie(IdentityConstants.TwoFactorUserIdScheme, o =>
                {
                    o.Cookie.Name = IdentityConstants.TwoFactorUserIdScheme;
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                })
                .AddExternalCookie();

            services.AddMvc();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "Swagger",
                        Description = "Swagger for my program",
                        Version = "v1"
                    });

                var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
                options.IncludeXmlComments(filePath);

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.SeedDb(true);
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
