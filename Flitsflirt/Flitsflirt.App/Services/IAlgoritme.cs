using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flitsflirt.App.Models;

namespace Flitsflirt.App.Services
{
    public interface IAlgoritme
    {
        List<Gebruiker> getPeople();
        List<int> getRandomPeople(Gebruiker g);
        List<Gebruiker> maakGebruikers(List<int> x);
        public Gebruiker maakGebruiker(int ID);
        int getScore(Gebruiker g1, Gebruiker g2);
        public List<Gebruiker> scoreSort(Gebruiker HuidigeGebruiker, List<Gebruiker> x);
        public void getAlgorithmPoints();
        public string test();
    }
}
