using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Mvc;
using MvcApiTokenVideoJuegos.Filters;
using MvcApiTokenVideoJuegos.Models;
using MvcApiTokenVideoJuegos.Services;

namespace MvcApiTokenVideoJuegos.Controllers
{
    public class VideoJuegosController : Controller
    {
        private ServiceApiVideoJuegos service;
        private ServiceStorageBlobs serviceStorageBlobs;
        private string containerName;
        public VideoJuegosController(ServiceApiVideoJuegos service, ServiceStorageBlobs serviceStorageBlobs, IConfiguration configuration)
        {
            this.service = service;
            this.serviceStorageBlobs = serviceStorageBlobs;
            this.containerName =
                 configuration.GetValue<string>("BlobContainers:VideoJuegosContainerName");

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
        public async Task<IActionResult> NewVideoJuego(int idvideojuego, string nombre, string descripcion, int precio, IFormFile file)
        {
            string blobName = file.FileName;
            using (Stream stream = file.OpenReadStream())
            {
                await this.serviceStorageBlobs.UploadBlobAsync(this.containerName, blobName, stream);
            }
            await this.service.NewVideoJuegoAsync(idvideojuego, nombre, descripcion, precio, await this.serviceStorageBlobs.GetBlobUriAsync(this.containerName, blobName));
            return RedirectToAction("Index");
        } 
        public async Task <IActionResult> UpdateVideoJuego(int idvideojuego)
        {
            VideoJuego videojuego = await this.service.FindVideoJuegoAsync(idvideojuego);
            return View(videojuego);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateVideoJuego(int idvideojuego, string nombre, string descripcion, int precio, IFormFile file)
        {
            string blobName = file.FileName;
            using (Stream stream = file.OpenReadStream())
            {
                await this.serviceStorageBlobs.UploadBlobAsync(this.containerName, blobName, stream);
            }
            await this.serviceStorageBlobs.DeleteContainerAsync(this.containerName);
            await this.service.NewVideoJuegoAsync(idvideojuego, nombre, descripcion, precio, await this.serviceStorageBlobs.GetBlobUriAsync(this.containerName, blobName));
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteVideoJuego(int idvideojuego)
        {
            VideoJuego videoJuego = await this.service.FindVideoJuegoAsync(idvideojuego);
            string blobName = Path.GetFileName(videoJuego.Imagen);
            await this.serviceStorageBlobs.DeleteBlobAsync(this.containerName, blobName);
            await this.service.DeleteVideoJuegoAsync(idvideojuego);
            return RedirectToAction("Index");
        }
       
    }
}
