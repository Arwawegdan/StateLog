WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args);

string connectionString = webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection");
webApplicationBuilder.Services.AddDbContext<ApplicationDbContext>(options =>
                                                    options.UseSqlServer(connectionString)
                                                           .EnableDetailedErrors()
                                                           .EnableSensitiveDataLogging()
                                                           .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
webApplicationBuilder.Services.AddControllers();
webApplicationBuilder.Services.AddEndpointsApiExplorer();

webApplicationBuilder.Services.AddScoped<INationalityCosmosDbRepository, NationalityCosmosDbRepository>();
webApplicationBuilder.Services.AddScoped<INationalityUnitOfWork, NationalityUnitOfWork>();      
webApplicationBuilder.Services.AddScoped<INationalityRepository, NationalityRepository>();


webApplicationBuilder.Services.AddScoped<ICurrencyCosmosDbRepository, CurrencyCosmosDbRepository>();
webApplicationBuilder.Services.AddScoped<ICurrencyUnitOfWork, CurrencyUnitOfWork>();
webApplicationBuilder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();

webApplicationBuilder.Services.AddScoped<IStateLogCustomTagsRepository, StateLogCustomTagsRepository>();
webApplicationBuilder.Services.AddScoped<IStateLogCustomTagsUnitOfWork, StateLogCustomTagsUnitOfWork>();

webApplicationBuilder.Services.AddScoped<INationalityReducerRepository, NationalityReducerRepository>();
webApplicationBuilder.Services.AddScoped<INationalityReducerUnitOfWork, NationalityReducerUnitOfWork>();



webApplicationBuilder.Services.AddSignalR();
webApplicationBuilder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
});


//await NationalityRepository.UpdateNationalities(T);


var app = webApplicationBuilder.Build();
app.UseResponseCompression();
if (app.Environment.IsDevelopment())
    app.UseWebAssemblyDebugging();
else
    app.UseHsts();

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();