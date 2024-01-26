using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Flitsflirt.App.Services;
using Flitsflirt.App.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace Flitsflirt.App.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {

        IDBConnect DB;
        IHTTP _http;

        //String voor het ophalen van de ingevulde gebruikersnaam
        [ObservableProperty]
        private string enteredUser = "";
        //String voor het ophalen van het ingevulde wachtwoord
        [ObservableProperty]
        private string enteredPass = "";
        //Bool voor het ophalen van de checkbox status
        [ObservableProperty]
        private bool remember;

        //Als Remember == true probeer in te loggen
        public LoginViewModel(IDBConnect DB, IHTTP HTTP)
        {
            this.DB = DB;
            if (Preferences.Default.Get("Remember", false))
            {
                EnteredUser = Preferences.Default.Get("Username", "");
                EnteredPass = Preferences.Default.Get("Password", "");
                Login();
            }
            this._http = HTTP;
        }

        //Functie voor het inloggen
        [RelayCommand]
        private async void Login()
        {
            if (EnteredUser.Length < 1)
            {
                DB.MakeToast("Vul alstublieft een gebruikersnaam in");
            }
            else if (EnteredPass.Length < 1)
            {
                DB.MakeToast("Vul alstublieft een wachtwoord in");
            }
            else if (UserhasSpecialChar(EnteredUser))
            {
                DB.MakeToast("Gebruikersnaam mag geen speciale tekens bevatten");

            }
            else if (PasshasSpecialChar(EnteredPass))
            {
                DB.MakeToast("Wachtwoord mag geen \\|/(){}, of \" bevatten");
            }
            else
            {

                if (await CheckLogin())
                {

                    Preferences.Default.Set("AccountID", int.Parse(await DB.ReadQueryAsync("SELECT AccountID FROM Account WHERE Username = @Value1", EnteredUser)));
                    if (Remember || Preferences.Default.Get("Remember", false))
                    {
                        Preferences.Default.Set("Username", EnteredUser);
                        Preferences.Default.Set("Password", EnteredPass);
                        Preferences.Default.Set("Remember", true);
                    }
                    else
                    {
                        Preferences.Default.Set("Username", "");
                        Preferences.Default.Set("Password", "");
                        Preferences.Default.Set("Remember", false);
                    }
                    await Shell.Current.GoToAsync($"//{nameof(FlitsPage)}");

                }
                else
                {
                    DB.MakeToast("Combinatie van Gebruikersnaam en Wachtwoord is niet juist");
                    Preferences.Default.Set("Remember", false);
                }
            }


        }
        //Functie voor het checken van de ingevulde gegevens
        private async Task<bool> CheckLogin()
        {
            byte[] Salt = Convert.FromBase64String((await DB.ReadQueryAsync("SELECT Salt FROM Account WHERE Username = @Value1", EnteredUser)).Trim());
            string Pass = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: EnteredPass!,
                    salt: Salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));

            string checkUser = (await DB.ReadQueryAsync("SELECT Username FROM Account WHERE Username = @Value1 AND Password = @Value2", EnteredUser, Pass)).Trim();
            string checkPass = (await DB.ReadQueryAsync("SELECT Password FROM Account WHERE Username = @Value1 AND Password = @Value2", EnteredUser, Pass)).Trim();

            if (await DB.IsEqual(Pass, checkPass) && await DB.IsEqual(EnteredUser, checkUser))
            {
                return await Task.FromResult(true);
            }
            else
            {
                return await Task.FromResult(false);
            }
        }
        //Routing naar de registerpagina
        [RelayCommand]
        private async void Register()
        {
            await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
        }
        //Functie om te checken of er speciale tekens in de gebruikersnaam staan
        public static bool UserhasSpecialChar(string input)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }
        //Functie om te checken of er speciale tekens in het wachtwoord staan
        public static bool PasshasSpecialChar(string input)
        {
            string specialChar = "\\|/(){},\"";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }
    }
}
