using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using webAppHash.Models;

namespace webAppHash.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuariosContext usuariosContext;

        public LoginController(UsuariosContext context)
        {
            usuariosContext = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(Usuarios entidad)
        {
            if (!ModelState.IsValid)
            {
                return View(entidad);
            }

            List<Usuarios> user = usuariosContext.Usuarios.Where(u => u.Name.Equals(entidad.Name)).ToList();

            if (!user.Any())
            {
                ModelState.AddModelError("Name", "El usuario ingresado no existe");
                return View(entidad);
            }

            byte[] saltDB = Convert.FromBase64String(user[0].Salt);

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: entidad.Password,
                salt: saltDB,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            if (!hashed.Equals(user[0].Password))
            {
                ModelState.AddModelError("Password", "Las credenciales ingresadas no son validas");
                return View(entidad);
            }

            return Redirect("Home/Index");
        }

        [HttpGet]
        public IActionResult Registrar() => View(new Usuarios());

        [HttpPost]
        public IActionResult Registrar(Usuarios entidad)
        {
            if (!ModelState.IsValid)
            {
                return View(entidad);
            }

            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: entidad.Password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            Usuarios usuario = new()
            {
                Name = entidad.Name,
                Password = hashed,
                Salt = Convert.ToBase64String(salt)
            };


            usuariosContext.Add(usuario);
            usuariosContext.SaveChanges();

            return Redirect("~/Home/Index");

        }

        [HttpGet]
        public IActionResult Logout() => Redirect("/Login/Login")
;

    }
}
