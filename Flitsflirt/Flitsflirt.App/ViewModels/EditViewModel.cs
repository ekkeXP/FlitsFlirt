using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Flitsflirt.App.Services;
using Flitsflirt.App.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Identity.Client;

namespace Flitsflirt.App.ViewModels
{
    public partial class EditViewModel : ObservableObject
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        IDBConnect DB;
        IHTTP _client;

        int AccountID = Preferences.Default.Get("AccountID", -1);

//haalt data uit de nieuwe wachtwoord entry op en vult deze lokale variabel
        [ObservableProperty]
        public string enteredPass = "";
        
//haalt data uit de herhaal wachtwoord entry op en vult deze lokale variabel
        [ObservableProperty]
        public string enteredConfirm = "";

//haalt data uit de beschrijving entry op en vult deze lokale variabel
        [ObservableProperty]
        public string enteredDescription = "";

//haalt data uit de hobby ids op uit de picker en vult deze lokale variabel
        [ObservableProperty]
        int pickerHobby1;

        [ObservableProperty]
        int pickerHobby2;

        [ObservableProperty]
        int pickerHobby3;

//opgehaalde hobby's plaatsen in een lijst zodat ze uitgelezen kunnen worden in de picker
        [ObservableProperty]
        List<string> itemList = null;

        public EditViewModel(IDBConnect DB, IHTTP client)
        {
            this.DB = DB;
            _client = client;
        }

//haalt beschrijving en hobby's op uit de database die bij de gebruiker horen
        public async void Load()
        {
            AccountID = Preferences.Default.Get("AccountID", -1);
            EnteredDescription = await DB.GetDescription();
            PickerHobby1 = int.Parse(await DB.ReadQueryAsync($"SELECT HobbyID1 FROM Account_Hobbies WHERE AccountID = {AccountID}"))-1;
            PickerHobby2 = int.Parse(await DB.ReadQueryAsync($"SELECT HobbyID2 FROM Account_Hobbies WHERE AccountID = {AccountID}"))-1;
            PickerHobby3 = int.Parse(await DB.ReadQueryAsync($"SELECT HobbyID3 FROM Account_Hobbies WHERE AccountID = {AccountID}"))-1;
        }
//verstuurt foto naar server van de gebruiker
        [RelayCommand]
        private async void UploadFoto()
        {
            string id = AccountID.ToString();
            await _client.SendPhotoToServer(id);
        }

        [RelayCommand]
        private async void Back()
        {
            await Shell.Current.GoToAsync($"//{nameof(AccountPage)}");
        }
//forceer een gebruik om een foto te hebben voordat die door kan naar een andere pagina
        [RelayCommand]
        private async void CheckprofilePicture()
        {
            AccountID = Preferences.Default.Get("AccountID", -1);
            if (!await _client.ImageExists(AccountID))
            {
                DB.MakeToast("Upload een foto als profielfoto");
                await Shell.Current.GoToAsync($"//{nameof(EditHobbiesPage)}");
            }
        }

//update de hobby's in de database en routing
        [RelayCommand]
        private async void UpdateHobbies()
        {
            if (PickerHobby1 == PickerHobby2 || PickerHobby1 == PickerHobby3 || PickerHobby2 == PickerHobby3)
            {
                DB.MakeToast("U moet 3 verschillende hobbies kiezen");
            }
            else
            {
                CheckprofilePicture();
                await DB.UpdateQueryAsync($"UPDATE Account_Hobbies SET HobbyID1 = '{PickerHobby1+1}', HobbyID2 = '{PickerHobby2+1}', HobbyID3 = '{PickerHobby3+1}' WHERE AccountID = {AccountID}");
                await DB.UpdateQueryAsync($"UPDATE Account_Data SET Description = '{EnteredDescription}' WHERE AccountID = {AccountID}");
                await Shell.Current.GoToAsync($"//{nameof(AccountPage)}");
            }
       
        }
//update de gebruikers wachtwoord en routing
        [RelayCommand]
        private async void UpdateData()
        {
            if (PasshasSpecialChar(EnteredPass))
            {
                DB.MakeToast("Wachtwoord bevat een van de volgende tekens: \\|/(){},\"");
            }
            else if (EnteredPass.Length < 8) 
            {
                DB.MakeToast("Uw wachtwoord is niet lang genoeg. Minimaal 8 tekens");
            }
            else if (!IsEqual(EnteredPass, EnteredConfirm))
            {
                DB.MakeToast("Uw wachtwoord is niet hetzelfde als in de herhaal check.");
            }
            else
            {
                await DB.UpdateQueryAsync($"UPDATE Account SET Password = '{EnteredPass}' WHERE AccountID = {AccountID}");
                await Shell.Current.GoToAsync($"//{nameof(AccountPage)}");
                CheckprofilePicture();
            }
        }
//kijkt of je geen speciale karakters gebruikt in de entry
        public static bool PasshasSpecialChar(string input)
        {
            string specialChar = "\\|/(){},\"";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }
//kijkt of je wachtwoorden overeen komen
        public static bool IsEqual(string str1, string str2)
        {
            return string.Equals(str1, str2, StringComparison.Ordinal);
        }
//routing
        [RelayCommand]
        private async void Edit()
        {
            await Shell.Current.GoToAsync($"//{nameof(EditPage)}");
            CheckprofilePicture();
        }
//routing en haalt hobby's op voor de picker
        [RelayCommand]
        private async void EditHobbies()
        {

            ItemList = DB.GetHobbies();
            await Shell.Current.GoToAsync($"//{nameof(EditHobbiesPage)}");
        }
//routing
        [RelayCommand]
        private async void EditInfo()
        {
            await Shell.Current.GoToAsync($"//{nameof(EditInfoPage)}");
        }
//routing
        [RelayCommand]
        private async void Tutorial()
        {
            await Shell.Current.GoToAsync($"//{nameof(TutorialPage)}");
            CheckprofilePicture();
        }
//routing
        [RelayCommand]
        private async void Delete()
        {
            await Shell.Current.GoToAsync($"//{nameof(DeletePage)}");
            CheckprofilePicture();
        }
//wist alle gegevens uit de cache van de gebruiker en stuurt je terug naar het login scherm
        [RelayCommand]
        private async void Logout()
        {
            Preferences.Default.Set("Username", "");
            Preferences.Default.Set("Password", "");
            Preferences.Default.Set("Remember", false);
            Preferences.Default.Set("AccountID", -1);
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
