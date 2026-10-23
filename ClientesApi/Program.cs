using ClientesApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Security.Claims;
using System.Text;

//Construir la aplicacion(motor)
var builder = WebApplication.CreateBuilder(args);

//Registrar el DbContext
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//JWT - parte del archivo appsettings.json
var jwt = builder.Configuration.GetSection("Jwt");

//Configuracion de seguridad
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)),
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero //Evitar retraso en la expiración del token
        };
    });

//Otros Servicios
builder.Services.AddAuthorization(); //Primer servicio
builder.Services.AddControllers();
builder.Services.AddOpenApi();

//JWT Seguridad
builder.Services.AddOpenApi(options => {
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
        {
            ["Bearer"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Pega aquí el token JWT"
            }
        };

        document.Security =
        [
            new OpenApiSecurityRequirement
            {
                [ new OpenApiSecuritySchemeReference("Bearer", document) ] = new List<string>()
            }
        ];

        return Task.CompletedTask;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();  //1
app.UseAuthentication();    //2
app.UseAuthorization();     //3
app.MapControllers();       //4
app.Run();                  //5
