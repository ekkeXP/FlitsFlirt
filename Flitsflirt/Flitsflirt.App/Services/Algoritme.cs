using Flitsflirt.App.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flitsflirt.App.Services
{
    internal class Algoritme : IAlgoritme
    {
        IDBConnect DB;
        IHTTP _client;
        int AgeDifference;
        int Age;
        int City;
        int OneHobbySame;
        int TwoHobbySame;
        int ThreeHobbySame;
        int Flitsed;
        int huidigeGebruikerID;


        public Algoritme(IDBConnect DB, IHTTP client)
        {
            this.DB = DB;
            this._client = client;
            getAlgorithmPoints();
        }

// Haalt huidige gebruiker op en return een gesorteerde lijst met gebruikers
        public List<Gebruiker> getPeople()
        {
            huidigeGebruikerID = Preferences.Default.Get("AccountID", -1);
            Gebruiker HuidigeGebruiker = new Gebruiker(0, "x", 0, "x", 'x', 'x', "x", "x", 0, 0, 0, "");
            HuidigeGebruiker = maakGebruiker(huidigeGebruikerID);

            List<int> randomPeopleIDList = new List<int>();
            randomPeopleIDList = getRandomPeople(HuidigeGebruiker);

            List<Gebruiker> randomPeopleGebruikersList = new List<Gebruiker>();
            randomPeopleGebruikersList = maakGebruikers(randomPeopleIDList);

            List<Gebruiker> sortedLijst = new List<Gebruiker>();
            sortedLijst = scoreSort(HuidigeGebruiker, randomPeopleGebruikersList);
            return sortedLijst;
        }

// Haalt 17 willekeurige gebruikerIDs op uit de database en returnt ze in een int list
        public List<int> getRandomPeople(Gebruiker g)
        {
            List<int> temp = new List<int>();
            if (g.Preference == 'X')
            {
                temp = DB.GetIntListFromTable($"SELECT TOP 17 AccountID FROM Account_Data WHERE AccountID != '{g.AccountID}' AND Preference = '{g.Gender}' ORDER BY newid()");
            }
            else
            {
                temp = DB.GetIntListFromTable($"SELECT TOP 17 AccountID FROM Account_Data WHERE (AccountID != '{g.AccountID}' AND Gender = '{g.Preference}' AND Preference = 'X') OR (AccountID != '{g.AccountID}' AND Gender = '{g.Preference}' AND Preference = '{g.Gender}') ORDER BY newid()");
            }

            return temp;
        }
        
// Roept voor elke gebruiker in de int list de maakGebruiker methode aan, verwijderd alle gebruikers die al geflitsed zijn
        public List<Gebruiker> maakGebruikers(List<int> x)
        {

            List<Gebruiker> templist1 = new List<Gebruiker>();
            for (int i = 0; i < x.Count; i++)
            {
                templist1.Add(maakGebruiker(x[i]));
            }

            for (int i = templist1.Count - 1; i >= 0; i--)
            {
                Task<string> algeflitsedT = DB.ReadQueryAsync("SELECT * FROM Flitsed WHERE AccountID = @Value1 AND FlitsedAccountID = @Value2", huidigeGebruikerID.ToString(), templist1[i].AccountID.ToString());
                string algeflitsedR = algeflitsedT.Result.ToString();
                if (algeflitsedR.Length > 0)
                {
                    templist1.RemoveAt(i);
                }
            }

            if (templist1.Count < 1)
            {
                Gebruiker placeholder = new Gebruiker(999, "Oeps!", 0, "x", 'X', 'X', "x", "Je hebt iedereen al geflitsed! Probeert het later nog eens.", 0, 0, 0, "flitsflirtlogo.svg");
                templist1.Add(placeholder);
            }
            return templist1;
        }

// Haalt alle informatie van een gebruikers ID uit de database op en stopt het in een gebruiker model
        public Gebruiker maakGebruiker(int ID)
        {

            Task<string> name = DB.ReadQueryAsync("SELECT FirstName FROM Account_Data WHERE AccountID = @Value1", ID.ToString());
            Task<string> age = DB.ReadQueryAsync("SELECT Age FROM Account_Data WHERE AccountID = @Value1", ID.ToString());
            Task<string> email = DB.ReadQueryAsync("SELECT Email FROM Account_Data WHERE AccountID = @Value1", ID.ToString());
            Task<string> gender = DB.ReadQueryAsync("SELECT Gender FROM Account_Data WHERE AccountID = @Value1", ID.ToString());
            Task<string> preference = DB.ReadQueryAsync("SELECT Preference FROM Account_Data WHERE AccountID = @Value1", ID.ToString());
            Task<string> city = DB.ReadQueryAsync("SELECT City FROM Account_Data WHERE AccountID = @Value1", ID.ToString());
            Task<string> description = DB.ReadQueryAsync("SELECT Description FROM Account_Data WHERE AccountID = @Value1", ID.ToString());
            Task<string> hobby1 = DB.ReadQueryAsync("SELECT HobbyID1 FROM Account_Hobbies WHERE AccountID = @Value1", ID.ToString());
            Task<string> hobby2 = DB.ReadQueryAsync("SELECT HobbyID2 FROM Account_Hobbies WHERE AccountID = @Value1", ID.ToString());
            Task<string> hobby3 = DB.ReadQueryAsync("SELECT HobbyID3 FROM Account_Hobbies WHERE AccountID = @Value1", ID.ToString());
            string nameR = name.Result.ToString();
            int ageR = int.Parse(age.Result);
            string emailR = email.Result.ToString();
            char genderR = gender.Result.FirstOrDefault();
            char preferenceR = preference.Result.FirstOrDefault();
            string cityR = city.Result.ToString();
            string descriptionR = description.Result.ToString();
            int hobby1R = int.Parse(hobby1.Result);
            int hobby2R = int.Parse(hobby2.Result);
            int hobby3R = int.Parse(hobby3.Result);
            string ProfilePic = _client.GetImage(ID);

            Gebruiker temp = new Gebruiker(ID, nameR, ageR, emailR, genderR, preferenceR, cityR, descriptionR, hobby1R, hobby2R, hobby3R, ProfilePic);
            return temp;
        }

// Bereken de score van een gebruiker
        public int getScore(Gebruiker g1, Gebruiker g2)
        {
            Task<string> FlitsedT = DB.ReadQueryAsync("SELECT * FROM Flitsed WHERE AccountID = @Value1 AND FlitsedAccountID = @Value2", g2.AccountID.ToString(), g1.AccountID.ToString());
            string flitsedr = FlitsedT.Result.ToString();
            int score = 0;
            int hobbycounter = 0;
            if ((Math.Max(g1.Age, g2.Age) - Math.Min(g1.Age, g1.Age)) <= AgeDifference) { score += Age; }
            if (g1.City.Equals(g2.City)) { score += City; }
            if (g1.Hobby1 == g2.Hobby1 || g1.Hobby1 == g2.Hobby2 || g1.Hobby1 == g2.Hobby3) { hobbycounter++; }
            if (g1.Hobby2 == g2.Hobby1 || g1.Hobby2 == g2.Hobby2 || g1.Hobby2 == g2.Hobby3) { hobbycounter++; }
            if (g1.Hobby3 == g2.Hobby1 || g1.Hobby3 == g2.Hobby2 || g1.Hobby3 == g2.Hobby3) { hobbycounter++; }
            if (hobbycounter == 1) { score += OneHobbySame; }
            if (hobbycounter == 2) { score += TwoHobbySame; }
            if (hobbycounter == 3) { score += ThreeHobbySame; }
            if (flitsedr.Length > 0) { score += Flitsed; }

            return score;


        }

// Sorteer de lijst van gebruikers op basis van score
        public List<Gebruiker> scoreSort(Gebruiker HuidigeGebruiker, List<Gebruiker> x)
        {

            for (int i = 0; i < x.Count; i++)
            {
                x[i].score = getScore(HuidigeGebruiker, x[i]);
            }
            x = x.OrderByDescending(x => x.score).ToList();
            return x;
        }

// Haal de punten instellingen van het dynamische algoritme op
        public void getAlgorithmPoints()
        {
            Task<string> AgeT = DB.ReadQueryAsync("SELECT Age FROM AlgorithmPoints");
            Task<string> AgeDifferenceT = DB.ReadQueryAsync("SELECT AgeDifference FROM AlgorithmPoints");
            Task<string> CityT = DB.ReadQueryAsync("SELECT City FROM AlgorithmPoints");
            Task<string> Hobby1T = DB.ReadQueryAsync("SELECT OneHobbySame FROM AlgorithmPoints");
            Task<string> Hobby2T = DB.ReadQueryAsync("SELECT TwoHobbySame FROM AlgorithmPoints");
            Task<string> Hobby3T = DB.ReadQueryAsync("SELECT ThreeHobbySame FROM AlgorithmPoints");
            Task<string> FlitsedT = DB.ReadQueryAsync("SELECT Flitsed FROM AlgorithmPoints");
            this.Age = int.Parse(AgeT.Result);
            this.AgeDifference = int.Parse(AgeDifferenceT.Result);
            this.City = int.Parse(CityT.Result);
            this.OneHobbySame = int.Parse(Hobby1T.Result);
            this.TwoHobbySame = int.Parse(Hobby2T.Result);
            this.ThreeHobbySame = int.Parse(Hobby3T.Result);
            this.Flitsed = int.Parse(FlitsedT.Result);

        }



        public string test()
        {
            Task<string> test = DB.ReadQueryAsync("SELECT * FROM AlgorithmPoints");
            string huts = test.Result;
            return huts;
        }


    }
}
