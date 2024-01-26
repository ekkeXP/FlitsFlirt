using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Flitsflirt.App.Services
{
    public class HTTP : IHTTP
    {
        HttpClient _client;
        //URL van de Apache2 server
        private string ServerURL = "http://145.44.235.179";
        //Initialiseer de HTTPclient
        public HTTP()
        {
            _client = new HttpClient();
        }
        //Functie om te checken of een image al bestaat op de server
        public async Task<bool> ImageExists(int AccountID)
        {
            try
            {
                var response = await _client.GetAsync($"{ServerURL}/IMG/{AccountID}.jpg");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();

                    return content.Length > 0;
                }
                else
                {
                    return false;
                    Console.WriteLine($"Fout: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                return false;
                Console.WriteLine($"Fout: {ex.Message}");
            }
        }
        //Functie die de URL van een profielfoto returnt
        public string GetImage(int AccountID)
        {
            return ($"{ServerURL}/IMG/{AccountID}.jpg");
        }
        //Functie die een profielfoto upload naar de server
        public async Task SendPhotoToServer(string AccountID)
        {
            var file = await FilePicker.PickAsync();

            if (file != null)
            {
                var jpgFile = Path.ChangeExtension(AccountID, "jpg");
                var jpgFilePath = Path.Combine(FileSystem.CacheDirectory, jpgFile);

                using (var stream = await file.OpenReadAsync())
                {
                    using (var jpgStream = File.Create(jpgFilePath))
                    {
                        await stream.CopyToAsync(jpgStream);
                    }
                }
                
                var url = $"{ServerURL}/upload.php";

                using (var content = new MultipartFormDataContent())
                {
                    using (var fileStream = File.OpenRead(jpgFilePath))
                    {
                        var fileContent = new StreamContent(fileStream);
                        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "file",
                            FileName = jpgFile
                        };

                        content.Add(fileContent);

                        var response = await _client.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine(response.Content);
                        }
                        else
                        {
                            Console.WriteLine($"Foutcode: {response.StatusCode}");
                        }
                    }
                }
                File.Delete(jpgFilePath);
            }
        }

        //Functie die een foto van de server verwijdert
        public async Task DeletePhoto(string photoName)
        {
            try
            {
                string deleteUrl = $"{ServerURL}/IMG/{photoName}";
                HttpResponseMessage response = await _client.DeleteAsync(deleteUrl);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Foto succesvol verwijderd.");
                }
                else
                {
                    Console.WriteLine($"Fout bij het verwijderen van de foto. Statuscode: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Er is een fout opgetreden: {ex.Message}");
            }
        }
    
    }


}
