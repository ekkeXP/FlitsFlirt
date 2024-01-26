using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Flitsflirt.App.Services;
using Flitsflirt.App.Views;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using static System.Net.WebRequestMethods;


namespace Flitsflirt.App.ViewModels
{

    public partial class RegisterViewModel : ObservableObject
    {
        IDBConnect DB;
        IHTTP _client;

        #region Properties
        //String voor het ophalen van de gebruikersnaam
        [ObservableProperty]
        string enteredUser = "";
        //String voor het ophalen van de email
        [ObservableProperty]
        string enteredEmail = "";
        //String voor het ophalen van het wachtwoord
        [ObservableProperty]
        string enteredPass = "";
        //String voor het ophalen van het check wachtwoord
        [ObservableProperty]
        string enteredConfirm = "";
        //int voor het opslaan van het AccountID
        private int AccountID;
        //int voor het opslaan van de leeftijd
        private int Age;
        //String voor het ophalen van de voornaam
        [ObservableProperty]
        string enteredFirstName = "";
        //DateTime voor het ophalen van ingevulde datum
        [ObservableProperty]
        DateTime enteredBirthDate = new DateTime(2000, 1, 1);
        //String voor het ophalen van de ingevulde stad
        [ObservableProperty]
        string enteredCity = ".";
        //Int voor het ophalen van het ingevulde gender
        [ObservableProperty]
        int enteredGender = -1;
        //Functie voor het veranderen van de int naar een gender char
        private char Gender
        {
            get
            {
                if (EnteredGender == 0)
                {
                    return 'M';
                }
                else if (EnteredGender == 1)
                {
                    return 'V';
                }
                else
                {
                    return '.';
                }
            }
        }
        //Int voor het ophalen van de ingevulde voorkeur
        [ObservableProperty]
        int enteredPreference = -1;
        //Functie voor het veranderen van de int naar een voorkeur char
        private char Pref
        {
            get
            {
                if (EnteredPreference == 0)
                {
                    return 'M';
                }
                else if (EnteredPreference == 1)
                {
                    return 'V';
                }
                else if (EnteredPreference == 2)
                {
                    return 'X';
                }
                else
                {
                    return '.';
                }
            }
        }
        //Int voor het ophalen van de ingevulde hobby1
        [ObservableProperty]
        int pickerHobby1 = -1;
        //Int voor het ophalen van de ingevulde hobby2
        [ObservableProperty]
        int pickerHobby2 = -1;
        //Int voor het ophalen van de ingevulde hobby3
        [ObservableProperty]
        int pickerHobby3 = -1;
        //List voor het binden van de Hobbylijst
        [ObservableProperty]
        List<string>? itemList;
        //List met meest bekende steden in nederland
        [ObservableProperty]
        List<string> cities = new List<string>
        {
            "Arnhem",
            "Assen",
            "Den Bosch",
            "Den Haag",
            "Groningen",
            "Haarlem",
            "Leeuwarden",
            "Lelystad",
            "Maastricht",
            "Middelburg",
            "Utrecht",
            "Zwolle"
        };

        #endregion
        //Minimale wachtwoordlengte
        #region Gegevenseisen
        private int MinPassLength = 8;
        #endregion

        DateTime zeroTime = new DateTime(1, 1, 1);

        public RegisterViewModel(IDBConnect DB, IHTTP Http)
        {
            this.DB = DB;
            _client = Http;
        }
        //Routing naar login pagina
        [RelayCommand]
        private async void LoginPage()
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
        //Routing naar register pagina
        [RelayCommand]
        private async void RegisterPage()
        {
            await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
        }
        //Functie die de ingevulde gegevens checkt + Routing naar register persoon pagina
        [RelayCommand]
        private async void RegisterPerson()
        {
            //check if user already exists
            if (await DB.UserExists(EnteredUser))
            {
                DB.MakeToast("Gebruikersnaam bestaat al. Kies een andere");
            }
            else if (hasSpecialChar(EnteredUser))
            {
                DB.MakeToast("Gebruikersaam mag geen speciale tekens bevatten");
            }
            else if (EnteredUser.Length < 1)
            {
                DB.MakeToast("Voer een gebruikersnaam in");
            }
            else if (!IsValidEmail(EnteredEmail))
            {
                DB.MakeToast("E-mail is niet goed getypet.");
            }
            else if (EnteredEmail.Length < 1)
            {
                DB.MakeToast("Voer een Email-adres in");
            }
            else if (EnteredPass.Length < MinPassLength)
            {
                DB.MakeToast("Uw wachtwoord is niet lang genoeg. Minimaal 8 tekens");
            }
            else if (PasshasSpecialChar(EnteredPass))
            {
                DB.MakeToast("Uw wachtwoord mag de volgende tekens niet bevatten: \\|/(){},\" ");
            }
            else if (!await DB.IsEqual(EnteredPass, EnteredConfirm))
            {   //check if enteredPass == enteredConfirm
                DB.MakeToast("Uw wachtwoord is niet hetzelfde als in de herhaal check.");
            }
            else
            {
                await Shell.Current.GoToAsync($"//{nameof(RegisterPersonPage)}");
            }


        }
        //Functie die de ingevulde gegevens checkt + Routing naar register hobbies pagina
        [RelayCommand]
        private async void RegisterHobbies()
        {
            //Check of voornaam gekke characters bevat -> TOAST MELDING
            if (hasSpecialChar(EnteredFirstName))
            {
                DB.MakeToast("Uw opgegeven voornaam mag geen speciale tekens bevatten.");
            }
            else if (EnteredFirstName.Length < 1)
            {
                DB.MakeToast("U heeft uw voornaam nog niet ingevuld");
            }
            else
            {
                //Check geboortedag 18+ -> TOAST MELDING
                TimeSpan span = DateTime.Now - EnteredBirthDate;
                int years = (zeroTime + span).Year - 1;
                Age = years;
                if (years < 18)
                {
                    DB.MakeToast("Minimale leeftijd: 18 jaar. Geen toegang onder 18.");
                }
                else
                {
                    ItemList = DB.GetHobbies();
                    if (!Cities.Contains(EnteredCity))
                    {
                        DB.MakeToast("U heeft uw Woonplaats nog niet ingevuld");
                    }
                    else if (Gender == '.')
                    {
                        DB.MakeToast("U heeft uw gender nog niet ingevuld");
                    }
                    else if (Pref == '.')
                    {
                        DB.MakeToast("U heeft uw voorkeur nog niet ingevuld");
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(RegisterHobbiesPage)}");
                    }


                }
            }
        }

        //Functie die de ingevulde gegevens checkt + verstuurt naar database + Routing naar tutorial pagina
        [RelayCommand]
        private async void Register(object obj)
        {
            //check of hobby 1, 2 en 3 verschillen
            if (PickerHobby1 == PickerHobby2 || PickerHobby1 == PickerHobby3 || PickerHobby2 == PickerHobby3)
            {
                DB.MakeToast("U moet 3 verschillende hobbies kiezen");
            }
            else if (PickerHobby1 == -1 || PickerHobby2 == -1 || PickerHobby3 == -1)
            {
                DB.MakeToast("U hebt niet alle hobbies ingevuld");
            }
            else
            {
                //maak nieuwe entry voor "Account"
                byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
                var EnteredSalt = Convert.ToBase64String(salt);

                string HashedPass = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: EnteredPass!,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));

                await DB.InsertQueryAsync($"INSERT INTO Account VALUES ('{EnteredUser}', '{HashedPass}', '{EnteredSalt}')");
                //Get AccountID
                AccountID = int.Parse(await DB.ReadQueryAsync("SELECT AccountID FROM Account WHERE Username = @Value1", EnteredUser));
                //maak nieuwe entry voor "Account_Data"
                await DB.InsertQueryAsync($"INSERT INTO Account_Data VALUES ('{AccountID}', '{EnteredFirstName}', '{Age}', '{EnteredEmail}', '{Gender}', '{Pref}', '{EnteredCity}', ' ')");
                //maak nieuwe entry voor "Account_Hobbies"
                await DB.InsertQueryAsync($"INSERT INTO Account_Hobbies VALUES ('{AccountID}', '{PickerHobby1+1}', '{PickerHobby2+1}', '{PickerHobby3+1}')");
                Preferences.Default.Set("AccountID", AccountID);
                await Shell.Current.GoToAsync($"//{nameof(TutorialPage)}");
                //await Application.Current.MainPage.Navigation.PushAsync(new TutorialPage());
            }
        }
        //Functie voor het checken op speciale tekens
        public static bool hasSpecialChar(string input)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_, ";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }
        //Functie voor het checken op speciale tekens in wachtwoord
        public static bool PasshasSpecialChar(string input)
        {
            string specialChar = "\\|/(){},\"";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }
        //Functie voor het checken op goede email
        public static bool IsValidEmail(string email)
        {
            string pattern = @"^[\w\.-]+@[\w\.-]+\.\w+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

    }
}
