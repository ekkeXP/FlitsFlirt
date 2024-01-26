using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flitsflirt.App.Models;


namespace Flitsflirt.App.Services
{
    public interface IUser
    {
        public Gebruiker maakGebruiker(int ID);
    }
}
