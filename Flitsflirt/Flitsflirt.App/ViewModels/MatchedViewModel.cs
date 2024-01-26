using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Flitsflirt.App.Models;
using Flitsflirt.App.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flitsflirt.App.ViewModels
{
    public partial class MatchedViewModel : ObservableObject
    {
        private IDBConnect DB;
        private IUser IU;
        public IHTTP _client;

        int User;

        List<int> AlreadyLoaded = new List<int>();

//lokale variabel die een lijst maakt met accounts
        [ObservableProperty]
        List<Gebruiker> loadingList = new List<Gebruiker>();


        public MatchedViewModel(IDBConnect DB, IUser IU, IHTTP client)
        {
            this.DB = DB;
            this.IU = IU;
            this._client = client;
        }

//haal alle matches op uit de database
        public void getMatch()
        {
            int user = Preferences.Default.Get("AccountID", -1);
            List<int> matchIDQuery = new List<int>();
            matchIDQuery = DB.GetIntListFromTable($"SELECT t1.FlitsedAccountID FROM flitsed t1 INNER JOIN flitsed t2 ON t1.AccountID = t2.FlitsedAccountID AND t1.FlitsedAccountID = t2.AccountID WHERE (t1.AccountID <> t1.FlitsedAccountID) AND t1.AccountID = {user}");

            if (User!=user)
            {
                User = user;
                AlreadyLoaded.Clear();
                LoadingList.Clear();
            }

            for (int i = 0; i < matchIDQuery.Count; i++)
            {
                Gebruiker temp = IU.maakGebruiker(matchIDQuery[i]);
                
                if (temp.AccountID != user && !AlreadyLoaded.Contains(temp.AccountID)) {
                    LoadingList.Add(temp);
                    AlreadyLoaded.Add(temp.AccountID);
                }
            }
        }       

    }
}
