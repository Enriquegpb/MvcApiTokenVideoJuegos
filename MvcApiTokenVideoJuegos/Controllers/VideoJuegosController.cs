using Microsoft.AspNetCore.Mvc;
using MvcApiTokenVideoJuegos.Filters;
using MvcApiTokenVideoJuegos.Models;
using MvcApiTokenVideoJuegos.Services;

namespace MvcApiTokenVideoJuegos.Controllers
{
    public class VideoJuegosController : Controller
    {
        private ServiceApiVideoJuegos service;
        public VideoJuegosController(ServiceApiVideoJuegos service)
        {
            this.service = service;
        }
        [AuthorizeUsuariosVideoJuegos]
        public async Task<IActionResult> Perfil()
        {
            string token =
                HttpContext.Session.GetString("token");
            UsuarioGaming usuario =
                await this.service.GetPerfilUsuarioGaming(token);
            return View(usuario);
        }

        [AuthorizeUsuariosVideoJuegos]
        public async Task<IActionResult> Index()
        {
            string token =
                 HttpContext.Session.GetString("token");
            if(token == null)
            {
                ViewData["MENSAJE"] = "Debe Inciar Sesion para la visualizacion de videojuegos";
                return View();
            }
            else
            {
                List<VideoJuego> videoJuegos =
                    await this.service.GetVideoJuegosAsync(token);
                return View(videoJuegos);
            }
            
        }

        public async Task<IActionResult> Details(int idvideojuego)
        {
            VideoJuego videoJuego =
                await this.service.FindVideoJuegoAsync(idvideojuego);
            return View(videoJuego);
        }

        public IActionResult NewVideoJuego()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewVideoJuego(int idvideojuego, string nombre, string descripcion, int precio, string imagen)
        {
            await this.service.NewVideoJuegoAsync(idvideojuego, nombre, descripcion, precio, imagen);
            return RedirectToAction("Index");
        } 
        public async Task <IActionResult> UpdateVideoJuego(int idvideojuego)
        {
            VideoJuego videojuego = await this.service.FindVideoJuegoAsync(idvideojuego);
            return View(videojuego);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateVideoJuego(int idvideojuego, string nombre, string descripcion, int precio, string imagen)
        {
            await this.service.UpdateVideoJuegoAsync(idvideojuego, nombre, descripcion, precio, imagen);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteVideoJuego(int idvideojuego)
        {
             await this.service.DeleteVideoJuegoAsync(idvideojuego);
            return RedirectToAction("Index");
        }
    }
}
