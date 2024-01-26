using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Flitsflirt.App.Models;
using Flitsflirt.App.Services;
using Flitsflirt.App.Views;
using Microsoft.Maui.Controls;


namespace Flitsflirt.App.ViewModels
{
    public partial class AccountViewModel : ObservableObject
    {
        IDBConnect DB;
        IUser IU;
        IHTTP _client;

        public AccountViewModel(IDBConnect DB, IUser IU, IHTTP client)
        {
            this.DB = DB;
            this.IU = IU;
            this._client = client;
            init();
        }

//maakt een nieuwe gebruiker model aan en vult deze met de huidige gebruikers data zodat dit opgehaald kan worden op de account view.
        public void init()
        {
            int User = Preferences.Default.Get("AccountID", -1);
            Gebruiker test = new Gebruiker();
            test = IU.maakGebruiker(User);
            FirstName = test.FirstName;
            Age = test.Age;
            City = test.City;
            Description = test.Description.Trim();
            FirstnameAndAge = test.FirstName.Trim() + ", " + test.Age.ToString();
            getHobbies(test);
            ImgSource = _client.GetImage(User);
            if (Description.Length <= 1)
            {
                DescriptionBackground = 0;
            }
            else
            {
                DescriptionBackground = 0.75;
            }

        }
        
//haalt hobby's op uit database en vult de lokale variabelen
        private void getHobbies(Gebruiker user)
        {
            Task<string> h1 = DB.ReadQueryAsync($"SELECT HobbyName FROM Hobby WHERE HobbyID = {user.Hobby1+1}");
            Task<string> h2 = DB.ReadQueryAsync($"SELECT HobbyName FROM Hobby WHERE HobbyID = {user.Hobby2+1}");
            Task<string> h3 = DB.ReadQueryAsync($"SELECT HobbyName FROM Hobby WHERE HobbyID = {user.Hobby3+1}");
            
            
            Hobby1 = h1.Result.Trim();
            Hobby2 = h2.Result.Trim();
            Hobby3 = h3.Result.Trim();
        }

//lokale variable waarmee de voornaam opgehaald kan worden in de view
        [ObservableProperty]
        public string firstName = "";
        
//lokale variable waarmee de leeftijd opgehaald kan worden in de view
        [ObservableProperty]
        public int age;

//lokale variable waarmee de voornaam en leeftijd opgehaald kan worden in de view
        [ObservableProperty]
        public string firstnameAndAge = "";

//lokale variable waarmee de stad opgehaald kan worden in de view
        [ObservableProperty]
        public string city = "";
        
 //lokale variable waarmee de beschrijving opgehaald kan worden in de view
        [ObservableProperty]
        public string description = "";

//lokale variable waarmee de eerste hobby opgehaald kan worden in de view
        [ObservableProperty]
        public string hobby1 = "";

//lokale variable waarmee de tweede hobby opgehaald kan worden in de view
        [ObservableProperty]
        public string hobby2 = "";

//lokale variable waarmee de derde hobby opgehaald kan worden in de view
        [ObservableProperty]
        public string hobby3 = "";

//lokale variable waarmee de gebruiker foto opgehaald kan worden in de view
        [ObservableProperty]
        public string imgSource = "";

//lokale variable waarmee de opacity van de beschrijving blok wordt bepaald in de view
        [ObservableProperty]
        public double descriptionBackground;

//routing
        [RelayCommand]
        private async void Edit()
        {
            await Shell.Current.GoToAsync($"//{nameof(EditPage)}");
        }
    }
}
