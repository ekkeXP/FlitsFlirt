using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Flitsflirtdev.Services;
using Flitsflirtdev.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Flitsflirtdev.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        IDBConnect DB;
        [ObservableProperty]
        public string enteredUser = "";

        [ObservableProperty]
        public string enteredPass = "";

        int ID;

        public LoginViewModel(IDBConnect DB)
        {
            this.DB = DB;
        }

// Check ingevoerde gegevens op hardcoded gegevens
        [RelayCommand]
        private async void Login()
        {
            if (UserhasSpecialChar(EnteredUser))
            {
               MakeToast("Gebruikersnaam mag geen speciale tekens bevatten");

            }else if (PasshasSpecialChar(EnteredPass))
            {
                MakeToast("Wachtwoord mag geen \\|/(){}, of \" bevatten");
            }
            else
            {

                string checkUser = "flitsflirtdev";
                string checkPass = "flitsdev321";
                
                if ((checkUser != "" && checkPass != "") && IsEqual(EnteredPass, checkPass) && IsEqual(EnteredUser, checkUser))
                {
                    await Shell.Current.GoToAsync($"//{nameof(AlgoPage)}");
                    
                }
                else
                {
                    MakeToast("Combinatie van Gebruikersnaam en Wachtwoord is niet juist");
                }
            }
            

        }

// Check op speciale characters
        public static bool UserhasSpecialChar(string input)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }
        
// Check op speciale characters bij wachtwoord
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

// Equals methode
        public static bool IsEqual(string str1, string str2)
        {
            return string.Equals(str1, str2, StringComparison.Ordinal);
        }
    }
}
