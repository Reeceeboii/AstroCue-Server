namespace AstroCue.Server
{
    using System;
    using System.IO.Abstractions;
    using System.IO.Compression;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data;
    using Data.Interfaces;
    using Entities;
    using Hangfire;
    using Hangfire.SqlServer;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.ResponseCompression;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using Services;
    using Services.DevelopmentServices;
    using Services.Interfaces;
    using Utilities;

    /// <summary>
    /// ASP .NET Core startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initialies a new instance of the <see cref="Startup"/> class
        /// </summary>
        /// <param name="configuration">Instance of <see cref="IConfiguration"/></param>
        /// <param name="env">Instance of <see cref="IWebHostEnvironment"/></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.Configuration = configuration;
            this.Environment = env;
        }

        /// <summary>
        /// Instance of <see cref="IEnvironmentManager"/>
        /// </summary>
        private IEnvironmentManager _environmentManager;

        /// <summary>
        /// Gets or sets an instance of <see cref="IConfiguration"/>
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Instance of <see cref="IWebHostEnvironment"/>
        /// </summary>
        private IWebHostEnvironment Environment { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container
        /// </summary>
        /// <param name="services">Instance of <see cref="IServiceCollection"/></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Register database context with SQL Server
            // (this line was borrowed from my IN3046: Cloud Computing coursework)
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(this.Configuration.GetConnectionString("SQLServer"));
            });

            services.AddHangfire(conf => conf
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(this.Configuration.GetConnectionString("SQLServer"), new SqlServerStorageOptions()
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                })
            );

            services.AddHangfireServer();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AstroCue Server",
                    Contact = new OpenApiContact()
                    {
                        Email = "reece.mercer@city.ac.uk",
                        Name = "Reece Mercer"
                    },
                    Description = "Backend API for AstroCue - a platform for planning and logging astronomical" +
                                  " observations via computational astronomy, open data, and public APIs",
                    Version = "v1"
                });

                c.EnableAnnotations();
            });

            // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/?view=aspnetcore-5.0
            services.AddAuthentication(a =>
            {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(b =>
            {
                b.Events = new JwtBearerEvents
                {
                    OnTokenValidated = ctx =>
                    {
                        IAstroCueUserService astroCueUserService =
                            ctx.HttpContext.RequestServices.GetRequiredService<IAstroCueUserService>();

                        int astroCueUserId = Convert.ToInt32(ctx.Principal.Identity.Name);
                        AstroCueUser astroCueUser = astroCueUserService.RetrieveById(astroCueUserId);

                        // inject the current request's user ID into the HTTP context
                        ctx.HttpContext.Items[Constants.HttpContextReqUserId] = astroCueUser.Id;

                        return Task.CompletedTask;
                    }
                };

                b.SaveToken = true;
                b.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.Configuration["AppSettings:JsonWebTokenSecret"])),
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });

            services.AddResponseCompression(opt =>
            {
                opt.EnableForHttps = true;
                opt.Providers.Add<GzipCompressionProvider>();
            });

            services.Configure<GzipCompressionProviderOptions>(opt =>
            {
                opt.Level = CompressionLevel.Optimal;
            });

            services.AddSingleton(this.Configuration);
            this._environmentManager = new EnvironmentManager(this.Configuration);
            services.AddSingleton(this._environmentManager);

            services.AddSingleton<IFileSystem>(new FileSystem());

            MapperConfiguration mapperConfiguration = new(mc =>
            {
                // ISO8601
                mc.CreateMap<DateTime, string>().ConvertUsing(datetime => datetime.ToString("s"));
                mc.AddProfile(new AutoMapperProfile());
            });

            services.AddSingleton(mapperConfiguration.CreateMapper());

            // register development services
            if (this._environmentManager.RegisterDevServices)
            {
                services.AddScoped<IEmailService, DevEmailService>();
            }
            else
            {
                services.AddScoped<IEmailService, EmailService>();
            }

            services.AddScoped<IHttpClientService, HttpClientService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAstroCueUserService, AstroCueUserService>();
            services.AddScoped<IMappingService, MappingService>();
            services.AddScoped<ILightPollutionService, LightPollutionService>();
            services.AddScoped<IWeatherForecastService, WeatherForecastService>();
            services.AddScoped<IObservationLocationService, ObservationLocationService>();
            services.AddScoped<IObservationService, ObservationService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IObservationLogService, ObservationLogService>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        /// </summary>
        /// <param name="app">Instance of <see cref="IApplicationBuilder"/></param>
        /// <param name="backgroundJobs">Instance of <see cref="IBackgroundJobClient"/></param>
        /// <param name="env">Instance of <see cref="IWebHostEnvironment"/></param>
        public void Configure(
            IApplicationBuilder app, 
            IBackgroundJobClient backgroundJobs,
            IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            app.UseCors(cors =>
            {
                cors.AllowAnyOrigin();
                cors.AllowAnyMethod();
                cors.AllowAnyHeader();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AstroCue Server"));
            }

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            if (env.IsDevelopment())
            {
                app.UseHangfireDashboard(options: new DashboardOptions()
                {
                    Authorization = new []
                    {
                        new HangfireDashAuth()
                    }
                });
            }

            RecurringJob.AddOrUpdate<IReportService>(s => s.GenerateReports(default), "0 16 * * 1,4");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });

            // carry out database seeding if it is required
            DataSeeder.SeedAstronomicalCatalogues(app);
        }
    }
}
