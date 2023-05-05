using MvcApiTokenVideoJuegos.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace MvcApiTokenVideoJuegos.Services
{
    public class ServiceApiVideoJuegos
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApiVideojuegos;
        public ServiceApiVideoJuegos(IConfiguration configuration)
        {
            this.UrlApiVideojuegos =
                configuration.GetValue<string>("ApiUrls:ApiOAuthVideojuegos");
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");

        }

        public async Task<UsuarioGaming> GetPerfilUsuarioGaming(string token)
        {
            string request = "/api/videojuegos/perfilusuariogaming";
            UsuarioGaming usuario = 
                await this.CallApiAsync<UsuarioGaming>(request, token);
            return usuario;
        }

        //LO PRIMERO DE TODO ES RECUPERAR NUESTRO TOKEN LA APP MVC
        public async Task<string> GetTokenAsync(string username, string password)
        {
            using(HttpClient client = new HttpClient())
            {
                string request = "/api/auth/login";
                client.BaseAddress = new Uri(this.UrlApiVideojuegos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                LoginModel model = new LoginModel
                {
                    UserName= username,
                    Password = password
                };
                string jsonModel = JsonConvert.SerializeObject(model);
                StringContent content =
                    new StringContent(jsonModel, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
                if(response.IsSuccessStatusCode)
                {
                    string data =
                        await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(data);
                    string token =
                        jsonObject.GetValue("response").ToString();
                    return token;

                }
                else
                {
                    return null;
                }
               
            }
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using(HttpClient client = new HttpClient())
            {
                
                client.BaseAddress = new Uri(this.UrlApiVideojuegos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if(response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }
        private async Task<T> CallApiAsync<T>(string request, string token)
        {
            using(HttpClient client = new HttpClient())
            {
                
                client.BaseAddress = new Uri(this.UrlApiVideojuegos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if(response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<List<VideoJuego>> GetVideoJuegosAsync(string token)
        {
            string request = "/api/videojuegos";
            List<VideoJuego> videoJuegos =
                await this.CallApiAsync<List<VideoJuego>>(request, token);
            return videoJuegos;
        }

        public async Task<VideoJuego> FindVideoJuegoAsync(int idvideojuego)
        {
            string request = "/api/videojuegos/" + idvideojuego;
            VideoJuego videojuego =
                await this.CallApiAsync<VideoJuego>(request);
            return videojuego;
        }

        public async Task NewVideoJuegoAsync(int idvideojuego, string nombre, string descripcion, int precio, string imagen)
        {
            
            using(HttpClient client = new HttpClient())
            {
                string request = "/api/videojuegos";
                client.BaseAddress = new Uri(this.UrlApiVideojuegos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                VideoJuego videoJuego = new VideoJuego
                {
                    IdVideojuego = idvideojuego,
                    Nombre = nombre,
                    Descripcion = descripcion,
                    Precio = precio,
                    Imagen = imagen
                };
                string jsonVideoJuego =
                    JsonConvert.SerializeObject(videoJuego);
                StringContent content =
                    new StringContent(jsonVideoJuego, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
                
            }
        }
        public async Task UpdateVideoJuegoAsync(int idvideojuego, string nombre, string descripcion, int precio, string imagen)
        {
            
            using(HttpClient client = new HttpClient())
            {
                string request = "/api/videojuegos";
                client.BaseAddress = new Uri(this.UrlApiVideojuegos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                VideoJuego videoJuego = new VideoJuego
                {
                    IdVideojuego = idvideojuego,
                    Nombre = nombre,
                    Descripcion = descripcion,
                    Precio = precio,
                    Imagen = imagen
                };
                string jsonVideoJuego =
                    JsonConvert.SerializeObject(videoJuego);
                StringContent content =
                    new StringContent(jsonVideoJuego, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PutAsync(request, content);
                
            }
        }
        public async Task DeleteVideoJuegoAsync(int idvideojuego)
        {
            using(HttpClient client = new HttpClient())
            {
                string request = "/api/videojuegos/" +idvideojuego;
                client.BaseAddress = new Uri(this.UrlApiVideojuegos);
                client.DefaultRequestHeaders.Clear();
                HttpResponseMessage response =
                    await client.DeleteAsync(request);
            }
        }
    }
}
