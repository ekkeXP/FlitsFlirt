using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Flitsflirtdev.Services;
using Flitsflirtdev.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;


namespace Flitsflirtdev.ViewModels
{

    public partial class AlgoViewModel : ObservableObject
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IDBConnect DB;

        [ObservableProperty]
        public string enteredAgedifference = "";

        [ObservableProperty]
        public string enteredAge = "";

        [ObservableProperty]
        public string enteredCity = "";

        [ObservableProperty]
        public string enteredOnehobbysame = "";

        [ObservableProperty]
        public string enteredTwohobbysame = "";

        [ObservableProperty]
        public string enteredThreehobbysame = "";

        [ObservableProperty]
        public string enteredFlitsed = "";

        public AlgoViewModel(IDBConnect DB)
        {
            this.DB = DB;
        }

// Gelinkt aan de button op de algoritme pagina
// Verstuurt de ingevoerde punten naar de database
        [RelayCommand]
        private async void SubmitPoints(Object obj)
        {
            await DB.UpdateQueryAsync($"UPDATE AlgorithmPoints SET AgeDifference = '{EnteredAgedifference}'");
            await DB.UpdateQueryAsync($"UPDATE AlgorithmPoints SET Age = '{EnteredAge}'");
            await DB.UpdateQueryAsync($"UPDATE AlgorithmPoints SET City = '{EnteredCity}'");
            await DB.UpdateQueryAsync($"UPDATE AlgorithmPoints SET OneHobbySame = '{EnteredOnehobbysame}'");
            await DB.UpdateQueryAsync($"UPDATE AlgorithmPoints SET TwoHobbySame = '{EnteredTwohobbysame}'");
            await DB.UpdateQueryAsync($"UPDATE AlgorithmPoints SET ThreeHobbySame = '{EnteredThreehobbysame}'");
            await DB.UpdateQueryAsync($"UPDATE AlgorithmPoints SET Flitsed = '{EnteredFlitsed}'");
            MakeToast($"Algoritme bijgewerkt!");
                
        }


// Routing
        [RelayCommand]
        private async void Hobby()
        {
            await Shell.Current.GoToAsync($"//{nameof(HobbyPage)}");
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

// Check wachtwoord op speciale characters
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
