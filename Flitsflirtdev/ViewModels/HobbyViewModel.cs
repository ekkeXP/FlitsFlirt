using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Flitsflirtdev.Services;
using Flitsflirtdev.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;


namespace Flitsflirtdev.ViewModels
{

    public partial class HobbyViewModel : ObservableObject
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IDBConnect DB;

        [ObservableProperty]
        List<string> itemList = null;


        [ObservableProperty]
        int updateHobbyID = -1;

        [ObservableProperty]
        string updateHobby = "";

        [ObservableProperty]
        public string nieuweHobby = "";

        [ObservableProperty]
        int deleteHobbyID = -1;

        public HobbyViewModel(IDBConnect DB)
        {
            ItemList = DB.GetHobbies();
            this.DB = DB;
        }

// Pas hobby in db aan
        [RelayCommand]
        private async void EditHobby(Object obj)
        {
            UpdateHobbyID++;
            await DB.UpdateQueryAsync($"UPDATE Hobby SET HobbyName = '{UpdateHobby}' WHERE HobbyID = '{UpdateHobbyID}' ");
            MakeToast($"Hobby bijgewerkt!");
            ItemList = DB.GetHobbies();
            UpdateHobbyID = -1;
        }
        
// Voeg hobby toe aan db
        [RelayCommand]
        private async void NewHobby(Object obj)
        {
            await DB.InsertQueryAsync($"INSERT INTO Hobby (HobbyName) VALUES ('{NieuweHobby}')");
            MakeToast($"Hobby toegevoegd!");

        }


// Routing
        [RelayCommand]
        private async void Algo()
        {
            await Shell.Current.GoToAsync($"//{nameof(AlgoPage)}");
        }

// Routing
        [RelayCommand]
        private async void Logout()
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

// Check op speciale characters
        public static bool hasSpecialChar(string input)
        {
            string specialChar = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghikjlmnopqrstuvwxyz\|!#$%&/()=?»«@£§€{}.-;'<>_, ";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }

// Check op speciale characters in wachtwoord
        public static bool PasshasSpecialChar(string input)
        {
            string specialChar = "\\|/(){},\"";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }

// Maak toast aan
        public async void MakeToast(string text)
        {
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;
            var toast = Toast.Make(text, duration, fontSize);
            await toast.Show(cancellationTokenSource.Token);
        }

    }
}
