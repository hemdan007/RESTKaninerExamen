using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RestKaniner.Models;

var builder = WebApplication.CreateBuilder(args);

//JWT 1
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

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
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();


//CORS 1 Cross Origin Resource Sharing
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin() 
                  .AllowAnyMethod()  
                  .AllowAnyHeader();
        });
});

// Add services to the container.
builder.Services.AddControllers();

//D.I
builder.Services.AddSingleton<KaninRepository>(new KaninRepository(true));

//swagger 1
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

//swagger 2
app.UseSwagger(); // opretter en beskrivelse af alle API-endpoints.
app.UseSwaggerUI(); //giver lov til at teste mine endpoints direkte i browseren


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//CORS 2
app.UseCors("AllowAll");

//JWT 2
app.UseAuthentication(); // Checks "Who are you?"
app.UseAuthorization();  // Checks "Are you allowed to be here?"

app.MapControllers();

app.Run();
