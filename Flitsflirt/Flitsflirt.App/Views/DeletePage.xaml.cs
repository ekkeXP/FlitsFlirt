using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Flitsflirt.App.ViewModels;

namespace Flitsflirt.App.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class DeletePage : ContentPage
{
    public DeletePage(DeleteViewModel DV)
    {
        InitializeComponent();
        BindingContext = DV;
    }
}