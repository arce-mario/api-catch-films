using ApiCatchFilms.Models;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiCatchFilms.Controllers
{
    public class LoginController : ApiController
    {

        public const string ADMIN_ROL = "Admin";
        public const string PUBLIC_ROL = "public";

        private ApiCatchFilmsContext db = new ApiCatchFilmsContext();
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> LoginAsync(LoginUser loginUser)
        {
            if (loginUser == null)
                return BadRequest("Usuario y Contraseña requeridos.");

            var userInfo = await AutenticarUsuarioAsync(loginUser.User, loginUser.Password);
            if (userInfo != null)
            {
                return Ok(new { token = GenerarTokenJWT(userInfo) });
            }
            else
            {
                return Unauthorized();
            }
        }
        private async Task<User> AutenticarUsuarioAsync(string userName, string password)
        {
            IQueryable<User> result = db.Users.Where(u => u.userName.Equals(userName) && u.pass.Equals(password));
            User user = await db.Users.FindAsync(result.First().userID);
            return user;
        }
        private string GenerarTokenJWT(User usuarioInfo)
        {
            IdentityModelEventSource.ShowPII = true;
            string rol = "";
            var secretKey = ConfigurationManager.AppSettings["secretKey"];
            var Issuer = ConfigurationManager.AppSettings["Issuer"];
            var Audience = ConfigurationManager.AppSettings["Audience"];
            int Expires;

            try{

                Expires = Int32.Parse(ConfigurationManager.AppSettings["Expires"].ToString());
            }
            catch(Exception e){
                Debug.WriteLine("Error al obtener el tiempo de expiración, "+e.Message);
                Expires = 0;
            }

            if (Expires > 0) Expires = 24;

            if (usuarioInfo.userID == 1)
            {
                rol = ADMIN_ROL;
            }
            else
            {
                rol = PUBLIC_ROL;
            }
            Debug.WriteLine(secretKey);
            var symmetricSecurityKey = new SymmetricSecurityKey(
                    Encoding.Default.GetBytes(secretKey));

            var signingCredentials = new SigningCredentials(
                symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature
            );

            var Header = new JwtHeader(signingCredentials);
            var Claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, usuarioInfo.userName),
                new Claim("lastName", usuarioInfo.firstName),
                new Claim("firstName", usuarioInfo.lastName),
                new Claim("rolID", usuarioInfo.rol.ToString()),
                new Claim("userID", usuarioInfo.userID.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuarioInfo.email),
                new Claim(ClaimTypes.Role, rol)
            };
            var Payload = new JwtPayload(
                    issuer: Issuer,
                    audience: Audience,
                    claims: Claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddHours(Expires)
                );

            var Token = new JwtSecurityToken(
                Header,
                Payload
            );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
