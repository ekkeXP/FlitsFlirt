using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Flitsflirt.App.Models;
using Flitsflirt.App.Services;
using Flitsflirt.App.Views;

namespace Flitsflirt.App.ViewModels
{
    public partial class FlitsViewModel : ObservableObject
    {
        private IDBConnect DB;
        private IAlgoritme algor;
        private IHTTP _client;

        private int User;

        private int AccountID = Preferences.Default.Get("AccountID", -1);

        private bool FirstSwipe = true;

        [ObservableProperty]
        private List<Gebruiker> gebruikers = new List<Gebruiker>(); //lijst met gebruikers

        private List<string> Hobbies;

        //huidige gebruiker
        [ObservableProperty]
        private Gebruiker? currentGebruiker;
        
        //titel
        [ObservableProperty]
        private string? title = "Flits";

        //voortgang van de progressbar. 1 = vol
        [ObservableProperty]
        private double progress = 1;

        #region LoadProperties
        //region voor het laden van een flits
        
        [ObservableProperty]
        string? pic;

        [ObservableProperty]
        public string firstName = "";

        [ObservableProperty]
        public string firstnameAndAge = "";

        [ObservableProperty]
        public int age;

        [ObservableProperty]
        public string city = "";

        [ObservableProperty]
        public string description = "";

        [ObservableProperty]
        public double descriptionBackground;

        [ObservableProperty]
        public int hobbyHide;

        [ObservableProperty]
        public string hobby1 = "";

        [ObservableProperty]
        public string hobby2 = "";

        [ObservableProperty]
        public string hobby3 = "";

        #endregion

    	//constructor maakt verbiningen me db, algoritme en http en zet de rest van de flitspagina klaar
        public FlitsViewModel(IDBConnect DB, IAlgoritme algor, IHTTP client)
        {
            this.DB = DB;
            this.algor = algor;
            _client = client;
            FillGebruikers();
            Hobbies = this.DB.GetHobbies();
            UpdateProperties();
            CheckFoto();
        }

        private async void CheckFoto()
        {
        //stuurt een gebruiker terug als hen geen profielfoto hebben
            if(!await _client.ImageExists(AccountID))
            {
                await Shell.Current.GoToAsync($"//{nameof(EditHobbiesPage)}");
                DB.MakeToast("Upload een foto als profielfoto");
            }
        }

        public void SwipedLeft()
        {
              HandleGebruiker();
        }

        public void SwipedRight()
        {
            //voeg persoon toe aan flitsed tabel
            if (Gebruikers[0].AccountID == -2)
            {
                HandleGebruiker();
            }
            else
            { // zet de match in de flitsed tabel van de database
                DB.InsertQueryAsync("INSERT INTO Flitsed VALUES (@Value1, @Value2)", $"{AccountID}", $"{Gebruikers[0].AccountID}");
                HandleGebruiker();
            }
        }

        private void FillGebruikers()
        {
            //haal een gesorteerde lijst uit het algoritme
            List<Gebruiker> temp = algor.getPeople();
            foreach (Gebruiker gebruiker in temp)
            {
                //zet de lijst in een observableproperty
                Gebruikers.Add(gebruiker);
            }
            //indien er nog niet geswiped is: zet de eerste gebruiker klaar
            if (Gebruikers[0].AccountID != 0 && FirstSwipe)
            {
                Gebruikers.Insert(0, new Gebruiker());
                FirstSwipe = false;
            }
        }

        public void ResetGebruikers()
        { //als de lijst een account bevat. maak dan een nieuwe gebruiker aan en vul die met UpdateProperties
            if (Gebruikers[0].AccountID != 0)
            {
                Gebruikers.Insert(0, new Gebruiker());
            }
            UpdateProperties();
        }

        //ga naar de volgende gebruiker
        public void HandleGebruiker()
        {
            Gebruikers.RemoveAt(0);
            // pak de volgende gebruiker
            if (Gebruikers.Count > 1)
            {
                CurrentGebruiker = Gebruikers[0];
            }
            else
            {
            //als gebruikers leeg is: vul gebruikers opnieuw
                FillGebruikers();
            }
            
            UpdateProperties();
        }
        //haal info van een gebruiker uit de database
        public void UpdateProperties()
        {
            
            AccountID = Preferences.Default.Get("AccountID", -1);
            if (User != AccountID)
            {
                User = AccountID;
                Gebruikers.Clear();
                FirstSwipe = true;
                FillGebruikers();
            }
            CurrentGebruiker = Gebruikers[0];
            FirstName = CurrentGebruiker.FirstName.Trim();
            Pic = CurrentGebruiker.ProfilePic;
            Age = CurrentGebruiker.Age;
            Description = CurrentGebruiker.Description.Trim();
            City = CurrentGebruiker.City;
            FirstnameAndAge = FirstName.Trim() + ", " + Age.ToString();
            Hobby1 = Hobbies[CurrentGebruiker.Hobby1].Trim();
            Hobby2 = Hobbies[CurrentGebruiker.Hobby2].Trim();
            Hobby3 = Hobbies[CurrentGebruiker.Hobby3].Trim();
            
            /*
            Deze checkt zorgt ervoor dat op de 'swipe om te beginnen' scherm geen -1 komt te staan.
            Ook verbergt dit de hobby's, beschrijving en naam
            */
            if (Age < 18)
            {
                FirstnameAndAge = FirstName;
                HobbyHide = 0;
            }
            else
            {
                HobbyHide = 1;
            }

            if (Description.Length <= 1)
            {
                DescriptionBackground = 0;
            }
            else
            {
                DescriptionBackground = 0.75;
            }
        }

    }
}
