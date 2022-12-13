using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using webAppHash.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace webAppHash.Controllers
{
    public class HomeController : Controller
    {

        private readonly UsuariosContext usuariosContext;

        public HomeController(UsuariosContext context)
        {
            usuariosContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Generar()
        {
            string? password = "Galileo2022*";

            // Generate a 128-bit salt using a sequence of
            // cryptographically strong random bytes.
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
            

            List<Usuarios> lista = usuariosContext.Usuarios.ToList();

            Usuarios user = usuariosContext.Usuarios.First(u => u.Name.Equals("bburgos"));

            byte[] saltDB = Convert.FromBase64String(lista[0].Salt);

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: saltDB,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            Console.WriteLine($"Hashed: {hashed}");

            if (!hashed.Equals(lista[0].Password))
            {
                Console.WriteLine("diferentes");
            }


            Usuarios usuario = new()
            {
                Name = "bburgos",
                Password = hashed,
                Salt = Convert.ToBase64String(salt)
            };

          
            usuariosContext.Add(usuario);
            usuariosContext.SaveChanges();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}