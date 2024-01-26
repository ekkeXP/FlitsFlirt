using CommunityToolkit.Maui.Core.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.Input;
using Flitsflirt.App.Services;
using CommunityToolkit.Maui.Alerts;
using Flitsflirt.App.Views;
using CommunityToolkit.Maui.Core;

namespace Flitsflirt.App.ViewModels
{
    public partial class DeleteViewModel : ObservableObject
    {
        private IDBConnect DB;
        private string _usernameCheck = ""; //voor de gebruikersnaam uit de database opslaan
        private string id = Preferences.Get("AccountID", -1).ToString(); //accountID ophalen uit preferences

        // om het resultaat uit de Entry vast te houden
        [ObservableProperty]
        private string entryString = "";
        
        //database verbinding
        public DeleteViewModel(IDBConnect DB)
        {
            this.DB = DB;
        }

        [RelayCommand]
        private async void BackOut(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(AccountPage)}");
        }

        [RelayCommand]
        private async void Delete(object obj)
        { 
            //haal de gebruikersnaam van DIT account op uit de DB
           _usernameCheck = (await DB.ReadQueryAsync("SELECT Username FROM Account WHERE AccountID = @Value1", id)).Trim();
           //check of de gebruikersnaam uit de database overeenkomt met de gebruikersnaam aan uit de entry
            if (_usernameCheck.Equals(EntryString))
            {
                //als het bovenstaande klopt, verwijder dan uit de database
                await DB.DeleteQueryAsync("DELETE FROM Account WHERE AccountID = @Value1", id);
                await DB.DeleteQueryAsync("DELETE FROM Account_Data WHERE AccountID = @Value1", id);
                await DB.DeleteQueryAsync("DELETE FROM Account_Hobbies WHERE AccountID = @Value1", id);
                await DB.DeleteQueryAsync("DELETE FROM Flitsed WHERE AccountID = @Value1 OR FlitsedAccountID = @Value1", id);


                //na het verwijderen, terug naar loginpage
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            } 
            else
            {
                //als de gebruikersnaam niet klopt, laat een error zien
                DB.MakeToast("Ingevoerde gebruikersnaam komt niet overheen.");
            }
        }

    }
}
