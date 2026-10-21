using ClientesApi.Data;
using ClientesApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
namespace ClientesApi.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context; // Acceso BD
        private readonly IConfiguration _config; // Acceso a configuración
        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        //Registrar
        //POST: api/auth/registrar
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] RegistroRequest request)
        {
            // Si el usuario ya existe, cancelamos el proceso
            if (await _context.Usuarios.AnyAsync(u => u.Username == request.Username))
            {
                //Server code 409: Conflict
                return Conflict(new { message = "El usuario ya está registrado" });
            }
            // Si no existe, creamos uno nuevo utilziando AuthDtos
            var usuario = new Usuario
            {
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Rol = "Usuario"
            };
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            //Server code 200: OK
            return Ok(new { message = "Usuario registrado correctamente" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(U => U.Username == request.Username);

            //¿La contraseña se desencripta?
            //NO, se utiliza un algoritmo matematico para verificar su autenticidad, en este caso BCrypt.Net.BCrypt.Verify
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
            {
                //Server code 401: Unauthorized
                return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos" });
            }

            //var => declaración dinámica objetos
            //Deserealización
            var (token, expira) = GenerarToken(usuario); 
            return Ok(new { token, expira });
        }

        //El metodo retorna:
        //token: string, el token JWT generado
        //fecha hora: DateTime, la fecha y hora de expiración del token
        private (string token, DateTime expira) GenerarToken(Usuario usuario)
        {
            //Json Web Token
            var jwt = _config.GetSection("Jwt");

            //Nota: Los paámetros [Param] vienen del archivo appsettings.json (JSON)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var expira = DateTime.UtcNow.AddMinutes(double.Parse(jwt["ExpireMinutes"]!));
            
            //Configuración del algoritmo que emite el certificado y la firma de seguridad
            //algoritmos criptográficos
            var descriptor = new SecurityTokenDescriptor
            {
                //IDusuario, Nombre, Rol, Permiso
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Username),
                    new Claim(ClaimTypes.Role, usuario.Rol),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Issuer = jwt["Issuer"],
                Audience = jwt["Audience"],
                Expires = expira,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            

            var token = new JsonWebTokenHandler().CreateToken(descriptor);
            return (token, expira);
        }
        
    }
}
