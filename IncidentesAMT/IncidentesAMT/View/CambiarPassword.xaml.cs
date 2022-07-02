using IncidentesAMT.Model;
using IncidentesAMT.Modelo;
using IncidentesAMT.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IncidentesAMT.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CambiarPassword : ContentPage
    {
        public CambiarPassword(InfoUserByIdModel cambiarPasswordModel, ImageSource fotoPerfil)
        {
            InitializeComponent();
            BindingContext = new CambiarPasswordViewModel(Navigation, cambiarPasswordModel, fotoPerfil);
        }
    }
}