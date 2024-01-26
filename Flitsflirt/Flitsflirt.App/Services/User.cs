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
    public class User : IUser
    {
        IDBConnect DB;
        IHTTP _client;
        Gebruiker temp;

        public User(IDBConnect DB, IHTTP http)
        {
            this.DB = DB;
            _client = http;
        }

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

            temp = new Gebruiker(ID, nameR, ageR, emailR, genderR, preferenceR, cityR, descriptionR, hobby1R, hobby2R, hobby3R, ProfilePic);
            return temp;
        }

    }
}
