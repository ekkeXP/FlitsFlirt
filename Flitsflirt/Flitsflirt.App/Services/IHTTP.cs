using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flitsflirt.App.Services
{
    public interface IHTTP
    {
        string GetImage(int AccountID);
        Task SendPhotoToServer(string AccountID);
        Task DeletePhoto(string photoName);
        Task<bool> ImageExists(int AccountID);
    }
}
