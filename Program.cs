using dotnet_rpg.Services.CharacterService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
.AddJsonOptions(options => 
                options.JsonSerializerOptions.Converters.Add(
                new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddOpenApi();

builder.Services.AddScoped<ICharacterService, CharacterService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();