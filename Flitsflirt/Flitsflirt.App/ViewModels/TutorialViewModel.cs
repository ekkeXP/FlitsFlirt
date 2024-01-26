using CommunityToolkit.Mvvm.ComponentModel;
using Flitsflirt.App.Models;
using Flitsflirt.App.Views;
using Flitsflirt.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OnboardingScreenUI.ViewModels;
using Flitsflirt.App.Services;

namespace Flitsflirt.App.ViewModels
{
    public class TutorialViewModel : BaseViewModel
    {
        #region Properties

        private string _buttonVisibility = "False";
        public string ButtonVisibility
        {
            get => _buttonVisibility;
            set => SetProperty(ref _buttonVisibility, value);
        }

        
// Toon button wanneer gebruiker op laatste pagina is        
        private int _position;
        public int Position
        {
            get => _position;
            set => SetProperty(ref _position, value, onChanged: (() =>
            {
                if (value == TutorialSchermen.Count - 1)
                {
                    ButtonVisibility = "True";
                }
                else
                {
                    ButtonVisibility = "False";
                }
            }));
        }

        public ObservableCollection<TutorialModel> TutorialSchermen { get; set; } = new ObservableCollection<TutorialModel>();
        #endregion



        public TutorialViewModel()
        {
// Voeg alle schermen toe
            TutorialSchermen.Add(new TutorialModel
            {
                TutorialTitle = "Welkom bij Flitsflirt!",
                TutorialDescription = "Swipe om te beginnen 👉 ",
                TutorialImage = "//Resources/Images/flitsflirt_logo.svg"
            });

            TutorialSchermen.Add(new TutorialModel
            {
                TutorialTitle = "Swipen maar!",
                TutorialDescription = $"Swipe naar rechts om iemand te liken of naar links om ze over te slaan.",
                TutorialImage = "tut1.png"
            });

            TutorialSchermen.Add(new TutorialModel
            {
                TutorialTitle = "Goed bezig!",
                TutorialDescription = $"Als ze ook naar rechts op jou swipen heb je een flits!",
                TutorialImage = "tut1.png"
            });

            TutorialSchermen.Add(new TutorialModel
            {
                TutorialTitle = "Maar let op!",
                TutorialDescription = $"Als je iemand niet swiped voordat de tijdbalk leeg loopt sla je ze automatisch over!",
                TutorialImage = "tut2.png"
            });

            TutorialSchermen.Add(new TutorialModel
            {
                TutorialTitle = "Dat was hem!",

                TutorialDescription = $"Je vind je flitsen op de match pagina, linksonder in de app.",
                TutorialImage = "tut3.png"
            });
        }


// Button command
        public ICommand ButtonCommand => new Command(async () =>
        {
            if (Position >= TutorialSchermen.Count - 1)
            {
                await AppShell.Current.GoToAsync($"//{nameof(FlitsPage)}");
            }
            else
            {
                Position += 1;
            }
        });
    }
}
