
using Microsoft.Identity.Client;
using System;


namespace Flitsflirt.App.Models
{
    public class Gebruiker
    {

        public Gebruiker() {
            this.AccountID = 0;
            this.FirstName = "Swipe 2x om te beginnen";
            this.Age = -1;
            this.Email = "";
            this.Gender = 'M';
            this.Preference = 'V';
            this.City = "";
            this.Description = "";
            this.Hobby1 = 1;
            this.Hobby2 = 2;
            this.Hobby3 = 3;
            this.score = 0;
            this.ProfilePic = "http://145.44.235.179/IMG/0.jpg";
        }  

        public Gebruiker(int accountid, string firstname, int age, string email, char gender, char preference, string city, string description, int hobby1, int hobby2, int hobby3, string profilepicture)
        {

            this.AccountID = accountid;
            this.FirstName = firstname;
            this.Age = age;
            this.Email = email;
            this.Gender = gender;
            this.Preference = preference;
            this.City = city;
            this.Description = description;
            this.Hobby1 = hobby1;
            this.Hobby2 = hobby2;
            this.Hobby3 = hobby3;
            this.score = 0;
            this.ProfilePic = profilepicture;
        }

        public Gebruiker(int accountid, string firstname, int age, string email, char gender, char preference, string city, string description, int hobby1, int hobby2, int hobby3, string profilepicture, int score)
        {

            this.AccountID = accountid;
            this.FirstName = firstname;
            this.Age = age;
            this.Email = email;
            this.Gender = gender;
            this.Preference = preference;
            this.City = city;
            this.Description = description;
            this.Hobby1 = hobby1;
            this.Hobby2 = hobby2;
            this.Hobby3 = hobby3;
            this.ProfilePic = profilepicture;
            this.score = score;
        }


        public int AccountID { get; set; }
        public string FirstName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public char Gender { get; set; }
        public char Preference { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public int Hobby1 { get; set; }
        public int Hobby2 { get; set; }
        public int Hobby3 { get; set; }
        public int score { get; set; }
        public string ProfilePic {  get; set; }
    }
}